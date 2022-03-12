using System.Diagnostics;
using System.Net.Mime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using HepsiFly.Common.Exceptions;

namespace HepsiFly.Api.Middlewares;

public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                    return;

                try
                {
                    HepsiFlyException exception = null;

                    switch (ex)
                    {
                        case HepsiFlyException ae:

                            if (ae.Code == "JhCzhu")
                                context.Response.Headers.Add("WWW-Authenticate",
                                    "error=\"invalid_refresh_token\", error_description=\"The refresh token is expired\"");
                            exception = ae;
                            break;

                        default:

                            exception = new HepsiFlyException()
                            {
                                Code = "UNdB3K",
                                Title = "Internal error occured",
                                Detail = ex.Message,
                                StackTrace = ex.StackTrace
                            };

                            _logger.LogError(
                                LoggingEvents.GlobalException,
                                ex,
                                "Global Exception Handler Default Catcher"
                            );

                            break;
                    }

                    var apiErrorResponseViewModel = new ApiErrorResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Title = exception.Title,
                        Detail = exception.Detail,
                        Errors = exception.Errors
                    };

                    var json = JsonConvert.SerializeObject(apiErrorResponseViewModel, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented,
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    context.Response.StatusCode = apiErrorResponseViewModel.StatusCode;

                    //PTM => Processing Time Milliseconds
                    if (context.Items["watch"] is Stopwatch watch)
                        context.Response.Headers.Add("x-ptm", new[] {watch.ElapsedMilliseconds.ToString()});

                    var apiErrorLogModel = new ApiErrorResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Title = exception.Title,
                        Detail = exception.Detail
                    };

                    if (!string.IsNullOrWhiteSpace(exception.StackTrace))
                        _logger.LogError(
                            LoggingEvents.GlobalException,
                            exception,
                            "Global Exception Handler Exception"
                        );

                    var logJson = JsonConvert.SerializeObject(apiErrorLogModel, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Formatting = Formatting.Indented,
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    _logger.LogInformation(LoggingEvents.ReqResLog, GetResponseLog(context, logJson));

                    await context.Response.WriteAsync(json);
                }
                catch (Exception ex2)
                {
                    _logger.LogError(0, ex2, "Global Exception Handler Response Creation Catcher");
                }
            }
        }

        private static string GetResponseLog(HttpContext context, string responseJson)
        {
            var respLog = new ApiResponse
            {
                Headers = context.Response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                StatusCode = context.Response.StatusCode
            };

            if (!string.IsNullOrEmpty(responseJson))
            {
                dynamic payload = JToken.Parse(responseJson);
                respLog.Payload = payload;
            }

            if (!string.IsNullOrWhiteSpace(respLog.ContentType))
                respLog.ContentType = respLog.ContentType;

            if (respLog.ContentLength != null && respLog.ContentLength > 0)
                respLog.ContentLength = respLog.ContentLength;

            var resp = JsonConvert.SerializeObject(respLog, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return resp;
        }
    }