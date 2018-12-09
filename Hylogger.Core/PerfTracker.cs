using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Hylogger.Core
{
    public class PerfTracker
    {
        private readonly Stopwatch _sw;
        private readonly HylogDetails _infoToLog;

        public PerfTracker(string name, string userId, string userName, string location, string product, string layer)
        {
            _sw = Stopwatch.StartNew();
            _infoToLog = new HylogDetails()
            {
                Message = name,
                UserId = userId,
                UserName = userName,
                Product = product,
                Layer = layer,
                Location = location,
                HostName = Environment.MachineName
            };
            var beginTime = DateTime.Now;
            _infoToLog.AdditionalInfo = new Dictionary<string, object>()
            {
                { "Started",beginTime.ToString(CultureInfo.InvariantCulture) }
            };
        }

        public PerfTracker(string name, string userId, string userName, string location, string product, string layer, Dictionary<string, object> perfParams) :
            this(name, userId, userName, location, product, layer)
        {
            foreach (var item in perfParams)
            {
                _infoToLog.AdditionalInfo.Add("input-" + item.Key, item.Value);
            }
        }
        public void Stop()
        {
            _sw.Stop();
            _infoToLog.EllapsedMillisecounds = _sw.ElapsedMilliseconds;
            Hlogger.WritePerf(_infoToLog);
        }
    }
}
