using MediatR;

namespace ShoppingSystem.Microservice.Application.MediatR.Setting;

public class ValidationBehaviour<TRequest,TResponse> :
IPipelineBehavior<TRequest,TResponse> where TRequest : IRequest<TResponse>
{
    //todo : logs  => validations & performance
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public Task<TResponse> Handle
    (TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}