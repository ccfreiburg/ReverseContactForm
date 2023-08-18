using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;

namespace ContRev.Backend;

public class AppSettings 
{
    public string Secret { get; private set; }
    public string BaseUrl { get; private set; }
    public string HomeUrl { get; private set; }
    public string Valid { get; private set; }
    public string EmailFrom { get; private set; }
    public string SmtpHost { get; private set; }
    public int SmtpPort { get; private set; }
    public string SmtpUser { get; private set; }
    public string SmtpPass { get; private set; }
    public string Admin { get; private set; }
    public string Pass { get; private set; }

    private T getConfigValue<T>( IConfiguration configuration, string key ) {
        var env_key = "RCF_" + key.ToUpper();
        var value = configuration.GetValue<T>(env_key);
        if (value!=null)
            return value;
        var appSettings = configuration.GetSection("AppSettings");
        if (typeof(T) is Int32)
            return (T)Convert.ChangeType(Convert.ToInt32(appSettings[key]), typeof(T));
        return (T)Convert.ChangeType(appSettings[key], typeof(T));
    }
    
    public AppSettings(IConfiguration configuration)
    {
        BaseUrl = getConfigValue<string>(configuration,"BaseUrl");
        HomeUrl = getConfigValue<string>(configuration,"HomeUrl");
        Valid = getConfigValue<string>(configuration,"Valid");
        Secret = getConfigValue<string>(configuration,"Secret");
        EmailFrom = getConfigValue<string>(configuration,"EmailFrom");
        SmtpHost = getConfigValue<string>(configuration,"SmtpHost");
        SmtpPort = getConfigValue<int>(configuration,"SmtpPort");
        SmtpPass = getConfigValue<string>(configuration,"SmtpPass");
        SmtpUser = getConfigValue<string>(configuration,"SmtpUser");
        Admin = getConfigValue<string>(configuration,"Admin");
        Pass = getConfigValue<string>(configuration,"Pass");
    }

}