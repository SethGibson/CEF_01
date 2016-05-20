using UnityEngine;
using System.Collections;

namespace HTC
{
    public class LogUtilility
    {      
        // Logging level.
        public enum LogLevel
        {
            LOG_DEBUG,
            LOG_INFO,
            LOG_WARN,
            LOG_ERROR
        }

        private static string[] LogLevelMsg =
        {
            "DEBUG",
            "INFO",
            "WARN",
            "ERROR"
        };

        // The lowest logging level.
        public static LogLevel lowestLogLevel = LogLevel.LOG_DEBUG;

        public static void SetLowestLogLevel(LogLevel level)
        {
            lowestLogLevel = level;
        }

        public static void LogD(string msg, string loggingClassName = "", string loggingMethodName = "")
        {
            Log(LogLevel.LOG_DEBUG, msg, loggingClassName, loggingMethodName);
        }

        public static void LogI(string msg, string loggingClassName = "", string loggingMethodName = "")
        {
            Log(LogLevel.LOG_INFO, msg, loggingClassName, loggingMethodName);
        }

        public static void LogW(string msg, string loggingClassName = "", string loggingMethodName = "")
        {
            Log(LogLevel.LOG_WARN, msg, loggingClassName, loggingMethodName);
        }

        public static void LogE(string msg, string loggingClassName = "", string loggingMethodName = "")
        {
            Log(LogLevel.LOG_ERROR, msg, loggingClassName, loggingMethodName);
        }

        private static void Log(LogLevel level, string msg, string loggingClassName, string loggingMethodName)
        {
            // Check lowest log level.
            if ((int)level >= (int)lowestLogLevel)
            {
                // Check which logging level to use.
                if ((int)level <= (int)LogLevel.LOG_INFO)
                {
                    Debug.Log("[" + LogLevelMsg[(int)level] + "]" + (loggingClassName != "" ? "[" + loggingClassName + "::" + loggingMethodName + "] " : " ") + msg);
                }
                else if ((int)level == (int)LogLevel.LOG_WARN)
                {
                    Debug.LogWarning("[" + LogLevelMsg[(int)level] + "]" + (loggingClassName != "" ? "[" + loggingClassName + "::" + loggingMethodName + "] " : " ") + msg);
                }
                else if ((int)level >= (int)LogLevel.LOG_ERROR)
                {
                    Debug.LogError("[" + LogLevelMsg[(int)level] + "]" + (loggingClassName != "" ? "[" + loggingClassName + "::" + loggingMethodName + "] " : " ") + msg);
                }
            }
        }
    }
}