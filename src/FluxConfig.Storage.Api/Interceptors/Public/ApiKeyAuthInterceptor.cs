using FluxConfig.Storage.Api.Clients.Interfaces;
using FluxConfig.Storage.Api.Contracts.InternalAPI;
using FluxConfig.Storage.Api.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace FluxConfig.Storage.Api.Interceptors.Public;

public class ApiKeyAuthInterceptor : Interceptor
{
    private readonly IManagementServiceClient _managementServiceClient;

    public ApiKeyAuthInterceptor(IManagementServiceClient client)
    {
        _managementServiceClient = client;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        await Authenticate(context);
        return await continuation(request, context);
    }

    private async Task Authenticate(ServerCallContext context)
    {
        string apiKey = context.RequestHeaders.GetValue("X-API-KEY") ??
                        throw new ClientServiceUnauthenticatedException("Empty authentication metadata.");

        var authResponse = await _managementServiceClient.AuthenticateClient(
            request: new AuthClientRequest(apiKey),
            cancellationToken: context.CancellationToken
        );

        context.RequestHeaders.Add("X-CFG-KEY", authResponse.ConfigurationKey);
    }
}