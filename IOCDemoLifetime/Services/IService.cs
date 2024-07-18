namespace IOCDemoLifetime.Services;

public interface IService
{
    public void Run();
}

public interface IService1 : IService
{
}

public interface IService2 : IService
{
}

public interface IService3 : IService
{
}