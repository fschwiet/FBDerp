using System;
using System.Web;

namespace FBDerp.Common.ViewHelpers
{
    public class Config
    {
        public static AppSettingConfig Current;
        public string AppHarborCommit;
        public string FacebookApplicationId;
        public string FacebookApplicationSecret;
        public string BaseUrl;

        public string UrlFor(string path)
        {
            var uriString = BaseUrl;

            if (string.IsNullOrEmpty(uriString))
                uriString = "http://" + HttpContext.Current.Request.Url.Authority + "/";

            return new Uri(new Uri(uriString), path).AbsoluteUri;
        }
    }
}