namespace OrderSystem.Infrastructure.Services
{
    public class TelegramBotService
    {
        public async Task SendNotificationAsync(string message)
        {
            Console.WriteLine($"[Telegram Bot] Message sent: {message}");
            await Task.CompletedTask;
        }
    }
}
