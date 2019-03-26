using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OFood.Domain.Core.Builders;
using OFood.Dependency;
using OFood.Exceptions;
using OFood.Extensions;

namespace OFood.Domain.Core.Packs
{
    /// <summary>
    /// OSharp模块管理器
    /// </summary>
    public class OsharpPackManager : IOFoodPackManager
    {
        private readonly List<OFoodPack> _sourcePacks;

        /// <summary>
        /// 初始化一个<see cref="OsharpPackManager"/>类型的新实例
        /// </summary>
        public OsharpPackManager()
        {
            _sourcePacks = new List<OFoodPack>();
            LoadedPacks = new List<OFoodPack>();
        }

        /// <summary>
        /// 获取 自动检索到的所有模块信息
        /// </summary>
        public IEnumerable<OFoodPack> SourcePacks => _sourcePacks;

        /// <summary>
        /// 获取 最终加载的模块信息集合
        /// </summary>
        public IEnumerable<OFoodPack> LoadedPacks { get; private set; }

        /// <summary>
        /// 加载模块服务
        /// </summary>
        /// <param name="services">服务容器</param>
        /// <returns></returns>
        public virtual IServiceCollection LoadPacks(IServiceCollection services)
        {
            IOFoodPackTypeFinder packTypeFinder =
                services.GetOrAddTypeFinder<IOFoodPackTypeFinder>(assemblyFinder => new OsharpPackTypeFinder(assemblyFinder));
            Type[] packTypes = packTypeFinder.FindAll();
            _sourcePacks.Clear();
            _sourcePacks.AddRange(packTypes.Select(m => (OFoodPack)Activator.CreateInstance(m)));

            IOFoodBuilder builder = services.GetSingletonInstance<IOFoodBuilder>();
            List<OFoodPack> packs;
            if (builder.AddPacks.Any())
            {
                packs = _sourcePacks.Where(m => m.Level == PackLevel.Core)
                    .Union(_sourcePacks.Where(m => builder.AddPacks.Contains(m.GetType()))).Distinct()
                    .OrderBy(m => m.Level).ThenBy(m => m.Order).ToList();
                List<OFoodPack> dependPacks = new List<OFoodPack>();
                foreach (OFoodPack pack in packs)
                {
                    Type[] dependPackTypes = pack.GetDependPackTypes();
                    foreach (Type dependPackType in dependPackTypes)
                    {
                        OFoodPack dependPack = _sourcePacks.Find(m => m.GetType() == dependPackType);
                        if (dependPack == null)
                        {
                            throw new OFoodException($"加载模块{pack.GetType().FullName}时无法找到依赖模块{dependPackType.FullName}");
                        }
                        dependPacks.AddIfNotExist(dependPack);
                    }
                }
                packs = packs.Union(dependPacks).Distinct().ToList();
            }
            else
            {
                packs = _sourcePacks.ToList();
                packs.RemoveAll(m => builder.ExceptPacks.Contains(m.GetType()));
            }
            packs = packs.OrderBy(m => m.Level).ThenBy(m => m.Order).ToList();
            LoadedPacks = packs;

            foreach (OFoodPack pack in LoadedPacks)
            {
                services = pack.AddServices(services);
            }

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public virtual void UsePack(IServiceProvider provider)
        {
            ILogger logger = provider.GetLogger<OsharpPackManager>();
            logger.LogInformation("OSharp框架初始化开始");
            DateTime dtStart = DateTime.Now;

            foreach (OFoodPack pack in LoadedPacks)
            {
                pack.UsePack(provider);
                logger.LogInformation($"模块{pack.GetType()}加载成功");
            }

            TimeSpan ts = DateTime.Now.Subtract(dtStart);
            logger.LogInformation($"Osharp框架初始化完成，耗时：{ts:g}");
        }
    }
}