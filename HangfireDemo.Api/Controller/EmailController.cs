using Hangfire;
using HangfireDemo.Core.Filters;
using HangfireDemo.Core.Services.Email;
using Microsoft.AspNetCore.Mvc;

namespace HangfireDemo.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class EmailController
{
    // private readonly IEmailService _emailService;
    //
    // public EmailController(IEmailService emailService)
    // {
    //     _emailService = emailService;
    // }
    
    [Route("send"), HttpGet]
    public void SendMsg(int id, string message)
    {
        RecurringJob.AddOrUpdate<IEmailService>(x => x.Send(id, message), Cron.Minutely);
        // BackgroundJob.Schedule<IEmailService>(x => x.Send(id, message), TimeSpan.FromSeconds(10));
        // _emailService.Send(id, message);
    }
    
    [Route("sendError"), HttpGet]
    public void SendMsgError(int id, string message)
    {
        BackgroundJob.Schedule<IEmailService>(x => x.SendError(id, message), TimeSpan.FromSeconds(10));
        // _emailService.Send(id, message);
    }

    // 定义线程私有变量，每个线程有独立的值
    private static ThreadLocal<int> _threadLocalData = new ThreadLocal<int>(() => Thread.CurrentThread.GetHashCode());

    [Route("test"), HttpGet]
    public async void Test()
    {
        _threadLocalData.Value += 1; // 修改线程私有变量的值
        Console.WriteLine("线程私有变量初始值Before：" + _threadLocalData.Value);

        var str = await getStr();

        Console.WriteLine("线程私有变量值After：" + _threadLocalData.Value);
        Console.WriteLine("123");
        Console.WriteLine(str);
    }

    public static async Task<string> getStr()
    {
        Console.WriteLine("getStr线程私有变量初始值Before：" + _threadLocalData.Value);
        
        await Task.Delay(5000);
        
        Console.WriteLine("getStr线程私有变量值After：" + _threadLocalData.Value);
        return "hi";
    }
}