//using Autodissmark.TGBot.Models;
//using Telegram.Bot;
//using Telegram.Bot.Types.Enums;
//using Telegram.Bot.Types.ReplyMarkups;
//using Telegram.Bot.Types;
//using PoetryViewerBack.Autocreation;
//using PoetryViewerBack.Models;
//using System;
//using System.IO;
//using Newtonsoft.Json;
//using PoetryViewerBack.DTO;
//using System.Text;

//namespace Autodissmark.TGBot;

//public class TGBot
//{
//    private TgSettings Settings;
//    private Dictionary<long, ChatStatus>? Chats;

//    // Set up block

//    public TGBot() 
//    {
//        Settings = TgSettings.ReadSettings();

//        Chats = ReadChatsState();
//        if (Chats is null)
//            Chats = new Dictionary<long, ChatStatus>();
//    }

//    public void Run()
//    {
//        if (Chats is null)
//            return;
//        var botClient = new TelegramBotClient("6906816161:AAFEaldl6S7q6yt-Ha3_TCB2gg3lMCBFJeU");
//        botClient.StartReceiving(BotUpdate, BotError);
//    }

//    // Read/Write methods

//    private Dictionary<long, ChatStatus>? ReadChatsState()
//    {
//        // TODO: make TgRole properly readible and writable 0/1 vs 1/2
//        Dictionary<long, ChatStatus> res = new Dictionary<long, ChatStatus>();
//        string path = DTO.PathMaster.ChatsStatesPath();
//        string[] filePaths = Directory.GetFiles(path, "*.txt");
//        foreach (var filePath in filePaths)
//        {
//            long chatId = Convert.ToInt64(filePath.Split('/', '\\').Last().Split(".")[0]);
//            try
//            {
//                string json = System.IO.File.ReadAllText(filePath);

//                ChatStatus? chat = JsonConvert.DeserializeObject<ChatStatus>(json);
//                if (chat is not null)
//                    res.Add(chatId, chat);
//            }
//            catch { }
//        }

//        return res;
//    }

//    private bool WriteChatsState()
//    {
//        // TODO: make TgRole properly readible and writable 0/1 vs 1/2
//        foreach (var chat in Chats)
//        {
//            string filePath = DTO.PathMaster.ChatsStatesFilePath(chat.Key);
//            try
//            {
//                string json = JsonConvert.SerializeObject(chat.Value, Formatting.Indented);
//                System.IO.File.WriteAllText(filePath, json);
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }
//        return true;
//    }

//    // Update routing

//    private void AddNewChat(long chatId)
//    {
//        if (!Chats.ContainsKey(chatId))
//        {
//            Chats[chatId] = new ChatStatus() 
//            { 
//                Role = TgRole.User, 
//                Disable = false, 
//                LeftAttempts = Settings.UserAttempts, 
//                LastModify = DateTime.Today.AddHours(12)
//            };
//        }
//    }

//    async private Task BotUpdate(ITelegramBotClient botClient, Update update, CancellationToken token)
//    {
//        if (update.Type == UpdateType.Message)
//        {
//            MessageUpdate(botClient, update, token);
//            return;
//        }
//        if (update.Type == UpdateType.CallbackQuery)
//        {
//            CallbackQueryUpdate(botClient, update, token);
//            return;
//        }
//    }

//    async private Task MessageUpdate(ITelegramBotClient botClient, Update update, CancellationToken token)
//    {
//        var message = update.Message;
//        long chatId = message.Chat.Id;

//        AddNewChat(chatId); // add if there no such

//        var chat = Chats[chatId];
//        chat.UnimportantMessages.Add(message.MessageId);

//        if (Chats.ContainsKey(chatId) && Chats[chatId].Disable)
//            return;

//        // Common commands
//        if (message.Text == "/start")
//        {
//            Message Message;
            
//            // restore LeftAttempts every day
//            if ((DateTime.Now - chat.LastModify) > TimeSpan.FromDays(1))
//            {
//                if (chat.Role == TgRole.User)
//                    chat.LeftAttempts = Settings.UserAttempts;
//                if (chat.Role == TgRole.Admin)
//                    chat.LeftAttempts = Settings.AdminAttempts;
//                chat.LastModify = DateTime.Today.AddHours(12);

//                await SendUniportentMessage(botClient, chatId, 
//                    $"Хорошая новость, ваши попытки васстановлены у вас снова {chat.LeftAttempts} попыток!"
//                    );
//            }

//            // check LeftAttempts
//            if (chat.LeftAttempts <= 0) 
//            {
//                await SendUniportentMessage(botClient, chatId, "У вас больше не осталось попыток!");
//                return;
//            }
//            else
//            {
//                chat.LeftAttempts--;
//            }

