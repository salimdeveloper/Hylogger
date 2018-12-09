using System;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;

namespace Hylogger.Core
{
    public class HloggerEfInterceptor : IDbCommandInterceptor
    {
        private Exception WrapEntityFrameworkException(DbCommand command, Exception ex)
        {
            var newException = new Exception("EntityFramework command faild!", ex);
            AddParamsToException(command.Parameters, newException);
            return newException;
        }
        private void AddParamsToException(DbParameterCollection parameters, Exception exception)
        {
            foreach (DbParameter param in parameters)
            {
                exception.Data.Add(param.ParameterName, param.Value.ToString());
            }
        }
        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            if (interceptionContext.Exception != null)
                interceptionContext.Exception = WrapEntityFrameworkException(command, interceptionContext.Exception);

        }

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            if (interceptionContext.Exception != null)
                interceptionContext.Exception = WrapEntityFrameworkException(command, interceptionContext.Exception);
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            if (interceptionContext.Exception != null)
                interceptionContext.Exception = WrapEntityFrameworkException(command, interceptionContext.Exception);
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
        }
    }
}
