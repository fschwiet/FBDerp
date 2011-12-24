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

        public int GetNumberOfTestUsers()
        {
            return FacebookAPIWrapper.GetSomeExistingTestUsers(_accessToken, _applicationId).Count();
        }

        public void DeleteAllTestUsers()
        {
            throw new NotImplementedException("it seems facebook will keep reporting deleted users.");

            var remainingUsers = FacebookAPIWrapper.GetSomeExistingTestUsers(_accessToken, _applicationId);

            if (remainingUsers.Count() == 0)
                return;

            do
            {
                Console.WriteLine("Deleting {0} users, first is {1}...", remainingUsers.Count(), remainingUsers.First().id);
                
                foreach(var user in remainingUsers)
                {
                    FacebookAPIWrapper.DeleteUser(_accessToken, user.id);
                }

                remainingUsers = FacebookAPIWrapper.GetSomeExistingTestUsers(_accessToken, _applicationId);
            } while (remainingUsers.Count() > 0);
        }
    }
}
