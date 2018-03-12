using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Medico.Service.DynamicForm
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost
                //.ConfigureAppConfiguration((c,b)=>b.AddJsonFile(""))
                .CreateDefaultBuilder(args)
                .UseKestrel() 
                 .UseUrls("http://*:6500")
                .UseStartup<Startup>()
                .Build();
    }
}
