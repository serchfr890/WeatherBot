﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using WeatherEchoBotV4.Models;
using WeatherEchoBotV4.Helpers;
using WeatherEchoBotV4.Services;
using System.Collections.Generic;

namespace WeatherEchoBotV4
{
    public class WeatherEchoBotV4Bot : IBot
    {
        public static readonly string LuisKey = "WeatherLUISV4";
        private readonly BotService _services;
     
        public WeatherEchoBotV4Bot(BotService services)
        {
            _services = services ?? throw new System.ArgumentNullException(nameof(services));

            if (!_services.LuisServices.ContainsKey(LuisKey))
                throw new System.ArgumentException($"Invalid configuration....");
        }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var recognizer = await _services.LuisServices[LuisKey].RecognizeAsync(turnContext, cancellationToken);
                var topIntent = recognizer?.GetTopScoringIntent();
                
                switch (topIntent.Value.intent)
                {
                    case "Get_Weather_Condition":
                        {
                            if(topIntent != null && topIntent.HasValue && topIntent.Value.intent != "None")
                            {
                                var location = LuisParser.GetEntityValue(recognizer, Constants.LocationLabel, Constants.LocationPatternLabel);
                                if(location.ToString() != string.Empty)
                                {
                                    var ro = await WeatherService.GetWeather(location);
                                    var weather = $"{ro.weather.First().main}({ro.main.temp.ToString("N2")} °C)";
                                    var typing = Activity.CreateTypingActivity();
                                    var delay = new Activity { Type = "delay", Value = 5000 };
                                    var activities = new IActivity[]
                                    {
                                        typing,
                                        delay,
                                        MessageFactory.Text($"Weather of {location} is: {weather}"),
                                        MessageFactory.Text("Thanks for using our service!")
                                    };
                                    await turnContext.SendActivitiesAsync(activities);
                                }
                                else
                                {
                                    await turnContext.SendActivityAsync("Sorry, I don´t understand");
                                }
                            }
                            else
                            {
                                var msg = @"No LUIS intents were found.
                                This sample is about identifying a city and an intent:
                                'Find the current weather in a city'
                                Try typing 'What's the weather in Prague'";

                                await turnContext.SendActivityAsync(msg);
                            }
                            break;
                        }
                    case "QnAMaker":
                        {
                            var serviceQnAMaker = new QnAMakerService();
                            var answer = serviceQnAMaker.GetAnswer(turnContext.Activity.Text);
                            if (answer.Equals(Constants.AnswerNotFound))
                            {
                                await turnContext.SendActivityAsync("Lo siento, pero no estoy preparado para este tipo de preguntas.");
                            }
                            else
                            {
                                await turnContext.SendActivityAsync(answer);
                            }
                            break;
                        }
                    case "SearchVideo":
                        {
                            //var searchVideo = LuisParser.GetEntityValue(recognizer, Constants.VideoLabel, Constants.VideoPatternLabel);
                            var searchVideo = recognizer.Entities.Last.First[0].ToString();
                            var serviceVideoIndexer = new VideoIndexerService();
                            var response = Task.Run(() => serviceVideoIndexer.SearchVideo("valor"));
                            response.Wait();
                            var videoList = response.Result;


                            var reply = (turnContext.Activity as Activity).CreateReply();
                            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                        
                            var card = videoList.Select(r => new ThumbnailCard(r.VideoTitle, r.Description, r.Duration,
                                new List<CardImage> { new CardImage(url: r.Thumbnaill, r.VideoTitle) },
                                new List<CardAction>
                                {
                                    new CardAction(ActionTypes.OpenUrl, "ver",null, value: r.UrlVideo, text:"ver", displayText:"ver"),
                                    new CardAction(ActionTypes.OpenUrl, "Descargar", null, value: r.DownloadVideoUrl, text: "Descargar", displayText:"Descargar")
                                }
                                ).ToAttachment()).ToList();

                            if (card.Any())
                            {
                                await turnContext.SendActivityAsync("Gracias por la espera, estos son los videos que encontré");
                                reply.Attachments = card;
                                await turnContext.SendActivityAsync(reply);
                            }

                            break;
                        }
                }
            }
            else if (turnContext.Activity.Type == ActivityTypes.ConversationUpdate)
                await SendWelcomeMessageAsync(turnContext, cancellationToken);
            else
                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected", cancellationToken: cancellationToken);
        }

        private static async Task SendWelcomeMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(
                        $"Welcome to WeatherBotv4 {member.Name}!",
                        cancellationToken: cancellationToken);
                }
            }
        }
    }
}
