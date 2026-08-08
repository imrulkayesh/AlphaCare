using Oracle.ManagedDataAccess.Client;

namespace QCMS.Services
{
    public class DatabaseService
    {
        private readonly IConfiguration _configuration;

        public DatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public OracleConnection GetConnection()
        {
            string connStr = _configuration.GetConnectionString("hps")
                ?? throw new InvalidOperationException("connection string missing");

            return new OracleConnection(connStr);
        }
        public OracleConnection GetHrsConnection()
        {
            string connStrHr = _configuration.GetConnectionString("hris")
                ?? throw new InvalidOperationException("HRIS connection string missing");

            return new OracleConnection(connStrHr);
        }
    }
}
