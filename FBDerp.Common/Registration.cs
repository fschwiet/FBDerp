using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using FBDerp.Common.ViewHelpers;

namespace FBDerp.Common
{
    public class Registration
    {
        /// <summary>
        /// Wraps facebook registration script
        /// http://developers.facebook.com/docs/plugins/registration/
        /// </summary>
        /// <param name="onFinishUrl"></param>
        /// <param name="registrationFields">should serialize like the fields parameter passed to facebook's registration url.</param>
        /// <returns></returns>
        public static IHtmlString ShowIframe(
            AppSettingConfig config,
            string onFinishUrl, 
            object registrationFields,
            object iframeAttributes)
        {
            var scriptUrlQuerystring = new QuerystringParameters();
            scriptUrlQuerystring.Add("client_id", config.FacebookApplicationId);
            scriptUrlQuerystring.Add("redirect_uri", onFinishUrl);
            scriptUrlQuerystring.Add("fields", Newtonsoft.Json.JsonConvert.SerializeObject(registrationFields));

            var iframe = new XElement("iframe");
            iframe.SetAttributeValue("src", "https://www.facebook.com/plugins/registration.php?" + scriptUrlQuerystring.AsQuerystring());

            foreach (var iframeProperty in iframeAttributes.GetPropertyValues())
            {
                iframe.SetAttributeValue(iframeProperty.Key, iframeProperty.Value);
            }

            return new HtmlString(iframe.ToString());
        }
    }
}
