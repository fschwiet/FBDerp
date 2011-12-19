using System;
using System.Net;
using NJasmine;
using Newtonsoft.Json;

namespace FBDerp
{
    public class Can_create_a_Facebook_user : GivenWhenThenFixture
    {
        public override void Specify()
        {
            string userFullname = "User Name";
            var applicationId = Properties.Settings.Default.FacebookApplicationId;
            var applicationSecret = Properties.Settings.Default.FacebookSecret;

            var accessToken = arrange(() => FacebookClient.GetAppToken(applicationId, applicationSecret));

            when("we create a facebook user", delegate()
            {
                var user = arrange(() =>
                {
                    return FacebookClient.CreateUser(accessToken, applicationId, userFullname);
                });

                then("facebook returns a user object", delegate()
                {
                    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(user, Formatting.Indented));
                });
            });
        }
    }
}
