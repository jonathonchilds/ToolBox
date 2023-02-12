using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToolBox.Models;
using ToolBox.Utils;

namespace ToolBox
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Utilities.CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var canContinue = await Utilities.WaitForMigrations(host, context);

                if (!canContinue)
                {
                    return;
                }
            }

            var task = host.RunAsync();

            Utilities.Notify("ToolBox Running!");

            Console.WriteLine("You must also have the ClientApp running. In a separate terminal, run:");
            Console.WriteLine("    cd ClientApp");
            Console.WriteLine("    npm start");
            WebHostExtensions.WaitForShutdown(host);
        }
    }
}
