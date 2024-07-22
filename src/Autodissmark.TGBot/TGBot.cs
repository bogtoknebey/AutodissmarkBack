using Autodissmark.TGBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using System.Text;
using Autodissmark.TGBot.TgSettings.Options;
using Autodissmark.TGBot.Autogeneration;
using Autodissmark.TGBot.TgSettings;
using Autodissmark.TGBot.UserChats;

namespace Autodissmark.TGBot;

public class TgBot
{
    private const int DailyAttemptsReloadTimeInHours = 12;

    private const string StartCommand = "/start";
    private const string ResetCommand = "/reset";
    private const string HelpCommand = "/help";
    private const string SaveCommand = "/save";
    private const string AddTagetCommand = "/add target ";
    private const string RemoveTargetCommand = "/remove target ";

    private const string CallbackId_Target = "target";
    private const string CallbackId_Beat = "beat";
    private const string CallbackId_Voice = "voice";

    private const int Bot_CloseToRun_Delay = 0;
    private const int Bot_BetweenRestartAttemts_Delay = 300000;
    private const int Bot_AfterRestartIgnoring_Delay = 0;

    private DateTime _lastStartTime;
    private TgBotSettingsMaster _tgBotSettingsMaster;
    private UserChatsMaster _userChatsMaster;
    private ApiOptions _apiOptions;
    private AutogenerationOptions _autogenerationOptions;
    private TelegramOptions _telegramOptions;
    private AutogenerationLogic _autogenerationLogic;
    private Dictionary<long, UserChat> Chats;

    #region Set up block

    public TgBot() { }

    public async Task Setup()
    {
        _tgBotSettingsMaster = new TgBotSettingsMaster();

        _apiOptions = await _tgBotSettingsMaster.ReadApiOptions();
        _autogenerationOptions = await _tgBotSettingsMaster.ReadAutogenerationOptions();
        _telegramOptions = await _tgBotSettingsMaster.ReadTelegramOptions();

        _userChatsMaster = new UserChatsMaster();
        _autogenerationLogic = new AutogenerationLogic(_apiOptions);

        Chats = await _userChatsMaster.ReadChatsStates();
    }

    public void Run()
    {
        if (Chats is null)
        {
            throw new Exception("Cannot run tgBot because of empty Chats.");
        }

        var botClient = new TelegramBotClient(_telegramOptions.BotKey);
        botClient.StartReceiving(BotUpdate, BotError);
        _lastStartTime = DateTime.UtcNow;
    }

    #endregion

    #region Update routing

    private void AddChatIfThereNo(long chatId)
    {
        if (!Chats.ContainsKey(chatId))
        {
            Chats[chatId] = new UserChat()
            {
                Role = TgRole.User,
                Disable = false,
                LeftAttempts = _telegramOptions.Roles.User.Attempts,
                LastModify = DateTime.Today.AddHours(DailyAttemptsReloadTimeInHours)
            };
        }
    }

    private async Task BotUpdate(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        if (DateTime.UtcNow.AddMilliseconds(-Bot_AfterRestartIgnoring_Delay) < _lastStartTime)
        {
            return;
        }

        await _userChatsMaster.AddLog($"BotUpdate was called. Utc time: {DateTime.UtcNow}");

        if (update.Type == UpdateType.Message)
        {
            MessageUpdate(botClient, update, ct);
            return;
        }
        if (update.Type == UpdateType.CallbackQuery)
        {
            CallbackQueryUpdate(botClient, update, ct);
            return;
        }
    }

