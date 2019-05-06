using Microsoft.Bot.Builder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherEchoBotV4.Helpers
{
    public class LuisParser
    {
        public static string GetEntityValue(RecognizerResult result, string label, string patternLabel)
        {
            foreach(var entity in result.Entities)
            {
                var location = JObject.Parse(entity.Value.ToString())[label];
                var locationPattern = JObject.Parse(entity.Value.ToString())[patternLabel];

                if(location != null || locationPattern != null)
                {
                    dynamic value = JsonConvert.DeserializeObject<dynamic>(entity.Value.ToString());
                    if(location != null)
                    {
                        return value.Location[0].text;
                    }

                    if(locationPattern != null)
                    {
                        return value.Location[0].text;
                    }
                }
            }
            return string.Empty;
        }

    }
}
