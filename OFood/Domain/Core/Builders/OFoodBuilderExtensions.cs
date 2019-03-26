using OFood.Domain.Core.Packs;


namespace OFood.Domain.Core.Builders
{
    /// <summary>
    /// IOSharpBuilder扩展方法
    /// </summary>
    public static class OsharpBuilderExtensions
    {
        /// <summary>
        /// 添加CorePack
        /// </summary>
        public static IOFoodBuilder AddCorePack(this IOFoodBuilder builder)
        {
            return builder.AddPack<OFoodCorePack>();
        }
    }
}