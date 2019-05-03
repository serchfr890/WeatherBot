using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherEchoBotV4.Services
{
    public class BotService
    {
        public BotService(BotConfiguration configuration) 
        {
            foreach(var service in configuration.Services)
            {
                switch (service.Type)
                {
                    case ServiceTypes.Luis:
                        {
                            var luis = (LuisService)service;
                            if(luis is null)
                            {
                                throw new InvalidOperationException("The Luis Service is not configured correctly in your '.bot' file.");
                            }
                            var app = new LuisApplication(luis.AppId, luis.AuthoringKey, luis.GetEndpoint());
                            var recognizer = new LuisRecognizer(app);
                            this.LuisServices.Add(luis.Name, recognizer);
                            break;
                        }
                }
            }
        }
        public Dictionary<string, LuisRecognizer> LuisServices { get; } = new Dictionary<string, LuisRecognizer>();
    }
}
