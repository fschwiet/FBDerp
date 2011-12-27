using System;
using System.Web;
using System.Web.Mvc;

namespace FBDerp.Common.ViewHelpers
{
    public static class Config
    {
        public static string AppHarborCommit;
        public static string FacebookApplicationId;
        public static string FacebookApplicationSecret;
        public static string BaseUrl;
 
        public static void SetAllTheThings(string appHarborCommit, string facebookApplicationId, string facebookApplicationSecret, string baseUrl)
        {
            AppHarborCommit = appHarborCommit;
            FacebookApplicationId = facebookApplicationId;
            FacebookApplicationSecret = facebookApplicationSecret;
            BaseUrl = baseUrl;
        }

        public static string UrlFor(string path)
        {
            var uriString = BaseUrl;

            if (string.IsNullOrEmpty(uriString))
                uriString = "http://" + HttpContext.Current.Request.Url.Authority + "/";

            return new Uri(new Uri(uriString), path).AbsoluteUri;
        }

        public static string Querystring(this WebViewPage page, params string[] parameters)
        {
            var querystring = new QuerystringParameters();

            for(var i = 0; i < parameters.Length; i += 2)
            {
                querystring[parameters[i]] = parameters[i+1];
            }

            return querystring.AsQuerystring();
        }
    }
}