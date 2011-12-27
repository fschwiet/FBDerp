using System.Collections.Generic;

namespace SimpleWebApplication.Models
{
    public class DbResultModel
    {
        public string DatabaseName;
        public IEnumerable<string> Script;
        public string Error;
    }
}