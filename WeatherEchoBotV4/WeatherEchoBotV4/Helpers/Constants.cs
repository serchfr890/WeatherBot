using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherEchoBotV4.Helpers
{
    public class Constants
    {
        //Constans for LUIS Application
        public readonly static string LuisArgs = "LuisEntities";
        public readonly static string LocationLabel = "Location";
        public readonly static string LocationPatternLabel = "Location_PatternAny";

        //Constans for OpenWeatherMap 
        public static string OpenWeatherMapUrl = $"http://api.openweathermap.org/data/2.5/weather";
        public readonly static string OpenWeatherMapKey = "2817f92970213e70ff9273be2b1cb75d";

        //Constans for QnA Maker Application 
        public readonly static string Host = "https://kbweather.azurewebsites.net/qnamaker";
        public readonly static string KnowledBaseId = "3df2107d-e1cc-4f9e-9cd1-0e7cea1fa216";
        public readonly static string EndPointKey = "a630bae5-2abf-447e-8ff5-b653a52dc5d0";
        public readonly static string FormatJson = "application/json";
        public readonly static string AnswerNotFound = "Lo siento, No encontré la respuesta para tu pregunta, " +
            " estaremos trabajando en ellos para darte pronto una solución.";

        //Constants for Video Indexer Application
        public readonly static string OCP_APIM_SUBCRIPTION_KEY = "Ocp-Apim-Subscription-Key";
        public readonly static string SUBSCRIPTION_KEY = "9910bef53a484b1ba9eb9961f8815d64";
        public readonly static string LOCATION = "trial";
        public readonly static string ACCOUNTID = "bc129b9e-ee67-4686-ba40-76f051a5911f";
        public readonly static string VideoLabel = "Video";
        public readonly static string VideoPatternLabel = "Video_PatternAny";
    }
}
