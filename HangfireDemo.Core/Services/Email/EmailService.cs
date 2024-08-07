using HangfireDemo.Core.Filters;

namespace HangfireDemo.Core.Services.Email;

[LogEverything]
public interface IEmailService : IScopeService
{
    [LogEverything]
    public void Send(int id, string message);
    public void SendError(int id, string message);
}

public class EmailService : IEmailService
{
    public void Send(int id, string message)
    {
        Console.WriteLine(DateTime.Now);
        Console.WriteLine($"{id}: {message}.");
        Console.WriteLine("====================");
    }

    public void SendError(int id, string message)
    {
        throw new Exception("模拟异常");
    }
}