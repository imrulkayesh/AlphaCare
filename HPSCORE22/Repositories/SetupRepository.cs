using AlphaCare.Interface;
using Oracle.ManagedDataAccess.Client;
using RetailCare.Common;
using RetailCare.Models;
using System.Data;

namespace AlphaCare.Repositories
{
    public class SMSSendingModel
    {
        public string? SMSPASSWORD { get; set; }
        public string? SMSUSERID { get; set; }
        public string? SMSMASKING { get; set; }
        public string? SMSMD5HASHPASSWORD { get; set; }
        public int SMSCOMPANYID { get; set; }
    }
    public class SetupRepository: ISetupRepository
    {
        private readonly string _connectionString;

        public SetupRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("hps");
        }

        public SMSSendingModel GetCompanySMSApiDetals(int CompanyID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("SP_GET_COMPANY_SMS_DETAILS", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.Add("P_SMSCOMPANYID", OracleDbType.Int32).Value = CompanyID;

                    command.Parameters.Add("P_RESULT", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<SMSSendingModel>(dt).FirstOrDefault();
        }
    }
}
