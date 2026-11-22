namespace SnackCounter.Interfaces
{
    public interface IChatService
    {
        Task SendMessageAsync(string employeeId, string message);

    }
}
