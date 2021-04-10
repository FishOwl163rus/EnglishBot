using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnglishBot.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;

namespace EnglishBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IVkApi>(provider =>
                    {
                        var api = new VkApi();
                        
                        api.Authorize(new ApiAuthParams
                        {
                            AccessToken = hostContext.Configuration["VKToken"]
                        });
                        
                        return api;
                    });
                    services.AddSingleton<IAsset, AssetManager>();
                    services.AddHostedService<BotService>();
                });
    }
}