    private async Task MessageUpdate(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        var message = update.Message;
        var chatId = message!.Chat.Id;

        AddChatIfThereNo(chatId);

        var chat = Chats[chatId];
        chat.UnimportantMessages.Add(message.MessageId);

        if (Chats.ContainsKey(chatId) && Chats[chatId].Disable) 
        {
            return;
        }

        // Common commands
        if (message.Text == StartCommand)
        {
            // restore LeftAttempts every day
            if ((DateTime.Now - chat.LastModify) > TimeSpan.FromDays(1))
            {
                if (chat.Role == TgRole.User) 
                {
                    chat.LeftAttempts = _telegramOptions.Roles.User.Attempts;
                }

                if (chat.Role == TgRole.Admin) 
                {
                    chat.LeftAttempts = _telegramOptions.Roles.Admin.Attempts;
                }

                chat.LastModify = DateTime.Today.AddHours(DailyAttemptsReloadTimeInHours);

                await SendUniportentMessage(botClient, chatId, $"Хорошая новость, ваши попытки васстановлены у вас снова {chat.LeftAttempts} попыток!");
            }

            // check and count LeftAttempts
            if (chat.LeftAttempts <= 0)
            {
                await SendUniportentMessage(botClient, chatId, "У вас больше не осталось попыток!");
                return;
            }
            chat.LeftAttempts--;

            // do /start
            await SendUniportentMessage(botClient, chatId, "Вас приветсвтует Автодиссер!");
            await SendMarkup(botClient, chatId, _autogenerationOptions.Targets, CallbackId_Target, "Выберите вашу цель: ");
            return;
        }
        if (message.Text == ResetCommand)
        {
            await RemoveUnimportentMessages(botClient, chatId);
            return;
        }
        if (message.Text == HelpCommand)
        {
            StringBuilder description = new StringBuilder();
            description.Append("Основные команды:\n\n");
            description.Append("/help - список команд и их описания\n");
            description.Append
            (
                $"/start - запустить процесс создания дисса, " +
                $"существует лимит на данную команду: {_telegramOptions.Roles.User.Attempts} раз в сутки\n"
            );
            description.Append("/reset - удалить все ненужные сообщения (все кроме диссов)\n");

            if (chat.Role == TgRole.Admin)
            {
                description.Append("\nСпециальные команды:\n\n");
                description.Append("/save - сохранить состояния всех чатов (небходимо вызвать перед перезапуском)\n");
                description.Append("/add target {имя} - Добавть цель \n");
                description.Append("/remove target {имя} - Удалить цель\n");
            }

            await SendUniportentMessage(botClient, chatId, description.ToString());
            return;
        }

        // Specific commands (only for Admins)
        if (chat.Role != TgRole.Admin) 
        {
            return;
        }

        if (message.Text == SaveCommand)
        {
            string response;

            var savingRes = await _userChatsMaster.WriteChatsStates(Chats);

            if (savingRes)
            {
                response = "Сохранение прошло успешно!";
            }
            else
            {
                response = "Ошибка сохранения!";
            }

            await SendUniportentMessage(botClient, chatId, response);
            return;
        }
        if (message.Text.Substring(0, AddTagetCommand.Length) == AddTagetCommand)
        {
            try
            {
                var targetName = message.Text.Split(' ')[2];
                _autogenerationOptions.Targets.Add(targetName);

                await _tgBotSettingsMaster.WriteAutogenerationOptions(_autogenerationOptions);
                SendUniportentMessage(botClient, chatId, $"Имя {targetName} добавлено.");
            }
            catch
            {
                SendUniportentMessage(botClient, chatId, "Не правильный формат комманды. Используйте /help чтобы уточнить комманду.");
            }

            return;
        }
        if (message.Text.Substring(0, RemoveTargetCommand.Length) == RemoveTargetCommand)
        {
            try
            {
                var targetName = message.Text.Split(' ')[2];
                _autogenerationOptions.Targets.Remove(targetName);

                await _tgBotSettingsMaster.WriteAutogenerationOptions(_autogenerationOptions);
                SendUniportentMessage(botClient, chatId, $"Имя {targetName} удалено.");
            }
            catch
            {
                SendUniportentMessage(botClient, chatId, "Не правильный формат комманды или неправильный аргумент. Используйте /help чтобы уточнить комманду.");
            }

            return;
        }
    }

