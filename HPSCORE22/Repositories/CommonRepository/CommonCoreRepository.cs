using AlphaCare.Models;
using Oracle.ManagedDataAccess.Client;
using RetailCare.Common;
using RetailCare.Models;
using RetailCare.Models.CRMModels;
using System.Data;

namespace AlphaCare.Repositories.CommonRepository
{
    public interface ICommonCoreRepository
    {
        public List<COMPANYSMSDETAILS> GetCompanyWiseSMSDetails(int CompanyID);
    }
    public class CommonCoreRepository: ICommonCoreRepository
    {
        private readonly string _connectionString;

        public CommonCoreRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("hps");
        }
        public COMPANYSMSDETAILS GetCompanyWiseSMSDetails(int CompanyID)
        {
            DataTable dt = new DataTable();
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.SP_GETALLCORESMSDETAILS", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.BindByName = true;
                    command.Parameters.Add("P_COMPANYID", OracleDbType.Int32).Value = CompanyID;
                    command.Parameters.Add("P_RECORDSET", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return ExtractData.Convert<COMPANYSMSDETAILS>(dt).FirstOrDefault();
        }
    }
}
