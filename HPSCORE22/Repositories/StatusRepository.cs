using Oracle.ManagedDataAccess.Client;
using RetailCare.Common;
using RetailCare.Interface.CRMInterface;
using RetailCare.Models;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace RetailCare.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly string _connectionString;

        public StatusRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("hps");
        }

        public List<StatusModel> GetAllStatus(int CompanyID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("GetAllStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    //command.Parameters.Add("p_CompanyID", OracleDbType.Int32).Value = CompanyID;

                    command.Parameters.Add("p_Result", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<StatusModel>(dt).ToList();
        }
    }
}