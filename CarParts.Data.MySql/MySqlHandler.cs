namespace CarParts.Data.MySql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Telerik.OpenAccess;

    public class MySqlHandler
    {
        public IEnumerable<PartReportInputModel> ReadReports()
        {
            var reports = new List<PartReportInputModel>();

            using (var db = new FluentModel())
            {
                reports = db.PartReportInputModels.ToList();
            }

            return reports;
        }

        public void WriteReports(IEnumerable<PartReportInputModel> reports)
        {
            using (var db = new FluentModel())
            {
                UpdateDatabase();

                db.Add(reports);

                db.SaveChanges();
            }
        }

        private static void UpdateDatabase()
        {
            using (var db = new FluentModel())
            {
            var schemaHandler = db.GetSchemaHandler();
            EnsureDB(schemaHandler);
            }
        }

        private static void EnsureDB(ISchemaHandler schemaHandler)
        {
            string script = null;
            if (schemaHandler.DatabaseExists())
            {
                script = schemaHandler.CreateUpdateDDLScript(null);
            }
            else
            {
                schemaHandler.CreateDatabase();
                script = schemaHandler.CreateDDLScript();
            }

            if (!string.IsNullOrEmpty(script))
            {
                schemaHandler.ExecuteDDLScript(script);
            }
        }
    }
}
