using IOCDemoLifetime.IOCS;
using IOCDemoLifetime.Services;

namespace IOCDemoLifetime;

internal class Program
{
    private static void Main(string[] args)
    {
        var container = new IOCFactory();

        // 注册服务
        container.RegisterSingleton<IService1, Simple1Service>();
        container.RegisterScoped<IService2, Simple2Service>();
        container.RegisterTransient<IService3, Simple3Service>();

        // 测试单例服务
        Console.WriteLine("Testing Singleton Service:");
        var singletonService1 = container.Resolve<IService1>();
        var singletonService2 = container.Resolve<IService1>();
        Console.WriteLine($"Instance 1 HashCode: {singletonService1.GetHashCode()}");
        Console.WriteLine($"Instance 2 HashCode: {singletonService2.GetHashCode()}");
        singletonService1.Run();
        Console.WriteLine();

        // 测试作用域服务
        Console.WriteLine("Testing Scoped Service:");
        container.BeginScope();
        var scopedService1 = container.Resolve<IService2>();
        var scopedService2 = container.Resolve<IService2>();
        Console.WriteLine($"Instance 1 HashCode: {scopedService1.GetHashCode()}");
        Console.WriteLine($"Instance 2 HashCode: {scopedService2.GetHashCode()}");
        scopedService1.Run();

        container.BeginScope();
        var scopedService3 = container.Resolve<IService2>();
        var scopedService4 = container.Resolve<IService2>();
        Console.WriteLine($"Instance 3 HashCode: {scopedService3.GetHashCode()}");
        Console.WriteLine($"Instance 4 HashCode: {scopedService4.GetHashCode()}");
        Console.WriteLine();

        // 测试瞬时服务
        Console.WriteLine("Testing Transient Service:");
        var transientService1 = container.Resolve<IService3>();
        var transientService2 = container.Resolve<IService3>();
        Console.WriteLine($"Instance 1 HashCode: {transientService1.GetHashCode()}");
        Console.WriteLine($"Instance 2 HashCode: {transientService2.GetHashCode()}");
        transientService1.Run();
        transientService2.Run();
    }
}