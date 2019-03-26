using System.Reflection;

using OFood.Dependency;
using OFood.Finders;



namespace OFood.Reflection
{
    /// <summary>
    /// 定义程序集查找器
    /// </summary>
    [IgnoreDependency]
    public interface IAssemblyFinder : IFinder<Assembly>
    { }
}