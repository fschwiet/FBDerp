using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
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

                var connectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

                var databaseName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;

                var scriptBlocks = Regex.Split(script.Replace("USE [aspnetdb]", "USE [" + databaseName + "]"), "GO\r\n");

                string error = "";

                //if (!string.IsNullOrEmpty(databaseName))
                {
                    try
                    {
                        RunSqlBlocks(connectionString, scriptBlocks);
                    }
                    catch (Exception e)
                    {
                        error = e.ToString();
                    }
                }

                var dbResultModel = new DbResultModel()
                {
                    DatabaseName = databaseName,
                    Script = scriptBlocks,
                    Error = error
                };
                return View("Create", dbResultModel);
            }
        }

        private void RunSqlBlocks(string connectionString, string[] scriptBlocks)
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