//            // do /start
//            await SendUniportentMessage(botClient, chatId, "Вас приветсвтует Автодиссер!");
//            await SendTargetNamesList(botClient, chatId);
//            return;
//        }
//        if (message.Text == "/reset")
//        {
//            await RemoveUnimportentMessages(botClient, chatId);
//            return;
//        }
//        if (message.Text == "/help")
//        {
//            StringBuilder description = new StringBuilder();
//            description.Append("Основные команды:\n\n");
//            description.Append("/help - список команд и их описания\n");
//            description.Append(
//                $"/start - запустить процесс создания дисса, " +
//                $"существует лимит на данную команду: {Settings.UserAttempts} раз в сутки\n"
//                );
//            description.Append("/reset - удалить все ненужные сообщения (все кроме диссов)\n");

//            if (chat.Role == TgRole.Admin)
//            {
//                description.Append("\nСпециальные команды:\n\n");
//                description.Append("/save - сохранить состояния всех чатов (небходимо вызвать перед перезапуском)\n");
//                description.Append("/add target {имя} - Добавть цель \n");
//                description.Append("/remove target {имя} - Удалить цель\n");
//            }

//            await SendUniportentMessage(botClient, chatId, description.ToString());
//            return;
//        }

//        // Specific commands
//        if (chat.Role != TgRole.Admin)
//            return;
//        if (message.Text == "/save")
//        {
//            string response;
//            bool savingRes = WriteChatsState();

//            if (chat.Role == TgRole.Admin) 
//            {
//                if (savingRes)
//                    response = "Сохранение прошло успешно!";
//                else
//                    response = "Ошибка сохранения!";
//            }
//            else
//            {
//                response = "У вас нет прав на использование этой комманды!";
//            }
//            await SendUniportentMessage(botClient, chatId, response);
//            return;
//        }
//        if (message.Text.Contains("/add target "))
//        {
//            try
//            {
//                string name = message.Text.Split(' ')[2];
//                Settings.TargetNames.Add(name);
//                SendUniportentMessage(botClient, chatId, $"Имя {name} добавлено.");
//                Settings.WriteSettings();
//            }
//            catch
//            {
//                SendUniportentMessage(botClient, chatId, "Не правильный формат комманды. Используйте /help чтобы уточнить комманду.");
//            }

//            return;
//        }
//        if (message.Text.Contains("/remove target "))
//        {
//            try
//            {
//                string name = message.Text.Split(' ')[2];
//                Settings.TargetNames.Remove(name);
//                SendUniportentMessage(botClient, chatId, $"Имя {name} удалено.");
//                Settings.WriteSettings();
//            }
//            catch
//            {
//                SendUniportentMessage(botClient, chatId, "Не правильный формат комманды или неправильный аргумент. Используйте /help чтобы уточнить комманду.");
//            }

//            return;
//        }
//    }

//    async private Task CallbackQueryUpdate(ITelegramBotClient botClient, Update update, CancellationToken token)
//    {
//        long chatId = update.CallbackQuery.Message.Chat.Id;
//        if (Chats[chatId].Disable)
//            return;

//        int messageId = update.CallbackQuery.Message.MessageId;
//        string[] data = update.CallbackQuery.Data.Split(":");

//        if (data[0] == "target")
//        {
//            await SelectTargetName(botClient, chatId, messageId, data[1]);
//            return;
//        }
//        if (data[0] == "beat")
//        {
//            await SelectBeat(botClient, chatId, messageId, data[1]);
//            return;
//        }
//        return;
//    }
    
//    // Main logic set 

//    async private Task SendTargetNamesList(ITelegramBotClient botClient, long chatId)
//    {
//        string callBackId = "target";
//        List<List<InlineKeyboardButton>> buttons = new();
//        foreach (var name in Settings.TargetNames)
//        {
//            InlineKeyboardButton urlButton = new InlineKeyboardButton(name);
//            urlButton.CallbackData = $"{callBackId}:{name}";
//            buttons.Add(new List<InlineKeyboardButton> { urlButton });
//        }
//        InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(buttons);

//        await SendUniportentMessage(botClient, chatId, "Выберите вашу цель: ", keyboard);
//    }

//    async private Task SelectTargetName(ITelegramBotClient botClient, long chatId, int messageId, string targetName)
//    {
//        // save selected target name
//        Chats[chatId].SelectedTarget = targetName;

//        // remove targetName keyboard
//        await botClient.EditMessageReplyMarkupAsync(chatId, messageId);
//        await botClient.EditMessageTextAsync(chatId, messageId, $"Ваша цель: {targetName}");

//        // set beat keyboard
//        await SendBeatsList(botClient, chatId);
//    }

