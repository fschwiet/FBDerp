using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBDerp
{
    public class QuerystringParameters : Dictionary<string,string>
    {
        public string AsQuerystring()
        {
            StringBuilder result = new StringBuilder();
            string separator = "";

            foreach(var kvp in this.AsEnumerable())
            {
                result.Append(separator);
                separator = "&";

                result.Append(Uri.EscapeDataString(kvp.Key));
                result.Append("=");
                //result.Append(kvp.Value);
                result.Append(Uri.EscapeDataString(kvp.Value));
            };

            return result.ToString();
        }
    }
}