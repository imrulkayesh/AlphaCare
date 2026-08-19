using Oracle.ManagedDataAccess.Client;
using RetailCare.Common;
using RetailCare.Interface;
using RetailCare.Models;
using RetailCare.Models.CRMModels;
using System.ComponentModel.Design;
using System.Data;

namespace RetailCare.Repositories
{
    public class ReportGenerationRepository: IReportGenerationRepository
    {
        private readonly string _connectionString;

        public ReportGenerationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("hps");
        }
        public List<CompalinModel> GetComplainReport(FilteringOption FilteringValues,int CompanyID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.SP_GET_ALL_COMPLAINSDATERANGE", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.BindByName = true;
                    command.Parameters.Add("P_COMPANYID", OracleDbType.Int32).Value = CompanyID;
                    command.Parameters.Add("P_STATUSID", OracleDbType.Int32).Value = FilteringValues.StatusID;
                    command.Parameters.Add("P_STARTDATE", OracleDbType.Date).Value = FilteringValues.StartDate;
                    command.Parameters.Add("P_ENDDATE", OracleDbType.Date).Value = FilteringValues.EndDate;
                    command.Parameters.Add("P_RECORDSET", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return ExtractData.Convert<CompalinModel>(dt).ToList();
        }
        public List<FeedBackReportModel> GetFeedBackReport(FilteringOption FilteringValues, int CompanyID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.SP_GET_FEEDBACK_REPORT", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.BindByName = true;
                    command.Parameters.Add("P_COMPANYID", OracleDbType.Int32).Value = CompanyID;
                    command.Parameters.Add("P_STARTDATE", OracleDbType.Date).Value = FilteringValues.StartDate;
                    command.Parameters.Add("P_ENDDATE", OracleDbType.Date).Value = FilteringValues.EndDate;
                    command.Parameters.Add("P_RECORDSET", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return ExtractData.Convert<FeedBackReportModel>(dt).ToList();
        }
    }
}
