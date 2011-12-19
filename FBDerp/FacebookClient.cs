using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FBDerp.Properties;

namespace FBDerp
{
    public class FacebookClient
    {
        private string _applicationId;
        private string _accessToken;

        public static FacebookClient Open()
        {
            var applicationId = Settings.Default.FacebookApplicationId;
            var applicationSecret = Settings.Default.FacebookSecret;

            return new FacebookClient()
            {
                _applicationId = applicationId,
                _accessToken = FacebookAPIWrapper.GetAppToken(applicationId, applicationSecret)
            };
        }

        public FacebookAPIWrapper.APITestUser ApiTestUser(string userFullname)
        {
            return FacebookAPIWrapper.CreateUser(_accessToken, _applicationId, userFullname);
        }
    }
}
