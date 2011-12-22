using System;
using System.Web;
using System.Web.Mvc;
using FBDerp.Common;

namespace SimpleWebApplication.ViewHelpers
{
    public static class Globals
    {
        public static string FacebookApplicationId
        {
            get { return Properties.Settings.Default.FacebookApplicationId; }
        }

        public static string UrlFor(this WebViewPage page, string path)
        {
            return new Uri(page.Context.Request.Url, path).AbsoluteUri;
        }

        public static string Querystring(this WebViewPage page, params string[] parameters)
        {
            var querystring = new QuerystringParameters();

            for(var i = 0; i < parameters.Length; i += 2)
            {
                querystring[parameters[i]] = parameters[i+1];
            }

            return querystring.AsQuerystring();
        }
    }
}