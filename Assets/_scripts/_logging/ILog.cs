﻿
namespace Logging
{
    public interface ILog
    {
        void Debug(string message, params object[] args);
        void Warn(string message, params object[] args);
        void Error(string message, params object[] args);
    }
}
