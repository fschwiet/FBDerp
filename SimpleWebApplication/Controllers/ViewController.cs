using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleWebApplication.Models;

namespace SimpleWebApplication.Controllers
{
    public class ViewController : Controller
    {
        public ActionResult Index(int id)
        {
            return View("Index", new ViewModel() { Id = id});
        }
    }
}
