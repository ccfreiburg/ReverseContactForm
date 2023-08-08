namespace ContRev.Backend {
public class Program
{
    public static void Main(string[] args)
    {

        var builder = CreateHostBuilder(args);

 
        var app = builder.Build();


        app.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>().UseUrls("http://0.0.0.0:5084");
            });
}
}