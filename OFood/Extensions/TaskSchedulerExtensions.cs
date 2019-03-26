using System;
using System.Threading;
using System.Threading.Tasks;

namespace OFood.Extensions
{
    public static class TaskFactoryExtensions
    {
        public static Task StartDelayedTask(this TaskFactory factory, int millisecondsDelay, Action action)
        {
            // 验证参数
            if (factory == null) throw new ArgumentNullException("factory");
            if (millisecondsDelay < 0) throw new ArgumentOutOfRangeException("millisecondsDelay");
            if (action == null) throw new ArgumentNullException("action");

            // 检查预先取消的标记
            if (factory.CancellationToken.IsCancellationRequested)
            {
                return new Task(() => { }, factory.CancellationToken);
            }

            // 创建定时任务
            var tcs = new TaskCompletionSource<object>(factory.CreationOptions);
            var ctr = default(CancellationTokenRegistration);

            // 创建计时器，但不要启动它。 如果我们现在开始，它可能会在构造函数被设置为正确的注册之前触发
            var timer = new Timer(self =>
            {
                // 理取消令牌和计时器，并尝试转换到完成
                ctr.Dispose();
                ((Timer)self).Dispose();
                tcs.TrySetResult(null);
            });

            // 注册取消令牌
            if (factory.CancellationToken.CanBeCanceled)
            {
                // When cancellation occurs, cancel the timer and try to transition to canceled.
                // There could be a race, but it's benign.
                ctr = factory.CancellationToken.Register(() =>
                {
                    timer.Dispose();
                    tcs.TrySetCanceled();
                });
            }

            // 启动计时器并交回任务...
            try { timer.Change(millisecondsDelay, Timeout.Infinite); }
            catch (ObjectDisposedException) { } // 

            return tcs.Task.ContinueWith(_ => action(), factory.CancellationToken, TaskContinuationOptions.OnlyOnRanToCompletion, factory.Scheduler ?? TaskScheduler.Current);
        }
    }
}