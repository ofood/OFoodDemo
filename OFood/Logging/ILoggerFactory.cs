using System;

namespace OFood.Logging
{
    /// <summary>
    /// 代表一个日志工厂
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// 用给定的日志名称创建一个日志
        /// </summary>
        ILogger Create(string name);
        /// <summary>
        /// 用给定的类型创建一个日志
        /// </summary>
        ILogger Create(Type type);
    }
}
