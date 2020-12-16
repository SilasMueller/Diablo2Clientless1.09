﻿using ConsoleBot.Bots.Types.Cows;
using ConsoleBot.Bots.Types.Mephisto;
using ConsoleBot.Bots.Types.Travincal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleBot.Bots.Types
{
    public static class BotTypeExtensions
    {
        public static void RegisterBotServices(this IServiceCollection services, IConfigurationRoot config)
        {
            services.AddSingleton<IBotInstance, MephistoBot>();
            services.AddSingleton<IBotInstance, TravincalBot>();
            services.AddSingleton<IBotInstance, TestBot>();
            services.AddSingleton<IBotInstance, CowBot>();
            services.AddSingleton<IBotFactory, BotFactory>();
            services.AddOptions<CowConfiguration>()
                .Bind(config.GetSection("bot").GetSection("cows"));
            services.AddOptions<MephistoConfiguration>()
                .Bind(config.GetSection("bot").GetSection("mephisto"));
            services.AddOptions<TravincalConfiguration>()
                .Bind(config.GetSection("bot").GetSection("travincal"));
        }
    }
}
