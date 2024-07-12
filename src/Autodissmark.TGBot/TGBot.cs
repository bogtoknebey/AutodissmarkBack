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
            await SendTargetNamesList(botClient, chatId);
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

        if (data[0] == "target")
        {
            await SelectTargetName(botClient, chatId, messageId, data[1]);
            return;
        }
        if (data[0] == "beat")
        {
            await SelectBeat(botClient, chatId, messageId, data[1]);
            return;
        }
    }

    #endregion

    #region Main logic set

    private async Task SendTargetNamesList(ITelegramBotClient botClient, long chatId)
    {
        var callbackId = "target";
        List<List<InlineKeyboardButton>> buttons = new();

        foreach (var targetName in _autogenerationOptions.Targets)
        {
            var urlButton = new InlineKeyboardButton(targetName);
            urlButton.CallbackData = $"{callbackId}:{targetName}";
            buttons.Add(new List<InlineKeyboardButton> { urlButton });
        }
        InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(buttons);

        await SendUniportentMessage(botClient, chatId, "Выберите вашу цель: ", keyboard);
    }

    private async Task SelectTargetName(ITelegramBotClient botClient, long chatId, int messageId, string targetName)
    {
        // save selected target name
        Chats[chatId].SelectedTarget = targetName;

        // remove targetName keyboard
        await botClient.EditMessageReplyMarkupAsync(chatId, messageId);
        await botClient.EditMessageTextAsync(chatId, messageId, $"Ваша цель: {targetName}");

        // set beat keyboard
        await SendBeatsList(botClient, chatId);
    }

    private async Task SendBeatsList(ITelegramBotClient botClient, long chatId)
    {
        string callbackId = "beat";
        List<List<InlineKeyboardButton>> buttons = new();

        foreach (var beat in _autogenerationOptions.Beats)
        {
            var urlButton = new InlineKeyboardButton(beat.ToString());
            urlButton.CallbackData = $"{callbackId}:{beat}";
            buttons.Add(new List<InlineKeyboardButton> { urlButton });
        }
        InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(buttons);

        await SendUniportentMessage(botClient, chatId, "Выберите ваше седство: ", keyboard);
    }

    private async Task SelectBeat(ITelegramBotClient botClient, long chatId, int messageId, string beat)
    {
        var chat = Chats[chatId];
        chat.SelectedBeatNum = Convert.ToInt32(beat); // Save selected target name

        // Remove beat keyboard
        await botClient.EditMessageReplyMarkupAsync(chatId, messageId);
        await botClient.EditMessageTextAsync(chatId, messageId, $"Ваше средство: {beat}");

        // Create audio
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
            // VoiceId = _autogenerationOptions.Voices.First(), // TODO: get VoiceId from user
            BeatId = chat.SelectedBeatNum
        };

        var dissAudioData = await _autogenerationLogic.Autogenerate(request, _autogenerationOptions);
        if (dissAudioData is null)
        {
            await SendUniportentMessage(botClient, chatId, "Autogeneration error...");
        }

        // Send Audio
        try
        {
            using (Stream stream = new MemoryStream(dissAudioData))
            {
                var ifs = new InputFileStream(stream);
                await botClient.SendAudioAsync(chatId, ifs, performer: "AftaDi$$PraDuckSheng", title: $"Di$$ Ha {chat.SelectedTarget}a"); // important message
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Send as MemoryStream Audio error:  {ex.Message}");
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

    #endregion

    #region Error block

    private async Task BotError(ITelegramBotClient botClient, Exception exception, CancellationToken ct)
    {
        await _userChatsMaster.AddLog(exception.Message);

        foreach (var chat in Chats)
        {
            if (chat.Value.Role == TgRole.Admin)
            {
                botClient.SendTextMessageAsync(chat.Key, $"TGBot have failed down, details: {exception.Message}");
            }
        }

        throw exception;
    }

    #endregion
}
