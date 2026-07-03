namespace TrainingDirectory.Api.Utils;

public static class Config
{
    public static void Initialize(IConfiguration configuration)
    {
        DotNetEnv.Env.Load();
    }
}
