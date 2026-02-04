using FluentValidation;

namespace CRMSystem.API.Common.Validation;

public class FluentValidationFilter<TRequest> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<TRequest>>();
        if (validator is null)
        {
            return await next(context);
        }

        var model = context.Arguments.OfType<TRequest>().FirstOrDefault();
        if (model is null)
        {
            return await next(context);
        }

        var result = await validator.ValidateAsync(model, context.HttpContext.RequestAborted);
        if (!result.IsValid)
        {
            return Results.ValidationProblem(result.ToDictionary());
        }

        return await next(context);
    }
}