using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FBDerp.Common
{
    public static class ObjectExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> GetPropertyValues(this object input)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            foreach (var iframeProperty in input.GetType().GetProperties())
            {
                string name = iframeProperty.Name;
                string value = iframeProperty.GetGetMethod().Invoke(input, null).ToString();

                result.Add(new KeyValuePair<string, string>(name, value));
            }

            return result;
        }
    }
}