    private async Task CallbackQueryUpdate(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        var chatId = update.CallbackQuery.Message.Chat.Id;

        if (Chats[chatId].Disable) 
        {
            return;
        }

        var messageId = update.CallbackQuery.Message.MessageId;
        var data = update.CallbackQuery.Data.Split(":");

        if (data[0] == CallbackId_Target)
        {
            await SelectTarget(botClient, chatId, messageId, data[1]);
            return;
        }
        if (data[0] == CallbackId_Beat)
        {
            var beat = new OptionRow() 
            { 
                Id = Convert.ToInt32(data[1]), 
                Name = data[2] 
            };

            await SelectBeat(botClient, chatId, messageId, beat);
            return;
        }
        if (data[0] == CallbackId_Voice)
        {
            var voice = new OptionRow()
            {
                Id = Convert.ToInt32(data[1]),
                Name = data[2]
            };

            await SelectVoice(botClient, chatId, messageId, voice);
            return;
        }
    }

    #endregion

    #region Main logic set
    private InlineKeyboardMarkup GetMarkup<T>(List<T> items, string callbackId)
    {
        List<List<InlineKeyboardButton>> buttons = new();

        foreach (var item in items)
        {
            InlineKeyboardButton urlButton;

            if (item is OptionRow optionRow)
            {
                urlButton = new(optionRow.Name);
                urlButton.CallbackData = $"{callbackId}:{optionRow.Id}:{optionRow.Name}";
            }
            else
            {
                urlButton = new(item.ToString());
                urlButton.CallbackData = $"{callbackId}:{item}";
            }

            buttons.Add(new List<InlineKeyboardButton> { urlButton });
        }

        return new InlineKeyboardMarkup(buttons);
    }

    private async Task SendMarkup<T>(ITelegramBotClient botClient, long chatId, List<T> items, string callbackId, string markupHeader)
    {
        var markup = GetMarkup(items, callbackId);
        await SendUniportentMessage(botClient, chatId, markupHeader, markup);
    }

    private async Task SelectTarget(ITelegramBotClient botClient, long chatId, int messageId, string targetName)
    {
        // Save selected target name
        Chats[chatId].SelectedTarget = targetName;

        // Remove targetName markup
        await botClient.EditMessageReplyMarkupAsync(chatId, messageId);
        await botClient.EditMessageTextAsync(chatId, messageId, $"Ваша цель: {targetName}");

        // Set beat markup
        await SendMarkup(botClient, chatId, _autogenerationOptions.Beats, CallbackId_Beat, "Выберите ваше седство: ");
    }

    private async Task SelectBeat(ITelegramBotClient botClient, long chatId, int messageId, OptionRow beat)
    {
        // Save selected beat
        var chat = Chats[chatId];
        chat.SelectedBeatId = Convert.ToInt32(beat.Id);

        // Remove beat markup
        await botClient.EditMessageReplyMarkupAsync(chatId, messageId);
        await botClient.EditMessageTextAsync(chatId, messageId, $"Ваше средство: {beat.Name}");

        // Set voice markup
        await SendMarkup(botClient, chatId, _autogenerationOptions.Voices, CallbackId_Voice, "Выберите исполнителя: ");
    }

    private async Task SelectVoice(ITelegramBotClient botClient, long chatId, int messageId, OptionRow voice)
    {
        // Save selected voice
        var chat = Chats[chatId];
        chat.SelectedVoiceId = Convert.ToInt32(voice.Id);

        // Remove voice markup
        await botClient.EditMessageReplyMarkupAsync(chatId, messageId);
        await botClient.EditMessageTextAsync(chatId, messageId, $"Ваш исполнитель: {voice.Name}");

        // Finally create audio
        await SendAudio(botClient, chatId);
    }

    private async Task SendAudio(ITelegramBotClient botClient, long chatId)
    {
        var chat = Chats[chatId];
        await SendUniportentMessage(botClient, chatId, "Ваш дисс уже в разработке, ожидайте...");
        chat.Disable = true;
        string messageText;
        try
        {
            await CreateAndSendAuido(botClient, chatId);
            messageText = $"У вас осталось eще {chat.LeftAttempts} попыток";
        }
        catch (Exception ex)
        {
            chat.LeftAttempts++;
            messageText = $"Извините дисса не будет, Марк насрал в код(";
            if (chat.Role == TgRole.Admin)
            {
                messageText += $", details: {ex.Message}";
            }
        }
        await SendUniportentMessage(botClient, chatId, messageText);
        chat.Disable = false;
    }

