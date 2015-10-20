namespace CarParts.Data.MySql
{
    using System.Linq;

    using Telerik.OpenAccess;
    using Telerik.OpenAccess.Metadata;

    public partial class FluentModel : OpenAccessContext
    {
        private static string connectionStringName = @"connectionId";

        private static BackendConfiguration backend =
            GetBackendConfiguration();

        private static MetadataSource metadataSource =
            new FluentModelMetadataSource();

        public FluentModel()
            : base(connectionStringName, backend, metadataSource)
        { 
        }

        public IQueryable<PartReportInputModel> PartReportInputModels
        {
            get
            {
                return this.GetAll<PartReportInputModel>();
            }
        }

        public static BackendConfiguration GetBackendConfiguration()
        {
            BackendConfiguration backend = new BackendConfiguration();
            backend.Backend = "MySql";
            backend.ProviderName = "MySql.Data.MySqlClient";

            return backend;
        }
    }
}
