using Microsoft.Extensions.DependencyInjection;


namespace OFood.Dependency
{
    /// <summary>
    /// <see cref="IServiceScope"/>工厂包装一下
    /// </summary>
    public interface IHybridServiceScopeFactory : IServiceScopeFactory
    { }
}