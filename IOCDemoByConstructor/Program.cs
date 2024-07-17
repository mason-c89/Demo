using IOCDemoByConstructor.Domain;
using IOCDemoByConstructor.IOCS;

namespace IOCDemoByConstructor;

class Program
{
    static void Main(string[] args)
    {
        var iocFactory = new IOCFactory();
        Product p = iocFactory.GetObject<Product>();
        Console.WriteLine(p.GetHashCode());
        p.Introduce();
        Product product = iocFactory.GetObject<Product>();
        product.Introduce();
        Console.WriteLine(product.GetHashCode());
    }
}