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
        //
        // GET: /Db/

        public ActionResult Create()
        {
            using(var reader = new StreamReader(GetType().Assembly.GetManifestResourceStream("SimpleWebApplication.schema.sql")))
            {
                var script = reader.ReadToEnd();

                var connectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

                var databaseName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;

                var scriptBlocks = Regex.Split(script.Replace("USE [aspnetdb]", "USE [" + databaseName + "]"), "GO\r\n");

                if (!string.IsNullOrEmpty(databaseName))
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        
                        foreach(var block in scriptBlocks)
                        {
                            using (var command = new SqlCommand(block))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }

                return View("Create", new DbResultModel()
                {
                    DatabaseName = databaseName,
                    Script = scriptBlocks
                });
            }
        }
    }
}
