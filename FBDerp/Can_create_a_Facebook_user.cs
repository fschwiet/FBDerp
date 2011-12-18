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
            string userFullname = "Some User";// +Guid.NewGuid();
            var applicationId = 220266841382535;
            var applicationSecret = "e9eae045396bbb09fd170496a4a21cdf";

            var accessToken = arrange(() => FacebookClient.GetAppToken(applicationId.ToString(), applicationSecret));

            var httpClient = arrange(() => new WebClient());

            when("we create a facebook user", delegate()
            {
                var user = arrange(() =>
                {
                    return FacebookClient.CreateUser(accessToken, httpClient, applicationId, userFullname);
                });

                then("facebook returns a user object", delegate()
                {
                    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(user, Formatting.Indented));
                });
            });
        }
    }
}
