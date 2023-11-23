using HotChocolate.Utilities;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace Template.API.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    string message = string.Empty;
                    if (contextFeature.Error is ServiceException exception)
                    {
                        context.Response.StatusCode = (int)exception.StatusCode;
                        message = exception.Message;
                        if (exception.LogError)
                            logger.LogError(contextFeature.Error.ToString());
                    }
                    else if (contextFeature.Error is OperationCanceledException //||
                                                                                //(contextFeature.Error is Microsoft.Data.SqlClient.SqlException sqlE && sqlE.Message.Contains("Operation cancelled by user"))
                                        )
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                        message = "Request cancelled";
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        message = "Internal Server Error.";
                        logger.LogError(contextFeature.Error.ToString());
                    }
                    await context.Response.WriteAsync(message);
                }
            });
        });
    }
}