    private async Task CreateAndSendAuido(ITelegramBotClient botClient, long chatId)
    {
        var chat = Chats[chatId];

        var request = new AutogenerationRequest
        {
            LinesCount = _autogenerationOptions.LinesCounts[1],
            WordsInLineCount = _autogenerationOptions.WordsInLineCounts[1],
            // SwitchLanguage = _autogenerationOptions.SwitchLanguages.First(),
            // SwitchTimes = _autogenerationOptions.SwitchTimes.First(),
            Target = chat.SelectedTarget,
            VoiceId = chat.SelectedVoiceId,
            BeatId = chat.SelectedBeatId
        };

        var dissAudioData = await _autogenerationLogic.Autogenerate(request, _autogenerationOptions);
        if (dissAudioData is null)
        {
            await SendUniportentMessage(botClient, chatId, "Autogeneration error...");
        }

        // Send audio
        using (Stream stream = new MemoryStream(dissAudioData))
        {
            var ifs = new InputFileStream(stream);
            await botClient.SendAudioAsync(chatId, ifs, performer: "AftaDi$$PraDuckSheng", title: $"Di$$ Ha {chat.SelectedTarget}a"); // important message
        }
    }

    private async Task SendUniportentMessage(ITelegramBotClient botClient, long chatId, string text, InlineKeyboardMarkup? keyboard = null)
    {
        Message message;

        if (keyboard is not null)
        {
            message = await botClient.SendTextMessageAsync(chatId, text, replyMarkup: keyboard);
        }
        else
        {
            message = await botClient.SendTextMessageAsync(chatId, text);
        }

        Chats[chatId].UnimportantMessages.Add(message.MessageId);
    }

    private async Task RemoveUnimportentMessages(ITelegramBotClient botClient, long chatId)
    {
        var chat = Chats[chatId];
        foreach (var messageId in chat.UnimportantMessages)
        {
            try
            {
                await botClient.DeleteMessageAsync(chatId, messageId);
            }
            catch { }
        }

        chat.UnimportantMessages.Clear();
    }

    private async Task SendMessageForAllAdmins(ITelegramBotClient botClient, string message)
    {
        foreach (var chat in Chats)
        {
            if (chat.Value.Role == TgRole.Admin)
            {
                try
                {
                    await botClient.SendTextMessageAsync(chat.Key, message);
                }
                catch { }
            }
        }
    }

    #endregion

    #region Error block

    private async Task BotError(ITelegramBotClient botClient, Exception exception, CancellationToken ct)
    {
        await _userChatsMaster.AddLog($"BotError was called. Utc time: {DateTime.UtcNow}, Exception Message: {exception.Message}");
        await SendMessageForAllAdmins(botClient, $"Bot failed down, Exception Message: {exception.Message}");

        while (true)
        {
            try
            {
                await RestartBot(botClient, ct);

                await _userChatsMaster.AddLog($"Bot was restarted. Utc time: {DateTime.UtcNow}");
                await Task.Delay(Bot_AfterRestartIgnoring_Delay + 5000, ct);
                await SendMessageForAllAdmins(botClient, $"Bot was restarted. Utc time: {DateTime.UtcNow}");
            }
            catch (Exception ex)
            {
                await _userChatsMaster.AddLog($"Bot Restart failed down. Utc time: {DateTime.UtcNow}, Exception Message: {ex.Message}");
                await SendMessageForAllAdmins(botClient, $"Bot Restart failed down. Utc time: {DateTime.UtcNow}, Exception Message: {ex.Message}");

                await Task.Delay(Bot_BetweenRestartAttemts_Delay, ct);
            }
        }

    }

    private async Task RestartBot(ITelegramBotClient botClient, CancellationToken ct)
    {
        await botClient.CloseAsync(ct);
        await Task.Delay(Bot_CloseToRun_Delay, ct);
        Run();
    }

    #endregion
}
