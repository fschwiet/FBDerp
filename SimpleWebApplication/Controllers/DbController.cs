using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using FBDerp;
using SimpleWebApplication.Models;

namespace SimpleWebApplication.Controllers
{
    public class DbController : Controller
    {
        public ActionResult Create()
        {
            using(var reader = new StreamReader(GetType().Assembly.GetManifestResourceStream("SimpleWebApplication.schema.sql")))
            {
                var script = reader.ReadToEnd();

                var result = RunScript(script);

                result.Title = "Create";

                return View("DbScriptResult", result);
            }
        }

        public static DbScriptResult RunScript(string script)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

            var databaseName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;

            var scriptBlocks = Regex.Split(script.Replace("aspnetdb", databaseName), "GO\r\n");

            string error = "";

            try
            {
                RunSqlBlocks(connectionString, scriptBlocks);
            }
            catch (Exception e)
            {
                error = e.ToString();
            }

            var dbResultModel = new DbScriptResult()
            {
                DatabaseName = databaseName,
                Script = scriptBlocks,
                Error = error
            };
            return dbResultModel;
        }

        static void RunSqlBlocks(string connectionString, string[] scriptBlocks)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                        
                foreach(var block in scriptBlocks)
                {
                    try
                    {
                        using (var command = new SqlCommand(block, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error running script: " + block, e);
                    }
                }
            }
        }
    }
}
