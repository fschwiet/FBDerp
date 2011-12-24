﻿using System;
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
                var parameters = new QuerystringParameters();

                parameters.Add("installed", "true");
                parameters.Add("name", userFullname);
                parameters.Add("permissions", "read_stream");
                parameters.Add("method", "post");
                parameters.Add("access_token", accessToken);

                string url = "https://graph.facebook.com/" + applicationId + "/accounts/test-users?" +
                                       parameters.AsQuerystring();
                
                var response = webClient.DownloadString(new Uri(url));

                return JsonConvert.DeserializeObject<APITestUser>(response);
            }
        }

        public class APIUserList
        {
            public APITestUser[] data;
        }

        /// <summary>
        /// I'm not bothering with paging yet, so this only gets some of your test users.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public static IEnumerable<APITestUser> GetSomeExistingTestUsers(string accessToken, string applicationId)
        {
            using (var webClient = new WebClient())
            {
                var parameters = new QuerystringParameters();

                parameters.Add("access_token", accessToken);

                string url = "https://graph.facebook.com/" + applicationId + "/accounts/test-users?" +
                                       parameters.AsQuerystring();

                var response = webClient.DownloadString(new Uri(url));

                Console.WriteLine(response);
                return JsonConvert.DeserializeObject<APIUserList>(response).data;
            }
        }

        public static void DeleteUser(string accessToken, string userId)
        {
            using (var webClient = new WebClient())
            {
                var parameters = new QuerystringParameters();

                parameters.Add("access_token", accessToken);

                string createUserUri = "https://graph.facebook.com/" + userId +"?" +
                                       parameters.AsQuerystring();

                var response = webClient.UploadString(new Uri(createUserUri), "DELETE");

                if (response != "true")
                    throw new Exception("Unable to delete user " + userId + ", response was: " + response);
            }
        }
    }
}
