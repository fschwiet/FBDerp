using System;
using System.Net;
using NJasmine;

namespace FBDerp
{
    public class Has_facebook_accounts : GivenWhenThenFixture
    {
        public override void Specify()
        {
            string fullName = "Some User";// +Guid.NewGuid();
            var applicationId = 220266841382535;
            var applicationSecret = "e9eae045396bbb09fd170496a4a21cdf";

            var accessToken = arrange(() => FacebookClient.GetAppToken(applicationId.ToString(), applicationSecret));
            arrange(() => Console.WriteLine(accessToken));

            var httpClient = arrange(() => new WebClient());

            given("a facebook user", delegate()
            {
                arrange(() =>
                {
                    var createUserParameters = new QuerystringParameters();

                    createUserParameters.Add("installed", "true");
                    createUserParameters.Add("name", fullName);
                    createUserParameters.Add("permissions", "read_stream");
                    createUserParameters.Add("method", "post");
                    createUserParameters.Add("access_token", accessToken);

                    string createUserUri = "https://graph.facebook.com/" + applicationId + "/accounts/test-users?" + createUserParameters.AsQuerystring();
                    Console.WriteLine(createUserUri);
                    var userCreationResponse = httpClient.DownloadString(new Uri(createUserUri));

                    Console.WriteLine(userCreationResponse);
                });

                it("does something", delegate()
                {
                        
                });
            });
        }
    }
}
