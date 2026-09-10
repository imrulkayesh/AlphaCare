using AlphaCare.Interface.DashboardInterface;
using AlphaCare.Models.Dashboard_Model;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Oracle.ManagedDataAccess.Client;
using QCMS.Services;
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
        public async Task<List<Top10TechnicianModel>> GetTop10TechnicianSolvedATA(int companyId,string showroom)
        {
            var list = new List<Top10TechnicianModel>();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (OracleCommand command = new OracleCommand(
                    "SP_GET_TOP10_TECH_SOLVE",
                    connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(
                        "P_COMPANYID",
                        OracleDbType.Int32
                    ).Value = companyId;

                    command.Parameters.Add(
                        "P_SHOWROOM",
                        OracleDbType.Varchar2
                    ).Value = showroom;

                    OracleParameter resultParameter = command.Parameters.Add(
                        "P_RESULT",
                        OracleDbType.RefCursor
                    );

                    resultParameter.Direction = ParameterDirection.Output;

                    using (OracleDataReader reader =
                           await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var model = new Top10TechnicianModel();

                            model.TECHNICIANNAME =
                                reader["TECHNICIANNAME"] == DBNull.Value
                                    ? null
                                    : reader["TECHNICIANNAME"].ToString();

                            model.STAFFID =
                                reader["STAFFID"] == DBNull.Value
                                    ? null
                                    : reader["STAFFID"].ToString();

                            model.CURRENT_MONTH_SOLVE =
                                reader["CURRENT_MONTH_SOLVE"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(
                                        Convert.ToDecimal(
                                            reader["CURRENT_MONTH_SOLVE"]));

                            model.PREVIOUS_MONTH_SOLVE =
                                reader["PREVIOUS_MONTH_SOLVE"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(
                                        Convert.ToDecimal(
                                            reader["PREVIOUS_MONTH_SOLVE"]));

                            model.SOLVE_GROWTH =
                                reader["SOLVE_GROWTH"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(
                                        Convert.ToDecimal(
                                            reader["SOLVE_GROWTH"]));

                            model.SOLVE_GROWTH_PERCENTAGE =
                                reader["SOLVE_GROWTH_PERCENTAGE"] == DBNull.Value
                                    ? 0
                                    : Convert.ToDecimal(
                                        reader["SOLVE_GROWTH_PERCENTAGE"]);

                            list.Add(model);
                        }
                    }
                }
            }

            return list;
        }
        public async Task<List<ChartModels>> GetTop5ProblemType(int companyId, string showroom)
        {
            var list = new List<ChartModels>();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (OracleCommand command = new OracleCommand("SP_GET_TOP5_PROBLEM_TYPE", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("P_COMPANYID",OracleDbType.Int32).Value = companyId;
                    command.Parameters.Add("P_SHOWROOM",OracleDbType.Varchar2).Value = showroom;
                    OracleParameter resultParameter = command.Parameters.Add("P_RESULT", OracleDbType.RefCursor);
                    resultParameter.Direction = ParameterDirection.Output;
                    using (OracleDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var model = new ChartModels();
                            model.CurrentMonthLabel = reader["PRODUCTNAME"] == DBNull.Value ? null : reader["PRODUCTNAME"].ToString();
                            model.CurrentMonthValue = reader["CURRENT_MONTH_TOTAL"] == DBNull.Value ? 0 :Convert.ToInt32(reader["CURRENT_MONTH_TOTAL"]);
                            model.PreviousMonthLabel = reader["PRODUCTNAME"] == DBNull.Value ? null : reader["PRODUCTNAME"].ToString();
                            model.PreviousMonthValue =reader["PREVIOUS_MONTH_TOTAL"] == DBNull.Value ? 0: Convert.ToInt32(Convert.ToDecimal(reader["PREVIOUS_MONTH_TOTAL"]));
                            list.Add(model);
                        }
                    }
                }
            }
            return list;
        }
        public async Task<List<Top10TechnicianModel>> GetLowest10TechnicianSolvedATA(int companyId, string showroom)
        {
            var list = new List<Top10TechnicianModel>();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (OracleCommand command = new OracleCommand(
                    "SP_GET_Lowest10_TECH_SOLVE",
                    connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(
                        "P_COMPANYID",
                        OracleDbType.Int32
                    ).Value = companyId;

                    command.Parameters.Add(
                        "P_SHOWROOM",
                        OracleDbType.Varchar2
                    ).Value = showroom;

                    OracleParameter resultParameter = command.Parameters.Add(
                        "P_RESULT",
                        OracleDbType.RefCursor
                    );

                    resultParameter.Direction = ParameterDirection.Output;

                    using (OracleDataReader reader =
                           await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var model = new Top10TechnicianModel();

                            model.TECHNICIANNAME =
                                reader["TECHNICIANNAME"] == DBNull.Value
                                    ? null
                                    : reader["TECHNICIANNAME"].ToString();

                            model.STAFFID =
                                reader["STAFFID"] == DBNull.Value
                                    ? null
                                    : reader["STAFFID"].ToString();

                            model.CURRENT_MONTH_SOLVE =
                                reader["CURRENT_MONTH_SOLVE"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(
                                        Convert.ToDecimal(
                                            reader["CURRENT_MONTH_SOLVE"]));

                            model.PREVIOUS_MONTH_SOLVE =
                                reader["PREVIOUS_MONTH_SOLVE"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(
                                        Convert.ToDecimal(
                                            reader["PREVIOUS_MONTH_SOLVE"]));

                            model.SOLVE_GROWTH =
                                reader["SOLVE_GROWTH"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(
                                        Convert.ToDecimal(
                                            reader["SOLVE_GROWTH"]));

                            model.SOLVE_GROWTH_PERCENTAGE =
                                reader["SOLVE_GROWTH_PERCENTAGE"] == DBNull.Value
                                    ? 0
                                    : Convert.ToDecimal(
                                        reader["SOLVE_GROWTH_PERCENTAGE"]);

                            list.Add(model);
                        }
                    }
                }
            }

            return list;
        }
        public PieChartModel GetCurrentDayTicketStatus(int companyId, string ShowrromCode)
        {
            PieChartModel model = new PieChartModel();

            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(
                    "SP_GET_TODAY_TICKET_STATUS", con))
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
                            model.TotalSolve =
                                Convert.ToInt32(reader["SOLVE"]);

                            model.TotalUnsolved =
                                Convert.ToInt32(reader["UNSOLVE"]);

                            model.ToalCancelled =
                                Convert.ToInt32(reader["CANCEL"]);
                        }
                    }
                }
            }
            return model;
        }
        public async Task<List<Top10TechnicianModel>> GetTop5Showroom(int companyId)
        {
            var list = new List<Top10TechnicianModel>();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (OracleCommand command = new OracleCommand(
                    "SP_GET_TOP5_CUSTOMER_COMPLAIN",
                    connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(
                        "P_COMPANYID",
                        OracleDbType.Int32
                    ).Value = companyId;

                    OracleParameter resultParameter = command.Parameters.Add(
                        "P_RESULT",
                        OracleDbType.RefCursor
                    );

                    resultParameter.Direction = ParameterDirection.Output;

                    using (OracleDataReader reader =
                           await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var model = new Top10TechnicianModel();

                            model.TECHNICIANNAME =
                                reader["SHOWROOM"] == DBNull.Value
                                    ? null
                                    : reader["SHOWROOM"].ToString();
                            model.STAFFID =
                                    reader["CUSTOMERNAME"] == DBNull.Value
                                        ? null
                                        : reader["CUSTOMERNAME"].ToString();
                            model.CURRENT_MONTH_SOLVE =
                                reader["COMPLAINNO"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(
                                        Convert.ToDecimal(
                                            reader["COMPLAINNO"]));
                            list.Add(model);
                        }
                    }
                }
            }

            return list;
        }
        public async Task<List<TimeTrandModel>> GetCurrentMonthTimeTrand(int companyId)
        {
            var list = new List<TimeTrandModel>();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (OracleCommand command = new OracleCommand(
                    "SP_GET_30DAYS_TICKET_REPORT",
                    connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(
                        "P_COMPANYID",
                        OracleDbType.Int32
                    ).Value = companyId;

                    OracleParameter resultParameter = command.Parameters.Add(
                        "P_RESULT",
                        OracleDbType.RefCursor
                    );

                    resultParameter.Direction = ParameterDirection.Output;

                    using (OracleDataReader reader =
                           await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var model = new TimeTrandModel();

                            model.ReportedDate =
                                reader["REPORTDATE"] == DBNull.Value
                                    ? null
                                    : Convert.ToDateTime(reader["REPORTDATE"])
                                        .ToString("yyyy-MM-dd");

                            model.TotalTickets =
                                reader["TOTALTICKETS"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["TOTALTICKETS"]);

                            model.SolvedTickets =
                                reader["SOLVEDTICKETS"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["SOLVEDTICKETS"]);

                            list.Add(model);
                        }
                    }
                }
            }

            return list;
        }
    }
}
