﻿using System;
using System.Threading;
using System.Threading.Tasks;
using OFood.Extensions;


namespace OFood.EventBuses.Internal
{
    /// <summary>
    /// 支持<see cref="Action"/>的事件处理器
    /// </summary>
    internal class ActionEventHandler<TEventData> : EventHandlerBase<TEventData> where TEventData : class,IEventData
    {
        /// <summary>
        /// 初始化一个<see cref="ActionEventHandler{TEventData}"/>类型的新实例
        /// </summary>
        public ActionEventHandler(Action<TEventData> action)
        {
            Action = action;
        }

        /// <summary>
        /// 获取 事件执行的委托
        /// </summary>
        public Action<TEventData> Action { get; }
        
        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        public override void Handle(TEventData eventData)
        {
            eventData.CheckNotNull(nameof(eventData));
            Action(eventData);
        }

        /// <summary>
        /// 异步事件处理
        /// </summary>
        /// <param name="eventData">事件源数据</param>
        /// <param name="cancelToken">异步取消标识</param>
        /// <returns>是否成功</returns>
        public override Task HandleAsync(TEventData eventData, CancellationToken cancelToken = default(CancellationToken))
        {
            eventData.CheckNotNull(nameof(eventData));
            cancelToken.ThrowIfCancellationRequested();
            return Task.Run(() => Action(eventData), cancelToken);
        }
    }
}