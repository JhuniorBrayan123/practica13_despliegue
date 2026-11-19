using Lab13_JhuniorGutierrez.Api.Configuration;
using Lab13_JhuniorGutierrez.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// All service registration lives in the centralization file (Configuration/ApiServiceExtensions.cs).
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Lab13_JhuniorGutierrez API v1"));
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
