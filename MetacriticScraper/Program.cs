using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MetacriticScraper
{
    /// <summary>
    /// System class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        /// <summary>
        /// Host builder.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <returns>Host builder object.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
