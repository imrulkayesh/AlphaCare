using AlphaCare.Interface.DashboardInterface;
using AlphaCare.Models.Dashboard_Model;
using Oracle.ManagedDataAccess.Client;
using RetailCare.Common;
using RetailCare.Models;
using RetailCare.Models.CRMModels;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AlphaCare.Repositories.UserDashboard
{
    public class UserDashboardRepository: IUserDashboardRepository
    {
        private readonly string _connectionString;

        public UserDashboardRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("hps");
        }
        public SummaryCardModel GetComplaintDashboard(int companyId,string ShowrromCode)
        {
            SummaryCardModel model = new SummaryCardModel();

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(
                    "SP_GET_COMPLAIN_DASHBOARD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // COMPANY ID
                    cmd.Parameters.Add("P_COMPANYID", OracleDbType.Int32)
                        .Value = companyId;
                    cmd.Parameters.Add("P_SHOWROOM", OracleDbType.Varchar2).Value = ShowrromCode;
                    // REF CURSOR
                    cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor)
                        .Direction = ParameterDirection.Output;

                    con.Open();

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model.CurrentMonthCount =
                                Convert.ToInt32(reader["CURRENT_MONTH_COUNT"]);

                            model.TodayCount =
                                Convert.ToInt32(reader["TODAY_COUNT"]);

                            model.PendingFeedbackCount =
                                Convert.ToInt32(reader["PENDING_FEEDBACK_COUNT"]);

                            model.OldPendingCount =
                                Convert.ToInt32(reader["OLD_PENDING_COUNT"]);
                        }
                    }
                }
            }
            return model;
        }
        public List<CompalinModel> GetPendingComplain(int CompanyID, string ShowrromCode)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.SP_GET_CURRENT_MONTH_TICKETS", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.BindByName = true;
                    command.Parameters.Add("P_COMPANYID", OracleDbType.Int32).Value = CompanyID;
                    command.Parameters.Add("P_SHOWROOM", OracleDbType.Varchar2).Value = ShowrromCode;
                    command.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return ExtractData.Convert<CompalinModel>(dt).ToList();
        }
        public List<CompalinModel> GetDueComplain(int CompanyID, string ShowrromCode)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.SP_GET_CURRENT_MONTH_TICKETS_DUE", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.BindByName = true;
                    command.Parameters.Add("P_COMPANYID", OracleDbType.Int32).Value = CompanyID;
                    command.Parameters.Add("P_SHOWROOM", OracleDbType.Varchar2).Value = ShowrromCode;
                    command.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return ExtractData.Convert<CompalinModel>(dt).ToList();
        }
        public DueSummaryCardModel GetDueComplaintDashboard(int companyId,string ShowrromCode)
        {
            DueSummaryCardModel model = new DueSummaryCardModel();

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(
                    "SP_GET_DASHBOARD_SUMMARY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // COMPANY ID
                    cmd.Parameters.Add("P_COMPANYID", OracleDbType.Int32)
                        .Value = companyId;
                    cmd.Parameters.Add("P_SHOWROOM", OracleDbType.Varchar2).Value = ShowrromCode;
                    
                    // REF CURSOR
                    cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor)
                        .Direction = ParameterDirection.Output;

                    con.Open();

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model.THIS_MONTH_SOLVED_PERCENTAGE =
                                Convert.ToInt32(reader["THIS_MONTH_SOLVED_PERCENTAGE"]);

                            model.LAST_MONTH_SOLVED_PERCENTAGE =
                                Convert.ToInt32(reader["LAST_MONTH_SOLVED_PERCENTAGE"]);

                            model.SOLVE_GROWTH_PERCENTAGE =
                                Convert.ToInt32(reader["SOLVE_GROWTH_PERCENTAGE"]);

                            model.ACTIVE_TECHNICIAN_COUNT =
                                Convert.ToInt32(reader["ACTIVE_TECHNICIAN_COUNT"]);
                        }
                    }
                }
            }
            return model;
        }
    }
}
