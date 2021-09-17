using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using twitter_api_service.Processor;
using twitter_api_service.Processor.Interfaces;

namespace twitter_api_service.Processor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host;

            try
            {
                host = CreateHostBuilder(args).Build();               

                // Trigger the IO container to instaniate the Singleton component to start
                // connecting to Twitter API endpoints and start receiving tweets.                         
                host.Services.GetService(typeof(TweetRetriever));

                // Trigger the IO container to instaniate the Singleton component to start
                // processing incoming tweets and do need real time calculation.
                host.Services.GetService(typeof(ITweetProcessor));
                
                host.Run();            
            }
            catch (Exception err)
            {
                Console.WriteLine($"Error: {err.Message}");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder hostBuilder;

            hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

            return hostBuilder;
        }
    }
}
