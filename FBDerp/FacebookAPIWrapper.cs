using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FBDerp.Common;
using Newtonsoft.Json;

namespace FBDerp
{
    public class FacebookAPIWrapper
    {
        public static string GetAppToken(string applicationId, string applicationSecret)
        {
            using(var client = new WebClient())
            {
                QuerystringParameters parameters = new QuerystringParameters();

                parameters.Add("client_id", applicationId);
                parameters.Add("client_secret", applicationSecret);
                parameters.Add("grant_type", "client_credentials");

                var uri = "https://graph.facebook.com/oauth/access_token?" + parameters.AsQuerystring();

                var result = client.DownloadString(uri);

                string prefix = "access_token=";
                if (!result.StartsWith(prefix))
                    throw new Exception("Unexpected response creating access token - expected prefix 'access_token=', actually started with " + result.Take(20));

                return result.Substring(prefix.Length);
            }
        }

        public class APITestUser
        {
            public string id { get; set; }
            public string access_token { get; set; }
            public string login_url { get; set; }
            public string email { get; set; }
            public string password { get; set; }
        }

        public static APITestUser CreateUser(string accessToken, string applicationId, string userFullname)
        {
            using (var webClient = new WebClient())
            {
                var createUserParameters = new QuerystringParameters();

                createUserParameters.Add("installed", "true");
                createUserParameters.Add("name", userFullname);
                createUserParameters.Add("permissions", "read_stream");
                createUserParameters.Add("method", "post");
                createUserParameters.Add("access_token", accessToken);

                string createUserUri = "https://graph.facebook.com/" + applicationId + "/accounts/test-users?" +
                                       createUserParameters.AsQuerystring();
                var userCreationResponse = webClient.DownloadString(new Uri(createUserUri));

                return JsonConvert.DeserializeObject<APITestUser>(userCreationResponse);
            }
        }
    }
}
