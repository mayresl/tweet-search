using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace TwitterAPI.Controllers
{
    public class TwitterController : ApiController
    {
        private async Task<string> GetAccessToken()
        {
            string OAuthConsumerKey = "HWgSQJYsNadgLu7X9PzF8GXQK";
            string OAuthConsumerSecret = "OCmOkA7BDUq2qfdhWqeSewb4BLXdNxbkwwsQ2WOsz1cZJRn2of";
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth2/token ");
            var customerInfo = Convert.ToBase64String(new UTF8Encoding()
                                      .GetBytes(OAuthConsumerKey + ":" + OAuthConsumerSecret));
            request.Headers.Add("Authorization", "Basic " + customerInfo);
            request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8,
                                                                      "application/x-www-form-urlencoded");

            HttpResponseMessage response = await httpClient.SendAsync(request);

            string json = await response.Content.ReadAsStringAsync();
            var serializer = new JavaScriptSerializer();
            dynamic item = serializer.Deserialize<object>(json);
            return item["access_token"];
        }

        [HttpGet]
        public async Task<dynamic> GetTweets(string key, string accessToken = null)
        {
            if (accessToken == null)
            {
                accessToken = await GetAccessToken();
            }

            var request = new HttpRequestMessage(HttpMethod.Get,
                string.Format("https://api.twitter.com/1.1/search/tweets.json?q={0}", key));

            request.Headers.Add("Authorization", "Bearer " + accessToken);
            var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.SendAsync(request);
            var serializer = new JavaScriptSerializer();
            dynamic json = serializer.Deserialize<object>(await response.Content.ReadAsStringAsync());
            return json;
        }
    }    
}