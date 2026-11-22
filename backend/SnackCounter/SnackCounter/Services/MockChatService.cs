using SnackCounter.Interfaces;

namespace SnackCounter.Services
{
    public class MockChatService: IChatService
    {
        public Task SendMessageAsync(
       string employeeId,
       string message)
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine("MOCK CHAT MESSAGE");
            Console.WriteLine($"Employee: {employeeId}");
            Console.WriteLine($"Message: {message}");
            Console.WriteLine("--------------------------------");

            return Task.CompletedTask;
        }

    }
}
