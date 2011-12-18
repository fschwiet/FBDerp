using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace FBDerp
{
    public class FacebookClient
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
    }
}
