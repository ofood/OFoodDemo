using System;

namespace OFood.Logging
{
    /// <summary>
    /// ILoggerFactory的一个空实现
    /// </summary>
    public class EmptyLoggerFactory : ILoggerFactory
    {
        private static readonly EmptyLogger Logger = new EmptyLogger();

        /// <summary>
        /// 按名称创建一个空的日志实例
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ILogger Create(string name)
        {
            return Logger;
        }
        /// <summary>
        /// 按类型创建一个空的日志实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ILogger Create(Type type)
        {
            return Logger;
        }
    }
}
