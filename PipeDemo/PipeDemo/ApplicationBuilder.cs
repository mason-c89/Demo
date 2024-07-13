namespace PipeDemo;

public class ApplicationBuilder
{
    private readonly List<Func<RequestDelegate, RequestDelegate>> _components = new List<Func<RequestDelegate, RequestDelegate>>();

    public ApplicationBuilder Use(Func<HttpContext, Func<Task>, Task> middleware)
    {
        return Use(next =>
        {
            return context =>
            {
                Task SimpleNext() => next(context);
                return middleware(context, SimpleNext);
            };
        });
    }
    
    public ApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
    {
        this._components.Add(middleware);
        return this;
    }

    public RequestDelegate Build()
    {
        RequestDelegate requestDelegate = context =>
        {
            Console.WriteLine("保底中间件");
            return Task.CompletedTask;
        };
        for (int index = this._components.Count - 1; index >= 0; --index)
        {
            requestDelegate = this._components[index](requestDelegate);
        }
        
        return requestDelegate;
    }
}