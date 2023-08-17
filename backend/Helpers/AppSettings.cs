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
    public AppSettings(IConfiguration configuration)
    {
        var appSettings = configuration.GetSection("AppSettings");
        BaseUrl = appSettings["BaseUrl"];
        HomeUrl = appSettings["HomeUrl"];
        Valid = appSettings["Valid"];
        Secret = appSettings["Secret"];
        EmailFrom = appSettings["EmailFrom"];
        SmtpHost = appSettings["SmtpHost"];
        SmtpPort = Convert.ToInt32(appSettings["SmtpPort"]);
        SmtpUser = appSettings["SmtpUser"];
        SmtpPass = appSettings["SmtpPass"];
        Admin = appSettings["Admin"];
        Pass = appSettings["Pass"];
    }

}