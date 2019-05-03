using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherEchoBotV4.Models;
using WeatherEchoBotV4.Helpers;
using System.Net.Http;
using Newtonsoft.Json;

namespace WeatherEchoBotV4.Services
{
    public class WeatherService
    {
        public static async Task<WeatherModel> GetWeather(string city)
        {
            var query = $"{Constants.OpenWeatherMapUrl}?q={city}&appid={Constants.OpenWeatherMapKey}";
            using (var client = new HttpClient())
            {
                var getWeather = await client.GetAsync(query);

                if (getWeather.IsSuccessStatusCode)
                {
                    var json = await getWeather.Content.ReadAsStringAsync();
                    var weather = JsonConvert.DeserializeObject<WeatherModel>(json);
                    weather.main.temp = weather.main.temp - 273.15;
                    return weather;
                }
            }
            return default(WeatherModel);
        }
    }
}
