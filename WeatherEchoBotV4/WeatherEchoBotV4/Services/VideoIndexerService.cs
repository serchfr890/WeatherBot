using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using WeatherEchoBotV4.Helpers;
using WeatherEchoBotV4.Models;

namespace WeatherEchoBotV4.Services
{
    public class VideoIndexerService
    {
        public async Task<string> GetAccountAccessTokenAsync()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add(Constants.OCP_APIM_SUBCRIPTION_KEY, Constants.SUBSCRIPTION_KEY);
            var url = $"https://api.videoindexer.ai/auth/{Constants.LOCATION}/Accounts/{Constants.ACCOUNTID}/AccessToken";
            var responseMessage = await client.GetAsync(url);
            return await responseMessage.Content.ReadAsStringAsync();
        }

        public async Task<string> GetVideoSourceFileDownloadUrl(string videoId)
        {
            var client = new HttpClient();
            var uri = $"https://api.videoindexer.ai/{Constants.LOCATION}/Accounts/{Constants.ACCOUNTID}/Videos/{videoId}/SourceFile/DownloadUrl/";
            var responseMessage = await client.GetAsync(uri);
            return await responseMessage.Content.ReadAsStringAsync();
        }

        public async Task<List<VideoDetailsModel>> SearchVideo(string searchVideo)
        {
            List<VideoDetailsModel> videosList = new List<VideoDetailsModel>();

            //Se obtiene la respuesta de la busqueda del video desde VideoIndexer
            var result = Task.Run(() => GetAccountAccessTokenAsync());
            result.Wait();
            var accessToken = result.Result.Trim('\"');
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["query"] = searchVideo;
            queryString["pageSize"] = "25";
            queryString["skip"] = "0";
            queryString["accessToken"] = accessToken;
            var uri = $"https://api.videoindexer.ai/{Constants.LOCATION}/Accounts/{Constants.ACCOUNTID}/Videos/Search?{queryString}";
            var client = new HttpClient();
            var responseMessage = await client.GetAsync(uri);
            var responseMessageResult = await responseMessage.Content.ReadAsStringAsync();
            var jsonResult = JsonConvert.DeserializeObject<VideoIndexerModel>(responseMessageResult);

            queryString.Clear();
            queryString["format"] = "Jpeg";
            queryString["accessToken"] = accessToken;
            foreach (var videoDetails in jsonResult.results)
            {
                VideoDetailsModel videoDetail = new VideoDetailsModel();
                videoDetail.VideoTitle = videoDetails.name;
                videoDetail.Duration = videoDetails.searchMatches[0].startTime;
                videoDetail.UrlVideo = $"https://www.videoindexer.ai/accounts/{Constants.ACCOUNTID}/videos/{videoDetails.id}/?";
                videoDetail.Thumbnaill = $"https://api.videoindexer.ai/{Constants.LOCATION}/Accounts/{Constants.ACCOUNTID}/Videos/{videoDetails.id}/Thumbnails/{videoDetails.thumbnailId}?{queryString}";
                videoDetail.Description = videoDetails.searchMatches[0].text;
                var url = Task.Run(() => GetVideoSourceFileDownloadUrl(videoDetails.id));
                url.Wait();
                videoDetail.DownloadVideoUrl = url.Result.Trim('\"');

                videosList.Add(videoDetail);
               

                /*
                var videoTitle = videoDetails.name;
                var duration = videoDetails.searchMatches[0].startTime;
                var urlVideo = $"https://www.videoindexer.ai/accounts/{Constants.ACCOUNTID}/videos/{videoDetails.id}/?";
                var thumbnaill = $"https://api.videoindexer.ai/{Constants.LOCATION}/Accounts/{Constants.ACCOUNTID}/Videos/{videoDetails.id}/Thumbnails/{videoDetails.thumbnailId}?{queryString}";
                var description = videoDetails.searchMatches[0].text;
                var url = Task.Run(() => GetVideoSourceFileDownloadUrl(videoDetails.id));
                url.Wait();
                var downloadVideoUrl = url.Result.Trim('\"');*/
            }
            return videosList;

        }
    }
}
