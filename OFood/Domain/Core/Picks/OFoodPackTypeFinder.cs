using OFood.Reflection;


namespace OFood.Domain.Core.Packs
{
    /// <summary>
    /// OSharp模块类型查找器
    /// </summary>
    public class OsharpPackTypeFinder : BaseTypeFinderBase<OFoodPack>, IOFoodPackTypeFinder
    {
        /// <summary>
        /// 初始化一个<see cref="OsharpPackTypeFinder"/>类型的新实例
        /// </summary>
        public OsharpPackTypeFinder(IAllAssemblyFinder allAssemblyFinder)
            : base(allAssemblyFinder)
        { }
    }
}