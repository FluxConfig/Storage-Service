using FluxConfig.Storage.Api.Interceptors.Utils;
using FluxConfig.Storage.Domain.Exceptions.Domain;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Status = Google.Rpc.Status;

namespace FluxConfig.Storage.Api.Interceptors;

public class InternalExceptionHandlerInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            RpcException rpcEx = MapExceptionToRpcException(ex, context);
            throw rpcEx;
        }
    }

    private static RpcException MapExceptionToRpcException(Exception ex, ServerCallContext context)
    {
        Status status = ex switch
        {
            DomainValidationException exception => RpcExceptionGenerator.GenerateBadRequestException(exception),

            DomainNotFoundException exception => RpcExceptionGenerator.InternalGenerateNotFoundException(exception, context),

            DomainAlreadyExistsException exception => RpcExceptionGenerator.InternalGenerateAlreadyExistsException(
                exception, context),

            _ => RpcExceptionGenerator.GenerateInternalException(
                callContext: context)
        };

        return status.ToRpcException();
    }
}