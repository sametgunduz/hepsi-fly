using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HepsiFly.Api.Middlewares;

 public static class LoggingEvents
    {
        public static readonly EventId InitDatabase = new EventId(101, "Error whilst creating and seeding database");
        public static readonly EventId InitLogger = new EventId(102, "Error whilst creating serilog mongodb sink");
        public static readonly EventId GlobalException = new EventId(201, "Global Exception Handler");
        public static readonly EventId ReqResLog = new EventId(301, "Request-Response Loggin Event");
    }
    
    public class ApiErrorResponse
    {
        public int StatusCode { set; get; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public List<string> Errors { get; set; }
    }

    public class ApiHost
    {
        public string Host { get; set; }
        public int? Port { get; set; }
    }

    public class ApiRequest
    {
        public string RequestId { get; set; }
        public string Method { get; set; }
        public bool HasFormContentType { get; set; }
        public string ClientIp { get; set; }
        public string Scheme { get; set; }
        public string ContentType { get; set; }
        public long? ContentLength { get; set; }
        public ApiHost Host { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Query { get; set; }
        public string Path { get; set; }
        public dynamic Payload { get; set; }
    }

    public class ApiResponse
    {
        public string ContentType { get; set; }
        public long? ContentLength { get; set; }
        public int StatusCode { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public dynamic Payload { get; set; }
    }

    public class RequestAndResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestAndResponseLoggingMiddleware> _logger;

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public RequestAndResponseLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestAndResponseLoggingMiddleware> logger
        )
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (
                context.WebSockets.IsWebSocketRequest ||
                context.Request.Path.Value.StartsWith("/swagger/") ||
                context.Request.ContentType != null &&
                context.Request.ContentType != MediaTypeNames.Application.Json
            )
            {
                await _next(context);
            }
            else
            {
                using (var loggableResponseStream = new MemoryStream())
                {
                    var originalResponseStream = context.Response.Body;

                    context.Response.Body = loggableResponseStream;

                    try
                    {
                        // Log request
                        _logger.LogInformation(LoggingEvents.ReqResLog, await FormatRequest(context));

                        await _next(context);

                        // Log response
                        _logger.LogInformation(LoggingEvents.ReqResLog, await FormatResponse(context));

                        if (loggableResponseStream.Length > 0)
                        {
                            //reset the stream position to 0
                            loggableResponseStream.Seek(0, SeekOrigin.Begin);

                            await loggableResponseStream.CopyToAsync(originalResponseStream);
                        }
                    }
                    finally
                    {
                        context.Response.Body = originalResponseStream;
                    }
                }
            }
        }

        private async Task<string> FormatRequest(HttpContext context)
        {
            var request = context.Request;

            request.EnableBuffering();

            var body = request.Body;

            var requestIdFeature = context.Features.Get<IHttpRequestIdentifierFeature>();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            var bodyAsText = Encoding.UTF8.GetString(buffer);

            body.Seek(0, SeekOrigin.Begin);

            var requestHeaders = request.Headers;

            request.Body = body;

            var reqLog = new ApiRequest
            {
                Method = request.Method,
                Path = request.Path,
                Headers = requestHeaders.ToDictionary(h => h.Key, h => h.Value.ToString()),
                Scheme = request.Scheme,
                HasFormContentType = request.HasFormContentType
            };

            if (requestIdFeature?.TraceIdentifier != null)
                reqLog.RequestId = requestIdFeature.TraceIdentifier;

            if (!string.IsNullOrWhiteSpace(request.ContentType))
                reqLog.ContentType = request.ContentType;

            if (request.ContentLength != null && request.ContentLength > 0)
                reqLog.ContentLength = request.ContentLength;

            if (request.HttpContext.Connection?.RemoteIpAddress != null)
                reqLog.ClientIp = request.HttpContext.Connection?.RemoteIpAddress?.ToString();

            if (request.Query != null && request.Query.Count > 0)
                reqLog.Query = request.Query.ToDictionary(h => h.Key, h => h.Value.ToString());

            if (!string.IsNullOrWhiteSpace(bodyAsText))
            {
                var payload = JToken.Parse(bodyAsText);

                reqLog.Payload = payload;
            }

            if (request.Host.HasValue)
            {
                reqLog.Host = new ApiHost
                {
                    Host = request.Host.Host,
                    Port = request.Host.Port
                };
            }

            var req = JsonConvert.SerializeObject(reqLog, JsonSerializerSettings);

            return req;
        }

        private async Task<string> FormatResponse(HttpContext context)
        {
            var response = context.Response;

            response.Body.Seek(0, SeekOrigin.Begin);

            var text = await new StreamReader(response.Body).ReadToEndAsync();

            var respLog = new ApiResponse
            {
                Headers = response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                StatusCode = response.StatusCode
            };

            if (response.Body.Length > 0)
            {
                try
                {
                    dynamic payload = JToken.Parse(text);
                    respLog.Payload = payload;
                }
                catch (JsonException e)
                {
                    _logger.LogDebug(e, "Response body parsing exception");
                    _logger.LogWarning($"could not parse response payload as jtoken, payload: {text}");
                    respLog.Payload = text;
                }
            }

            if (!string.IsNullOrWhiteSpace(respLog.ContentType))
                respLog.ContentType = respLog.ContentType;

            if (respLog.ContentLength != null && respLog.ContentLength > 0)
                respLog.ContentLength = respLog.ContentLength;

            var resp = JsonConvert.SerializeObject(respLog, JsonSerializerSettings);

            return resp;
        }
    }