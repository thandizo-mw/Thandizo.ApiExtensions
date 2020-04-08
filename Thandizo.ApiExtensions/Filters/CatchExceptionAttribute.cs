using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.IO;

namespace Thandizo.ApiExtensions.Filters
{
    public class CatchExceptionAttribute : ExceptionFilterAttribute
    {
        private readonly string _errorMessage;

        public CatchExceptionAttribute(string errorMessage = "")
        {
            _errorMessage = errorMessage;
        }

        public override void OnException(ExceptionContext context)
        {
            var message = context.Exception.Message;

            if (!string.IsNullOrEmpty(_errorMessage))
            {
                message = _errorMessage;
            }
            else
            {
                if (context.Exception.InnerException != null)
                {
                    message = context.Exception.InnerException.Message;
                }
            }

            var errorRootDirectory = "errors";
            if (!Directory.Exists(errorRootDirectory))
            {
                Directory.CreateDirectory(errorRootDirectory);
            }

            var monthDirectory = Path.Combine(errorRootDirectory, string.Format("{0:MMMyyyy}", DateTime.UtcNow).ToLower());
            if (!Directory.Exists(monthDirectory))
            {
                Directory.CreateDirectory(monthDirectory);
            }

            var logFileName = string.Format("error_log_{0:ddMMMyyyy}.txt", DateTime.Now);
            var fullPath = Path.Combine(monthDirectory, logFileName);

            using (var log = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.File(new CompactJsonFormatter(), fullPath)
                .CreateLogger())
            {
                log.Error(context.Exception, "Error Occured");
            }
            //************* end of serilog **************************************

            context.Result = new BadRequestObjectResult(message);
        }
    }
}
