namespace CreditTransactionApi.Data;

using static Environment;

public static class ConnectionString
{
    public const string ENV_USER_LABEL = "POSTGRES_USER";
    public const string ENV_PASSWORD_LABEL = "POSTGRES_PASSWORD";
    public const string ENV_HOST_LABEL = "POSTGRES_HOST";
    public const string ENV_DB_LABEL = "POSTGRES_DB";
    public const string ENV_PORT_LABEL = "POSTGRES_PORT";
    public static string GenerateFromEnvironment()
    {
        string? user = GetEnvironmentVariable(ENV_USER_LABEL);
        string? password = GetEnvironmentVariable(ENV_PASSWORD_LABEL);
        string? host = GetEnvironmentVariable(ENV_HOST_LABEL);
        string? db = GetEnvironmentVariable(ENV_DB_LABEL);
        string? port = GetEnvironmentVariable(ENV_PORT_LABEL);

        if (user is null
            || password is null
            || host is null
            || db is null
            || port is null)
        {
            throw new Exception("Connection string environment variables not set");
        }

        return $"User ID={user};Password={password};Host={host};Port={port};Database={db};Pooling=true;";
    }
}