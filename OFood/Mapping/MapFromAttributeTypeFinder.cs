using Microsoft.Extensions.DependencyInjection;

using OFood.Dependency;
using OFood.Reflection;


namespace OFood.Mapping
{
    /// <summary>
    /// 标注了<see cref="MapFromAttribute"/>标签的类型查找器
    /// </summary>
    [Dependency(ServiceLifetime.Singleton, TryAdd = true)]
    public class MapFromAttributeTypeFinder : AttributeTypeFinderBase<MapFromAttribute>, IMapFromAttributeTypeFinder
    {
        /// <summary>
        /// 初始化一个<see cref="MapFromAttributeTypeFinder"/>类型的新实例
        /// </summary>
        public MapFromAttributeTypeFinder(IAllAssemblyFinder allAssemblyFinder)
            : base(allAssemblyFinder)
        { }
    }
}