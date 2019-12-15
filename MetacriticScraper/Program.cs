using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MetacriticScraper
{
#pragma warning disable S1118 // Utility classes should not have public constructors
                             /// <summary>
                             /// System class.
                             /// </summary>
    public class Program
#pragma warning restore S1118 // Utility classes should not have public constructors
    {
        /// <summary>
        /// Main entry.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        /// <summary>
        /// Web host builder.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <returns>Web host builder object.</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
