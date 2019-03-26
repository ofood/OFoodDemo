using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using OFood.Domain;
using OFood.Domain.Core.Options;
using OFood.Dependency;
using OFood.Exceptions;
using OFood.Domain.Entity;
using OFood.Domain.Data;

namespace OFood.Domain
{
    /// <summary>
    /// <see cref="IUnitOfWorkManager"/>扩展方法
    /// </summary>
    public static class UnitOfWorkManagerExtensions
    {
        /// <summary>
        /// 获取指定实体所在的上下文对象
        /// </summary>
        public static IDbContext GetDbContext<TEntity, TKey>(this IUnitOfWorkManager unitOfWorkManager) where TEntity : IEntity<TKey>
        {
            Type entityType = typeof(TEntity);
            return unitOfWorkManager.GetDbContext(entityType);
        }

        /// <summary>
        /// 获取指定实体类型所在的上下文对象
        /// </summary>
        public static IDbContext GetDbContext(this IUnitOfWorkManager unitOfWorkManager, Type entityType)
        {
            if (!entityType.IsEntityType())
            {
                throw new OFoodException($"类型“{entityType}”不是实体类型");
            }
            IUnitOfWork unitOfWork = unitOfWorkManager.GetUnitOfWork(entityType);
            return unitOfWork?.GetDbContext(entityType);
        }

        /// <summary>
        /// 获取指定实体类型的数据上下文选项
        /// </summary>
        public static OFoodDbContextOptions GetDbContextResolveOptions<TEntity,TKey>(this IUnitOfWorkManager unitOfWorkManager) where TEntity : IEntity<TKey>
        {
            Type entityType = typeof(TEntity);
            return unitOfWorkManager.GetDbContextResolveOptions(entityType);
        }
        
        /// <summary>
        /// 获取指定实体类型的数据上下文选项
        /// </summary>
        public static OFoodDbContextOptions GetDbContextResolveOptions(this IUnitOfWorkManager unitOfWorkManager, Type entityType)
        {
            Type dbContextType = unitOfWorkManager.GetDbContextType(entityType);
            OFoodDbContextOptions dbContextOptions = unitOfWorkManager.ServiceProvider.GetOFoodOptions()?.GetDbContextOptions(dbContextType);
            if (dbContextOptions == null)
            {
                throw new OFoodException($"无法找到数据上下文“{dbContextType}”的配置信息");
            }
            return dbContextOptions;
        }

        /// <summary>
        /// 获取指定实体类型的Sql执行器
        /// </summary>
        public static ISqlExecutor<TEntity,TKey> GetSqlExecutor<TEntity,TKey>(this IUnitOfWorkManager unitOfWorkManager) where TEntity : IEntity<TKey>
        {
            OFoodDbContextOptions options = unitOfWorkManager.GetDbContextResolveOptions(typeof(TEntity));
            DatabaseType databaseType = options.DatabaseType;
            IList<ISqlExecutor<TEntity, TKey>> executors = unitOfWorkManager.ServiceProvider.GetServices<ISqlExecutor<TEntity, TKey>>().ToList();
            return executors.FirstOrDefault(m => m.DatabaseType == databaseType);
        }
    }
}