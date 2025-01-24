


using API.Errors;
using Application.Interfaces;
using Infrastructre.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace API.ExtensionsMethods;
public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddScoped<IFileProcessingService, FileProcessingService>();
        services.AddScoped<IZipFileProcessingService, ZipFileProcessingService>();
        services.AddApiBehaviorOptions();
        return services;
    }

    private static void AddApiBehaviorOptions(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                var errorResponse = new ApiValidationErrorResponse { Errors = errors };
                return new BadRequestObjectResult(errorResponse);
            };
        });
    }
}
