using System;
using System.Collections.Generic;
using OFood.Domain.Core.Options;
using OFood.Domain.Core.Packs;


namespace OFood.Domain.Core.Builders
{
    /// <summary>
    /// 定义OSharp构建器
    /// </summary>
    public interface IOFoodBuilder
    {
        /// <summary>
        /// 获取 加载的模块集合
        /// </summary>
        IEnumerable<Type> AddPacks { get; }

        /// <summary>
        /// 获取 排除的模块集合
        /// </summary>
        IEnumerable<Type> ExceptPacks { get; }

        /// <summary>
        /// 获取 OSharp选项配置委托
        /// </summary>
        Action<OFoodOptions> OptionsAction { get; }

        /// <summary>
        /// 添加指定模块
        /// </summary>
        /// <typeparam name="TPack">要添加的模块类型</typeparam>
        IOFoodBuilder AddPack<TPack>() where TPack : OFoodPack;

        /// <summary>
        /// 排除指定模块
        /// </summary>
        /// <typeparam name="TPack"></typeparam>
        /// <returns></returns>
        IOFoodBuilder ExceptPack<TPack>() where TPack : OFoodPack;

        /// <summary>
        /// 添加OSharp选项配置
        /// </summary>
        /// <param name="optionsAction">OSharp操作选项</param>
        /// <returns>OSharp构建器</returns>
        IOFoodBuilder AddOptions(Action<OFoodOptions>optionsAction);
    }
}