using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FBDerp.Common.ViewHelpers;

namespace FBDerp.Common
{
    public class FacebookScript
    {
        public static IHtmlString LoadSDK()
        {
            return new HtmlString(@"
<div id='fb-root'></div>
<script type='text/javascript'>   
    window.fbAsyncInit = function() {
        FB.init({
            appId      : '" + Config.FacebookApplicationId + @"',
            status     : true, 
            cookie     : true,
            xfbml      : true,
            oauth      : true,
        });
    };
    (function(d){
        var js, id = 'facebook-jssdk'; if (d.getElementById(id)) {return;}
        js = d.createElement('script'); js.id = id; js.async = true;
        js.src = '//connect.facebook.net/en_US/all.js';
        d.getElementById('fb-root').appendChild(js);
    })(document);
</script>

");
        }
    }
}
