using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace OFood.Extensions
{
    /// <summary>
    /// 集合扩展方法类
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 如果不存在，添加项
        /// </summary>
        public static void AddIfNotExist<T>(this ICollection<T> collection, T value, Func<T, bool> existFunc = null)
        {
            collection.CheckNotNull(nameof(collection));
            bool exists = existFunc == null ? collection.Contains(value) : existFunc(value);
            if (!exists)
            {
                collection.Add(value);
            }
        }

        /// <summary>
        /// 如果不为空，添加项
        /// </summary>
        public static void AddIfNotNull<T>(this ICollection<T> collection, T value) where T : class
        {
            collection.CheckNotNull(nameof(collection));
            if (value != null)
            {
                collection.Add(value);
            }
        }

        /// <summary>
        /// 获取对象，不存在对使用委托添加对象
        /// </summary>
        public static T GetOrAdd<T>(this ICollection<T> collection, Func<T, bool> selector, Func<T> factory)
        {
            collection.CheckNotNull(nameof(collection));
            T item = collection.FirstOrDefault(selector);
            if (item == null)
            {
                item = factory();
                collection.Add(item);
            }

            return item;
        }
    }
}