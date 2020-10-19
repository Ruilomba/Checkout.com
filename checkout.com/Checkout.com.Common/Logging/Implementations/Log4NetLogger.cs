namespace Checkout.com.Common.Logging.Implementations
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using log4net;
    using Newtonsoft.Json;

    public class Log4NetLogger : Logging.ILog
    {
        private readonly ILog log;

        public Log4NetLogger()
        {
        }

        public void LogInfo(string message, Func<object> objectFunc = null, Exception exception = null)
        {
            GetLogger().Info(this.GetLogObject(message, objectFunc, exception), exception);
        }

        public void LogError(string message, Func<object> objectFunc = null, Exception exception = null)
        {
            GetLogger().Error(this.GetLogObject(message, objectFunc, exception), exception);
        }

        public void LogWarning(string message, Func<object> objectFunc = null, Exception exception = null)
        {
            GetLogger().Warn(this.GetLogObject(message, objectFunc, exception), exception);
        }

        private ILog GetLogger()
        {
            StackTrace stackTrace = new StackTrace();
            return LogManager.GetLogger(stackTrace.GetFrame(1).GetMethod().DeclaringType);
        }

        private object GetLogObject(string message, Func<object> objectFunc, Exception exception)
        {
            object logObject;
            if (objectFunc != null && exception != null)
            {
                logObject = new
                {
                    Message = message,
                    Data = JsonConvert.SerializeObject(objectFunc()),
                    Exception = exception
                };
            }
            else if (objectFunc != null)
            {
                logObject = new
                {
                    Message = message,
                    Data = JsonConvert.SerializeObject(objectFunc()),
                };
            }
            else if (exception != null)
            {
                logObject = new
                {
                    Message = message,
                    Exception = exception,
                };
            }
            else
            {
                logObject = new
                {
                    Message = message,
                };
            }

            return logObject;
        }
    }
}