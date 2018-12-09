using Serilog;
using Serilog.Events;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Hylogger.Core
{
    public static class Hlogger
    {
        private static readonly ILogger _perfLogger;
        private static readonly ILogger _usageLogger;
        private static readonly ILogger _errorLogger;
        private static readonly ILogger _diagnosticLogger;
        static Hlogger()
        {
            _perfLogger = new LoggerConfiguration().WriteTo.File(path: "C:\\Users\\gigabyte\\hylogger\\perf.txt").CreateLogger();

            _usageLogger = new LoggerConfiguration().WriteTo.File(path: "C:\\Users\\gigabyte\\hylogger\\usage.txt").CreateLogger();

            _errorLogger = new LoggerConfiguration().WriteTo.File(path: "C:\\Users\\gigabyte\\hylogger\\error.txt").CreateLogger();

            _diagnosticLogger = new LoggerConfiguration().WriteTo.File(path: "C:\\Users\\gigabyte\\hylogger\\diagnostic.txt").CreateLogger();
        }
        public static void WritePerf(HylogDetails infoToLog)
        {
            _perfLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }
        public static void WriteUsage(HylogDetails infoToLog)
        {
            _usageLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }
        public static void WriteError(HylogDetails infoToLog)
        {
            if (infoToLog.Exception != null)
            {
                var procName = FindProcName(infoToLog.Exception);
                infoToLog.Location = string.IsNullOrEmpty(procName) ? infoToLog.Location : procName;
                infoToLog.Message = GetMessageFromException(infoToLog.Exception);
            }
            _errorLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }
        public static void WriteDiagonistic(HylogDetails infoToLog)
        {
            var writeDiagnostics = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableDiagnostics"]);
            if (!writeDiagnostics) return;

            _diagnosticLogger.Write(LogEventLevel.Information, "{@FlogDetail}", infoToLog);
        }
        private static string GetMessageFromException(Exception ex)
        {
            if (ex.InnerException != null)
                return GetMessageFromException(ex.InnerException);
            return ex.Message;
        }

        private static string FindProcName(Exception ex)
        {
            var sqlEx = ex as SqlException;
            if (sqlEx != null)
            {
                var procName = sqlEx.Procedure;
                if (!string.IsNullOrEmpty(procName))
                    return procName;
            }
            if (!string.IsNullOrEmpty((string)ex.Data["Procedure"]))
            {
                return (string)ex.Data["Procedure"];
            }
            if (ex.InnerException != null)
                return FindProcName(ex.InnerException);

            return null;
        }
    }

}
