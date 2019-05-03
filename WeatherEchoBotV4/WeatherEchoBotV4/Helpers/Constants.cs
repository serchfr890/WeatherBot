using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherEchoBotV4.Helpers
{
    public class Constants
    {
        public readonly static string LuisArgs = "LuisEntities";
        public readonly static string LocationLabel = "Location";
        public readonly static string LocationPatternLabel = "Location_PatternAny";

        public static string OpenWeatherMapUrl = $"http://api.openweathermap.org/data/2.5/weather";
        public readonly static string OpenWeatherMapKey = "2817f92970213e70ff9273be2b1cb75d";
    }
}
