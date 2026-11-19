using Lab13_JhuniorGutierrez.Api.Configuration;
using Lab13_JhuniorGutierrez.Api.Middleware;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsProduction())
{
    var forwardedOptions = new ForwardedHeadersOptions
    {
        ForwardedHeaders =
            ForwardedHeaders.XForwardedFor |
            ForwardedHeaders.XForwardedProto
    };

   
    forwardedOptions.KnownNetworks.Clear();
    forwardedOptions.KnownProxies.Clear();

    app.UseForwardedHeaders(forwardedOptions);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

var swaggerEnabled =
    app.Environment.IsDevelopment() ||
    app.Configuration.GetValue<bool>("Swagger:Enabled");

if (swaggerEnabled)
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "Lab13_JhuniorGutierrez API v1"
        );
    });
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Ok(new
    {
        message = "API de Tickets - Laboratorio 13",
        status = "Online",
        framework = ".NET 10",
        database = "PostgreSQL - Neon",
        hosting = "Render"
    }))
    .AllowAnonymous();

app.MapGet("/health", () => Results.Ok(new
    {
        status = "Healthy",
        timestamp = DateTime.UtcNow
    }))
    .AllowAnonymous();

app.Run();