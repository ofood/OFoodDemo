using System;

namespace OFood.Logging
{
    /// <summary>
    /// 一个空日志，什么都不记录
    /// </summary>
    public class EmptyLogger : ILogger
    {
        public bool IsDataLogging => throw new NotImplementedException();

        public bool IsTraceEnabled => throw new NotImplementedException();

        public bool IsDebugEnabled => throw new NotImplementedException();

        public bool IsInfoEnabled => throw new NotImplementedException();

        public bool IsWarnEnabled => throw new NotImplementedException();

        public bool IsErrorEnabled => throw new NotImplementedException();

        public bool IsFatalEnabled => throw new NotImplementedException();

        public void Debug<T>(T message)
        {
            throw new NotImplementedException();
        }

        public void Debug(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Error<T>(T message)
        {
            throw new NotImplementedException();
        }

        public void Error(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Error<T>(T message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Error(string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Fatal<T>(T message)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Fatal<T>(T message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Info<T>(T message, bool isData)
        {
            throw new NotImplementedException();
        }

        public void Info(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Trace<T>(T message)
        {
            throw new NotImplementedException();
        }

        public void Trace(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Warn<T>(T message)
        {
            throw new NotImplementedException();
        }

        public void Warn(string format, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}