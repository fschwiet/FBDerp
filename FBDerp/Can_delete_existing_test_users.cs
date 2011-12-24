﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NJasmine;

namespace FBDerp
{
    public class Can_delete_existing_test_users : GivenWhenThenFixture
    {
        public override void Specify()
        {
            ignoreBecause("Facebook doesnt seem to allow deleting test users.");

            var facebookClient = arrange(() => new FacebookClient());

            given("we have a test user", delegate()
            {
                var user = arrange(() => facebookClient.CreateTestUser("Gonna Deleteme"));

                then("there is at least one test user reported by facebook", delegate()
                {
                    var numberOfUsers = facebookClient.GetNumberOfTestUsers();

                    expect(() => numberOfUsers >= 1);
                });

                when("we delete all test users", delegate()
                {
                    arrange(() => facebookClient.DeleteAllTestUsers());

                    then("there are no test users reported by facebook", delegate()
                    {
                        var numberOfUsers = facebookClient.GetNumberOfTestUsers();

                        expect(() => numberOfUsers == 0);
                    });
                });
            });
        }
    }
}
