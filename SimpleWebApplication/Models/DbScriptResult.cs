using System.Collections.Generic;

namespace SimpleWebApplication.Models
{
    public class DbScriptResult
    {
        public string Title;
        public string DatabaseName;
        public IEnumerable<string> Script;
        public string Error;
    }
}