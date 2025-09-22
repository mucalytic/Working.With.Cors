using static Working.With.Cors.Api.Constants; 

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyNames.CorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:5146")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();
app.UseCors(CorsPolicyNames.CorsPolicy);
app.MapGet("api/v1/hello-world", () => "Hello World!");
app.Run();
