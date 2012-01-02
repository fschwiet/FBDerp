using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FBDerp.Common.ViewHelpers;

namespace SimpleWebApplication.Controllers
{
    public class CheckController : Controller
    {
        //
        // GET: /Check/

        public ActionResult Index()
        {
            Membership.ValidateUser("hi", "there"); // force creation of database

            var appData = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_data");

            var result = DbController.RunScript("DECLARE @helloDatabase INT");

            try
            {
                System.IO.File.WriteAllText(Path.Combine(appData, "appHarborCommit.txt"), Config.Current.AppHarborCommit);
            }
            catch (Exception e)
            {
                result.Error = "Unable to write to App_Data, check permissions.\n\n" + result.Error;
            }

            result.Title = "Check";

            return View("DbScriptResult", result);
        }

    }
}
