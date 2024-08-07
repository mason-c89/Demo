namespace HangfireDemo.Core.Services;

public interface IService { }
public interface ITransientService : IService { }
public interface IScopeService : IService { }
public interface ISingletonService : IService { }