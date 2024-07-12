using Autodissmark.TGBot.Autogeneration;
using Autodissmark.TGBot.TgSettings.Options;
using Autodissmark.TGBot.TgSettings;
using Autodissmark.TGBot.UserChats;


namespace Autodissmark.TGBot;

public class TgBotBuilder
{
    private TgBotSettingsMaster _tgBotSettingsMaster;
    private ApiOptions _apiOptions;
    private AutogenerationOptions _autogenerationOptions;
    private TelegramOptions _telegramOptions;

    private AutogenerationLogic _autogenerationLogic;
    private UserChatsMaster _userChatsMaster;
    private async Task Setup() 
    {
        _tgBotSettingsMaster = new TgBotSettingsMaster();

        _apiOptions = await _tgBotSettingsMaster.ReadApiOptions();
        _autogenerationOptions = await _tgBotSettingsMaster.ReadAutogenerationOptions();
        _telegramOptions = await _tgBotSettingsMaster.ReadTelegramOptions();

        _autogenerationLogic = new AutogenerationLogic(_apiOptions);
        _userChatsMaster = new UserChatsMaster();
    }

    private async Task ChangeConfigTest() 
    {
        // before
        var beforeTarget = _autogenerationOptions.Targets[0];
        Console.WriteLine($"before Target: {beforeTarget}");

        // after
        _autogenerationOptions.Targets[0] = "Крам";
        await _tgBotSettingsMaster.WriteAutogenerationOptions(_autogenerationOptions);
        Console.WriteLine($"after Target: {_autogenerationOptions.Targets[0]}");

        // finally
        _autogenerationOptions.Targets[0] = beforeTarget;
        await _tgBotSettingsMaster.WriteAutogenerationOptions(_autogenerationOptions);
        Console.WriteLine($"finally Target: {_autogenerationOptions.Targets[0]}");
    }

    private async Task<bool> AutogenerateTest()
    {
        var request = new AutogenerationRequest
        {
            LinesCount = _autogenerationOptions.LinesCounts.First(),
            WordsInLineCount = _autogenerationOptions.WordsInLineCounts.First(),
            SwitchLanguage = _autogenerationOptions.SwitchLanguages.First(),
            SwitchTimes = _autogenerationOptions.SwitchTimes.First(),
            Target = _autogenerationOptions.Targets.First(),
            VoiceId = _autogenerationOptions.Voices.First(),
            BeatId = _autogenerationOptions.Beats.First()
        };

        return await _autogenerationLogic.AutogenerateTest(request, _autogenerationOptions);
    }

    private async Task ChangeUserChatsTest()
    {
        var chats = await _userChatsMaster.ReadChatsStates();
        var chatId = chats.First().Key;

        // before
        var beforeTarget = chats[chatId].SelectedTarget;
        Console.WriteLine($"before SelectedTarget: {beforeTarget}");

        // after
        chats[chatId].SelectedTarget = "Жека";
        await _userChatsMaster.WriteChatsStates(chats);
        Console.WriteLine($"after SelectedTarget: {chats[chatId].SelectedTarget}");

        // finally
        chats[chatId].SelectedTarget = beforeTarget;
        await _userChatsMaster.WriteChatsStates(chats);
        Console.WriteLine($"finally SelectedTarget: {chats[chatId].SelectedTarget}");
    }

    public async Task<bool> RunAsync(CancellationToken ct)
    {
        await Setup();

        var tgBot = new TgBot();
        await tgBot.Setup();
        tgBot.Run();

        //await ChangeConfigTest();
        //await ChangeUserChatsTest();
        //await AutogenerateTest();

        return true;
    }
}
