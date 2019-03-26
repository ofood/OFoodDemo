using System;
using System.Collections.Generic;
using System.Linq;

using OFood.Collections;
using OFood.Domain.Core.Options;
using OFood.Domain.Core.Packs;
using OFood.Domain.Data;
using OFood.Extensions;

namespace OFood.Domain.Core.Builders
{
    /// <summary>
    /// OSharp构建器
    /// </summary>
    public class OFoodBuilder : IOFoodBuilder
    {
        /// <summary>
        /// 初始化一个<see cref="OsharpBuilder"/>类型的新实例
        /// </summary>
        public OFoodBuilder()
        {
            AddPacks = new List<Type>();
            ExceptPacks = new List<Type>();
        }

        /// <summary>
        /// 获取 加载的模块集合
        /// </summary>
        public IEnumerable<Type> AddPacks { get; private set; }

        /// <summary>
        /// 获取 排除的模块集合
        /// </summary>
        public IEnumerable<Type> ExceptPacks { get; private set; }

        /// <summary>
        /// 获取 OSharp选项配置委托
        /// </summary>
        public Action<OFoodOptions> OptionsAction { get; private set; }

        /// <summary>
        /// 添加指定模块，执行此功能后将仅加载指定的模块
        /// </summary>
        /// <typeparam name="TPack">要添加的模块类型</typeparam>
        public IOFoodBuilder AddPack<TPack>() where TPack : OFoodPack
        {
            List<Type> list = AddPacks.ToList();
            list.AddIfNotExist(typeof(TPack));
            AddPacks = list;
            return this;
        }

        /// <summary>
        /// 移除指定模块，执行此功能以从自动加载的模块中排除指定模块
        /// </summary>
        /// <typeparam name="TPack"></typeparam>
        /// <returns></returns>
        public IOFoodBuilder ExceptPack<TPack>() where TPack : OFoodPack
        {
            List<Type> list = ExceptPacks.ToList();
            list.AddIfNotExist(typeof(TPack));
            ExceptPacks = list;
            return this;
        }

        /// <summary>
        /// 添加OSharp选项配置
        /// </summary>
        /// <param name="optionsAction">OSharp操作选项</param>
        /// <returns>OSharp构建器</returns>
        public IOFoodBuilder AddOptions(Action<OFoodOptions> optionsAction)
        {
            optionsAction.CheckNotNull(nameof(optionsAction));
            OptionsAction = optionsAction;
            return this;
        }
    }
}