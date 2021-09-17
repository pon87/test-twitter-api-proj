using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Tweetinvi;
using Tweetinvi.Models;
using twitter_api_service.Entity;
using twitter_api_service.Repository.Interfaces;
using twitter_api_service.Processor;
using twitter_api_service.Processor.Interfaces;
using twitter_api_service.Repository;
using twitter_api_service.Utility.Interfaces;
using twitter_api_service.Utility;

namespace twitter_api_service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "twitter_api_service", Version = "v1" });
            });

            services.AddLogging(configure => configure.AddConsole());

            ConfigureTwitterService(services);

            services.AddSingleton<IRepository<AnalysisData>, InMemoryRepository>();

            services.AddSingleton<IDateTimeDiff, DateTimeDiffUtility>();
            services.AddSingleton<ITweetRetriever, TweetRetriever>();
            services.AddSingleton<ITweetProcessor, TweetProcessor>();
        }

        void ConfigureTwitterService(IServiceCollection services)
        {
            TwitterCredentials twitterCredentials;

            TwitterClient twitterClient;

            twitterCredentials = new TwitterCredentials();
            twitterCredentials.AccessToken = "1285798468447944704-DtyG5roobTjcFoti9av2FF69xhTAUj";
            twitterCredentials.AccessTokenSecret = "jbDjxQKw8DN3G4Z4uddb3E7dRa0nIAjuQXmYTfrczkjS1";
            twitterCredentials.BearerToken = "AAAAAAAAAAAAAAAAAAAAAOjcTgEAAAAA8Ta%2Bk48r1zF3Skz2q4Ivm85iKuA%3DLoM76SxExZMxLnBpDbHICbwDSxpFE1dr5LS1RHMfn1OcIrAlV7";
            twitterCredentials.ConsumerKey = "nseVc1jXpXzxiR82h8OD4ktvv";
            twitterCredentials.ConsumerSecret = "F1jdGgrwLhWuDY4UPB5KAERHDztmCaQPMfxErAcF0x3RqbCFhV";

            twitterClient = new TwitterClient(twitterCredentials);

            services.AddLogging(configure => configure.AddConsole());

            services.AddSingleton<ITwitterCredentials>(twitterCredentials);
            services.AddSingleton<ITwitterClient>(twitterClient);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "twitter_api_service v1"));
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
