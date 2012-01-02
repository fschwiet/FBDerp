using System.Configuration;
using System.Web.Mvc;

namespace FBDerp.Common.ViewHelpers
{
    public class AppSettingConfig : Config
    {
        public AppSettingConfig(string appHarborCommit, string facebookApplicationId, string facebookApplicationSecret, string baseUrl)

        {
            AppHarborCommit = appHarborCommit;
            FacebookApplicationId = facebookApplicationId;
            FacebookApplicationSecret = facebookApplicationSecret;
            BaseUrl = baseUrl;
        }
    }
}