namespace Working.With.Cors.Api;

public static class Constants
{
    public static class CorsPolicyNames
    {
        public const string CorsPolicy = nameof(CorsPolicy); 
    }
    
    public static class TestApi
    {
        public const string Host     = "https://localhost:5146"; 
        public const string Endpoint = "api/v1/hello-world"; 
    }
}
