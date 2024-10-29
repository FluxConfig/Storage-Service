using System.Text;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace FluxConfig.Storage.Api.Interceptors;

public class LoggerInterceptor : Interceptor
{
    private readonly ILogger<LoggerInterceptor> _logger;

    public LoggerInterceptor(ILogger<LoggerInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {   
        LogMethodCall<TRequest, TResponse>(context);
        LogHeaders(context);

        return await continuation(request, context);
    }

    private void LogMethodCall<TRequest, TResponse>(ServerCallContext context)
        where TRequest : class
        where TResponse : class
    {
        _logger.LogInformation(">>Executing call. Method: {methodName}, Request: {requestType}, Response: {responseType}",
            context.Method, typeof(TRequest), typeof(TResponse));
    }

    private void LogHeaders(ServerCallContext context)
    {
        StringBuilder headersMetadataBuilder = new StringBuilder();

        using IEnumerator<Metadata.Entry> headersEnumerator = context.RequestHeaders.GetEnumerator();
        while (headersEnumerator.MoveNext())
        {
            headersMetadataBuilder.Append($">>header: {headersEnumerator.Current.Key}, value: {headersEnumerator.Current.Value}\n");
        }
        headersEnumerator.Reset();;
        
        _logger.LogInformation("Passed headers:\n{headersInfo}", headersMetadataBuilder.ToString());
    }
}