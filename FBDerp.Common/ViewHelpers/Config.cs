using System;
using System.Web.Mvc;

namespace FBDerp.Common.ViewHelpers
{
    public static class Config
    {
        public static string FacebookApplicationId;
        public static string BaseUrl;
 
        public static void SetAllTheThings(string facebookApplicationId, string baseUrl)
        {
            FacebookApplicationId = facebookApplicationId;
            BaseUrl = baseUrl;
        }

        public static string UrlFor(string path)
        {
            return new Uri(new Uri(BaseUrl), path).AbsoluteUri;
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