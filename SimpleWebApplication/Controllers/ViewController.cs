using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using FBDerp.Common.ViewHelpers;
using SimpleWebApplication.Models;

namespace SimpleWebApplication.Controllers
{
    public class ViewController : Controller
    {
        public ActionResult Index(int id)
        {
            var path = "/view/" + id;

            return View("Index", new ViewModel()
            {
                Id = id,
                Url = AppSettingConfig.Current.UrlFor(path)
            });
        }
    }
}
