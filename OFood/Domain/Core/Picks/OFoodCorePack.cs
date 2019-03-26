using System.ComponentModel;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using OFood.Caching;
using OFood.Domain.Core.Options;
using OFood.Domain.Entity;
using OFood.Filter;


namespace OFood.Domain.Core.Packs
{
    /// <summary>
    /// OFood核心模块
    /// </summary>
    [Description("OFood核心模块")]
    public class OFoodCorePack : OFoodPack
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Core;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.TryAddSingleton<IConfigureOptions<OFoodOptions>, OFoodOptionsSetup>();
            services.TryAddSingleton<IEntityTypeFinder, EntityTypeFinder>();
            services.TryAddSingleton<IInputDtoTypeFinder, InputDtoTypeFinder>();
            services.TryAddSingleton<IOutputDtoTypeFinder, OutputDtoTypeFinder>();

            services.TryAddSingleton<ICacheService, CacheService>();
            services.TryAddScoped<IFilterService, FilterService>();

            return services;
        }
    }
}