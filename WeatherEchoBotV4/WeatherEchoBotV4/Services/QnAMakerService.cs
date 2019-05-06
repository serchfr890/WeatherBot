using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherEchoBotV4.Helpers;
using WeatherEchoBotV4.Models;

namespace WeatherEchoBotV4.Services
{
    public class QnAMakerService                                   
    {
        public string GetAnswer(string question)
        {
            //var url = $"{Constants.Host}/knowledgebases/{Constants.KnowledBaseId}/generateAnswer";
            var client = new RestClient(Constants.Host + "/knowledgebases/" + Constants.KnowledBaseId + "/generateAnswer");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization","EndpointKey "+ Constants.EndPointKey);
            //request.AddHeader("Content-Type", $"{Constants.FormatJson}");
            request.AddParameter(Constants.FormatJson ,"{\"question\": \"" + question + "\"}", ParameterType.RequestBody);
            var response = client.Execute(request);
            var result = JsonConvert.DeserializeObject<QnAMakerModel>(response.Content);

            if(result.Answers.Count > 0)
            {
                var answer = result.Answers[0].Answer;
                var score = result.Answers[0].Score;
                if(!answer.ToLower().Equals(Constants.AnswerNotFound) && score > 40)
                {
                    return answer;
                }
            }
            return Constants.AnswerNotFound;
        }
    }
}
