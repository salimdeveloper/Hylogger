using System;
using System.Collections.Generic;

namespace Hylogger.Core
{
    public class HylogDetails
    {
        public HylogDetails()
        {
            TimeStamp = DateTime.Now;
        }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }

        public string Product { get; set; }
        public string Layer { get; set; }
        public string Location { get; set; }
        public string HostName { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public long? EllapsedMillisecounds { get; set; }//only for performance entries
        public Exception Exception { get; set; }// the exception for error logging
        public string CorrelationId { get; set; }// exception shelding from server to client
        public Dictionary<string, object> AdditionalInfo { get; set; } //catch all for anything else we might want to log
    }
}
