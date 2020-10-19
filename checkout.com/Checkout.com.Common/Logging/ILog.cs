namespace Checkout.com.Common.Logging
{
    using System;

    public interface ILog
    {
        void LogInfo(string message, Func<object> objectFunc, Exception exception = null);

        void LogError(string message, Func<object> objectFunc, Exception exception = null);

        void LogWarning(string message, Func<object> objectFunc, Exception exception = null);
    }
}