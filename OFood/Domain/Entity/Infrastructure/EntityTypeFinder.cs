using System;
using System.Linq;
using System.Reflection;
using OFood.Extensions;
using OFood.Finders;
using OFood.Reflection;


namespace OFood.Domain.Entity
{
    /// <summary>
    /// 实体类型查找器
    /// </summary>
    public class EntityTypeFinder : FinderBase<Type>, IEntityTypeFinder
    {
        private readonly IAllAssemblyFinder _allAssemblyFinder;

        /// <summary>
        /// 初始化一个<see cref="OSharp.Entity.EntityTypeFinder"/>类型的新实例
        /// </summary>
        public EntityTypeFinder(IAllAssemblyFinder allAssemblyFinder)
        {
            _allAssemblyFinder = allAssemblyFinder;
        }

        /// <summary>
        /// 重写以实现所有项的查找
        /// </summary>
        /// <returns></returns>
        protected override Type[] FindAllItems()
        {
            Type baseType = typeof(IEntity<>);
            Assembly[] assemblies = _allAssemblyFinder.FindAll(true);
            return assemblies.SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsDeriveClassFrom(baseType)).Distinct().ToArray();
        }
    }
}