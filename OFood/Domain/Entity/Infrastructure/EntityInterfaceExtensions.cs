using System;
using OFood.Extensions;
using OFood.Reflection;


namespace OFood.Domain.Entity
{
    /// <summary>
    /// 实体接口扩展方法
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// 检测指定类型是否为<see cref="IEntity{TKey}"/>实体类型
        /// </summary>
        /// <param name="type">要判断的类型</param>
        /// <returns></returns>
        public static bool IsEntityType(this Type type)
        {
            type.CheckNotNull(nameof(type));
            return typeof(IEntity<>).IsGenericAssignableFrom(type) && !type.IsAbstract && !type.IsInterface;
        }

        /// <summary>
        /// 判断指定实体是否已过期
        /// </summary>
        /// <param name="entity">要检测的实体</param>
        /// <returns></returns>
        public static bool IsExpired(this IExpirable entity)
        {
            entity.CheckNotNull(nameof(entity));
            DateTime now = DateTime.Now;
            return entity.BeginTime != null && entity.BeginTime.Value > now || entity.EndTime != null && entity.EndTime.Value < now;
        }
    }
}