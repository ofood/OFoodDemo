using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using OFood.Collections;
using OFood.Domain.Core.Builders;
using OFood.Domain.Core.Options;
using OFood.Domain.Core.Packs;

using OFood.Dependency;
using OFood.Domain.Entity;
using OFood.Reflection;
using OFood.Extensions;
using OFood.Domain;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 依赖注入服务集合扩展
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 将OFood服务，各个<see cref="OFoodPack"/>模块的服务添加到服务容器中
        /// </summary>
        public static IServiceCollection AddOFood<TOFoodPackManager>(this IServiceCollection services, Action<IOFoodBuilder> builderAction = null)
            where TOFoodPackManager : IOFoodPackManager, new()
        {
            services.CheckNotNull(nameof(services));

            //初始化所有程序集查找器
            services.TryAddSingleton<IAllAssemblyFinder>(new AppDomainAllAssemblyFinder());

            IOFoodBuilder builder = services.GetSingletonInstanceOrNull<IOFoodBuilder>() ?? new OFoodBuilder();
            builderAction?.Invoke(builder);
            services.TryAddSingleton<IOFoodBuilder>(builder);

            TOFoodPackManager manager = new TOFoodPackManager();
            services.AddSingleton<IOFoodPackManager>(manager);
            manager.LoadPacks(services);
            return services;
        }

        /// <summary>
        /// 获取<see cref="IConfiguration"/>配置信息
        /// </summary>
        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            return services.GetSingletonInstance<IConfiguration>();
        }

        /// <summary>
        /// 从服务提供者中获取OSharpOptions
        /// </summary>
        public static OFoodOptions GetOFoodOptions(this IServiceProvider provider)
        {
            return provider.GetService<IOptions<OFoodOptions>>()?.Value;
        }

        /// <summary>
        /// 获取指定类型的日志对象
        /// </summary>
        /// <typeparam name="T">非静态强类型</typeparam>
        /// <returns>日志对象</returns>
        public static ILogger<T> GetLogger<T>(this IServiceProvider provider)
        {
            ILoggerFactory factory = provider.GetService<ILoggerFactory>();
            return factory.CreateLogger<T>();
        }

        /// <summary>
        /// 获取指定类型的日志对象
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="type">指定类型</param>
        /// <returns>日志对象</returns>
        public static ILogger GetLogger(this IServiceProvider provider, Type type)
        {
            ILoggerFactory factory = provider.GetService<ILoggerFactory>();
            return factory.CreateLogger(type);
        }

        /// <summary>
        /// 获取指定名称的日志对象
        /// </summary>
        public static ILogger GetLogger(this IServiceProvider provider, string name)
        {
            ILoggerFactory factory = provider.GetService<ILoggerFactory>();
            return factory.CreateLogger(name);
        }

        /// <summary>
        /// 获取指定实体类的上下文所在工作单元
        /// </summary>
        public static IUnitOfWork GetUnitOfWork<TEntity, TKey>(this IServiceProvider provider) where TEntity : IEntity<TKey>
        {
            IUnitOfWorkManager unitOfWorkManager = provider.GetService<IUnitOfWorkManager>();
            return unitOfWorkManager.GetUnitOfWork<TEntity, TKey>();
        }

        /// <summary>
        /// 获取指定实体类型的上下文对象
        /// </summary>
        public static IDbContext GetDbContext<TEntity, TKey>(this IServiceProvider provider) where TEntity : IEntity<TKey>
        {
            IUnitOfWorkManager unitOfWorkManager = provider.GetService<IUnitOfWorkManager>();
            return unitOfWorkManager.GetDbContext<TEntity, TKey>();
        }

        /// <summary>
        /// OFood框架初始化，适用于非AspNetCore环境
        /// </summary>
        public static IServiceProvider UseOFood(this IServiceProvider provider)
        {
            IOFoodPackManager packManager = provider.GetService<IOFoodPackManager>();
            packManager.UsePack(provider);
            return provider;
        }
    }
}