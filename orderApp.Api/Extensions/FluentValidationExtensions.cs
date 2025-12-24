using FluentValidation;
using FluentValidation.AspNetCore;
using OrderApp.Application.Validators;

namespace OrderApp.Api.Extensions;

public static class FluentValidationExtensions
{
    public static IServiceCollection AddFluentValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<InputOrderDtoValidator>();

        // 可选：自动验证（ASP.NET Core 集成）
        services.AddFluentValidationAutoValidation();

        return services;
    }
}