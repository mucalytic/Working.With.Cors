using static Working.With.Cors.Api.Constants;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyNames.CorsPolicy, policy =>
    {
        policy.WithOrigins(TestApi.Host)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();
app.UseCors(CorsPolicyNames.CorsPolicy);
app.MapGet(TestApi.Endpoint, () => "Hello World!");
app.Run();
