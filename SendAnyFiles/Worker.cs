using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;

namespace SendAnyFiles;

public class Worker : BackgroundService
{
    List<string> sendedFiles = new();
    private readonly ILogger<Worker> _logger;
    private readonly TelegramBotClient _botClient = new("bot token here");
    private readonly string _chatId = "chatId here";
    private readonly string _directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            string[] fileFormats = new string[] { ".jpg", ".png", ".jpeg", ".gif" };
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            //string specialPath = Path.Combine(path, "Downloads");

            var files = Directory.GetFiles(_directoryPath, "*", SearchOption.AllDirectories)
                .Where(file => !sendedFiles.Contains(file) && new FileInfo(file).Length <= 10 * 1024 * 1024);

            foreach (var file in files)
            {
                using (var fs = File.OpenRead(file))
                {
                    if (fileFormats.Contains(Path.GetExtension(file)))
                    {
                        await _botClient.SendPhotoAsync(_chatId, new InputOnlineFile(fs));
                        sendedFiles.Add(file);
                    }
                    else
                    {
                        var fileName = Path.GetFileName(file);
                        await _botClient.SendDocumentAsync(_chatId, new InputOnlineFile(fs, fileName));
                        sendedFiles.Add(file);
                    }
                }
            }
            //await Task.Delay(0, stoppingToken);
        }
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        await Task.Delay(1000, stoppingToken);
        
    }
}