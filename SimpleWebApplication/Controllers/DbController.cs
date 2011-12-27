using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
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

                script = script.Replace("USE [aspnetdb]", "USE [" + databaseName + "]");

                if (!string.IsNullOrEmpty(databaseName))
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (var command = new SqlCommand(script))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }

                return View("Create", new DbResultModel()
                {
                    DatabaseName = databaseName,
                    Script = script
                });
            }
        }
    }
}
