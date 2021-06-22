using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace NewsAggregator.Filters
{
    public class CustomExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var action = context.ActionDescriptor.DisplayName;
            var message = context.Exception.Message;
            var stackTrace = context.Exception.StackTrace;
            var httpRequest = context.HttpContext.Request;

            context.Result = new ViewResult()
            {
                ViewName = "CustomError"
            };

            Log.Error($"Error {context.Exception.Message} || {context.ActionDescriptor.DisplayName} || {context.Exception.StackTrace} || {context.HttpContext.Request}");

            context.ExceptionHandled = true;
        }
    }
}