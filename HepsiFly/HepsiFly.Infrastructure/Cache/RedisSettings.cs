namespace HepsiFly.Infrastructure.Cache;

public class RedisSettings
{
    public string ConnectionString;
    public string Host;
    public string Port;
    public string Pwd;
    
    #region Const Values

    public const string ConnectionStringValue = nameof(ConnectionString);
    public const string HostValue = nameof(Host);
    public const string PortValue = nameof(Port);
    public const string PwdValue = nameof(Pwd);
    #endregion
}