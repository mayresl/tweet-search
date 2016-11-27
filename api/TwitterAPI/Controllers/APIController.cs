using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using System.Configuration;

namespace TwitterAPI.Controllers
{
    [EnableCors(origins:"*", headers:"*", methods: "*")]
    public class TwitterController : ApiController
    {
        private async Task<string> GetAccessToken()
        {
            string OAuthConsumerKey = ConfigurationManager.AppSettings["OAuthConsumerKey"];
            string OAuthConsumerSecret = ConfigurationManager.AppSettings["OAuthConsumerSecret"];
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
        public async Task<dynamic> GetTweets(string key, int count, string last_id = null, string accessToken = null)
        {
            key = key.Replace("@", "%40");
            key = key.Replace("#", "%23");
            key = key.Replace(" ", "+");
            if (accessToken == null)
            {
                accessToken = await GetAccessToken();
            }

            HttpRequestMessage request = null;
            if (string.IsNullOrEmpty(last_id))
            {
                request = new HttpRequestMessage(HttpMethod.Get,
                    string.Format("https://api.twitter.com/1.1/search/tweets.json?q={0}&count={1}", key, count));
            }
            else
            {
                request = new HttpRequestMessage(HttpMethod.Get,
                    string.Format("https://api.twitter.com/1.1/search/tweets.json?q={0}&count={1}&max_id={2}", key, count, last_id));
            }

            request.Headers.Add("Authorization", "Bearer " + accessToken);
            var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.SendAsync(request);
            var serializer = new JavaScriptSerializer();
            dynamic json = serializer.Deserialize<object>(await response.Content.ReadAsStringAsync());
            return json;
        }
    }    
}