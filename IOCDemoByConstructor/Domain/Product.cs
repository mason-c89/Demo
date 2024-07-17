namespace IOCDemoByConstructor.Domain;

[IOCService]
public class Product
{
   private readonly Custom _custom;

   public Product(Custom custom)
   {
      Console.WriteLine("正在注入依赖......");
      _custom = custom;
   }

   public void Introduce()
   {
      _custom.See();
      Console.WriteLine("产品的详情是：......");
   }
}