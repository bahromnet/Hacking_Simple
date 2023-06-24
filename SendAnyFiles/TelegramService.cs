using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace SendAnyFiles;

public class TelegramService : IUpdateHandler
{
    public TelegramService()
    {
        
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
