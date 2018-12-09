using Hylogger.Core;
using System;

namespace HyloogerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var fd = GetLogDetails("starting application", null);
            Hlogger.WriteDiagonistic(fd);

            var tracker = new PerfTracker("CrxConsole_Execution", "", fd.UserName, fd.Location, fd.Product, fd.Layer);

            try
            {
                var ex = new Exception("Something bad has happend!");
                ex.Data.Add("input param", "nothing to see here");
                throw ex;
            }
            catch (Exception ex)
            {

                fd = GetLogDetails("", ex);
                Hlogger.WriteError(fd);
            }
            fd = GetLogDetails("Used Hylogger Console", null);
            Hlogger.WriteUsage(fd);

            fd = GetLogDetails("stopping app", null);
            Hlogger.WriteDiagonistic(fd);

            tracker.Stop();
        }
        private static HylogDetails GetLogDetails(string message, Exception ex)
        {
            return new HylogDetails
            {
                Product = "crxClient",
                Location = "CrxConsole",
                Layer = "Job",
                UserName = Environment.UserName,
                HostName = Environment.MachineName,
                Message = message,
                Exception = ex
            };
        }
    }
}