//    async private Task SendBeatsList(ITelegramBotClient botClient, long chatId)
//    {
//        string callBackId = "beat";
//        List<List<InlineKeyboardButton>> buttons = new();
//        foreach (var beat in Settings.Beats)
//        {
//            InlineKeyboardButton urlButton = new InlineKeyboardButton(beat);
//            urlButton.CallbackData = $"{callBackId}:{beat}";
//            buttons.Add(new List<InlineKeyboardButton> { urlButton });
//        }
//        InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(buttons);

//        await SendUniportentMessage(botClient, chatId, "Выберите ваше седство: ", keyboard);
//    }

//    async private Task SelectBeat(ITelegramBotClient botClient, long chatId, int messageId, string beat)
//    {
//        var chat = Chats[chatId];
//        chat.SelectedBeatNum = Convert.ToInt32(beat); // Save selected target name

//        // Remove beat keyboard
//        await botClient.EditMessageReplyMarkupAsync(chatId, messageId);
//        await botClient.EditMessageTextAsync(chatId, messageId, $"Ваше средство: {beat}");

//        // Create audio
//        await SendUniportentMessage(botClient, chatId, "Ваш дисс уже в разработке, ожидайте...");
//        chat.Disable = true;
//        string messageText;
//        try
//        {
//            await CreateAndSendAuido(botClient, chatId);
//            messageText = $"У вас осталось eще {chat.LeftAttempts} попыток";
//        }
//        catch(Exception ex)
//        {
//            chat.LeftAttempts++;
//            messageText = $"Извините дисса не будет, Марк насрал в код(";
//            if(chat.Role == TgRole.Admin)
//                messageText += $", details: {ex.Message}";
//        }
//        await SendUniportentMessage(botClient, chatId, messageText);
//        chat.Disable = false;
//    }

//    async private Task CreateAndSendAuido(ITelegramBotClient botClient, long chatId)
//    {
//        var chat = Chats[chatId];

//        // Create and save Audio
//        MultiCreateResponse res = await Autocreator.AudioAutoCreateAndSave(
//            "DissBot", chat.SelectedTarget, chat.SelectedBeatNum, 10, 5, 10
//            );
//        if (res.Error)
//            throw new Exception($"MultiCreateResponse have an error: {res.Message}");

//        // Get Audio 
//        string audioNumStr = res.CreateAudioResponse.Name;
//        int audioNum = Convert.ToInt32(audioNumStr.Split('.')[0]);
//        byte[]? dissAudioData = await DTO.AudioRecord.GetAudioByNumber(
//            "DissBot", res.CreatePoetryResponse.Name, audioNum
//            );
//        if (dissAudioData is null)
//            throw new Exception($"byte[]? dissAudioData is null");

//        // Send Audio
//        try
//        {
//            using (Stream stream = new MemoryStream(dissAudioData))
//            {
//                var ifs = new InputFileStream(stream);
//                await botClient.SendAudioAsync(chatId, ifs, performer: "AftaDi$$PraDuckSheng", title: $"Di$$ Ha {chat.SelectedTarget}a"); // important message
//            }
//        }
//        catch(Exception ex)
//        {
//            throw new Exception($"Send as MemoryStream Audio error:  {ex.Message}");
//        }

//    }

//    async private Task SendUniportentMessage(ITelegramBotClient botClient, long chatId, string text, 
//        InlineKeyboardMarkup? keyboard = null)
//    {
//        Message message;
//        if (keyboard is not null)
//            message = await botClient.SendTextMessageAsync(chatId, text, replyMarkup: keyboard);
//        else
//            message = await botClient.SendTextMessageAsync(chatId, text);

//        Chats[chatId].UnimportantMessages.Add(message.MessageId);
//    }

//    async private Task RemoveUnimportentMessages(ITelegramBotClient botClient, long chatId)
//    {
//        var chat = Chats[chatId];
//        foreach (var messageId in chat.UnimportantMessages)
//        {
//            try
//            {
//                await botClient.DeleteMessageAsync(chatId, messageId);
//            }
//            catch { }
//        }
            
//        chat.UnimportantMessages.Clear();
//    }

//    // Error block

//    private void AddLog(string logText)
//    {
//        string logfilePath = PathMaster.TelegramBotLogFilePath();
//        string text = System.IO.File.ReadAllText(logfilePath);
//        text += $"{logText}\n\n";
//        System.IO.File.WriteAllText(logfilePath, text);
//    }

//    private Task BotError(ITelegramBotClient botClient, Exception exception, CancellationToken token)
//    {
//        try
//        {
//            AddLog(exception.Message);
//        }
//        catch { }
        

//        foreach (var chat in Chats)
//        {
//            if (chat.Value.Role == TgRole.Admin)
//            {
//                botClient.SendTextMessageAsync(chat.Key, $"TGBot have failed down, details: {exception.Message}");
//            }
//        }

//        throw exception;
//    }
//}

