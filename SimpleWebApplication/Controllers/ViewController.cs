using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using SimpleWebApplication.Models;

namespace SimpleWebApplication.Controllers
{
    public class ViewController : Controller
    {
        public ActionResult Index(int id)
        {
            var url = this.HttpContext.Request.Url.AbsoluteUri;

            var querystringIndex = url.IndexOf("?");
            if (querystringIndex > 0)
                url = url.Substring(0, querystringIndex);

            return View("Index", new ViewModel()
            {
                Id = id,
                Url = url
            });
        }
    }
}
