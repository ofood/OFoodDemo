using OFood.Domain.Core.Packs;
using OFood.EventBuses;


namespace OFood.Domain.Audits
{
    /// <summary>
    /// 审计模块基类
    /// </summary>
    [DependsOnPacks(typeof(EventBusPack))]
    public abstract class AuditPackBase : OFoodPack
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Application;
    }
}