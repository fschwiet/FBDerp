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

        public FacebookClient()
        {
            _applicationId = Settings.Default.FacebookApplicationId;
            _accessToken = FacebookAPIWrapper.GetAppToken(_applicationId, Settings.Default.FacebookSecret);
        }

        public FacebookAPIWrapper.APITestUser CreateTestUser(string userFullname)
        {
            return FacebookAPIWrapper.CreateUser(_accessToken, _applicationId, userFullname);
        }
    }
}
