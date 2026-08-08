using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using RetailCare.Common;
using RetailCare.Interface;
using RetailCare.Models;
using System.Data;

namespace RetailCare.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly string _connectionString;

        public CompanyRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("hps");
        }

        public CompanyModel GetSingleCompanyDetails(int CompanyID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("GetAllCompany", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.Add("p_CompanyID", OracleDbType.Int32).Value = CompanyID;

                    command.Parameters.Add("p_Recordset", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<CompanyModel>(dt).FirstOrDefault();
        }
    }
}