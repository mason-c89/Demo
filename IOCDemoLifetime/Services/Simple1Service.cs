namespace IOCDemoLifetime.Services;

public class Simple1Service : IService1
{
    public void Run()
    {
        Console.WriteLine("Hello Simple1");
    }
}