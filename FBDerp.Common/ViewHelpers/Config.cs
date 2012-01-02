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
    }
}