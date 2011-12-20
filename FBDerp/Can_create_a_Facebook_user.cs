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
            var facebookClient = arrange(() => FacebookClient.Open());
            string userFullname = "User Name";
            
            when("we create a facebook user", delegate()
            {
                var user = arrange(() =>
                {
                    return facebookClient.ApiTestUser(userFullname);
                });

                then("facebook returns a user object", delegate()
                {
                    expect(() => !string.IsNullOrEmpty(user.email));
                    expect(() => !string.IsNullOrEmpty(user.password));
                    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(user, Formatting.Indented));
                });
            });
        }
    }
}
