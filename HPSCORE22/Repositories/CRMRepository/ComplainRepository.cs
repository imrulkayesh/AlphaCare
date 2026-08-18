using Oracle.ManagedDataAccess.Client;
using RetailCare.Common;
using RetailCare.Interface.CRMInterface;
using RetailCare.Models;
using RetailCare.Models.CRMModels;
using System.Data;

namespace RetailCare.Repositories.CRMRepository
{
    public class ComplainRepository: IComplainRepository
    {
        private readonly string _connectionString;

        public ComplainRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("hps");
        }
        public bool AddNewComplain(CompalinModel complain)
        {
            bool isSuccess = true;
            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    connection.Open();

                    using (OracleCommand command = new OracleCommand("ESERV.SP_INSERT_COMPLAIN", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.BindByName = true;

                        command.Parameters.Add("P_TICKETCODE", OracleDbType.Varchar2).Value = (object?)complain.TICKETCODE ?? DBNull.Value;
                        command.Parameters.Add("P_CUSTOMERNAME", OracleDbType.Varchar2).Value = (object?)complain.CUSTOMERNAME ?? DBNull.Value;
                        command.Parameters.Add("P_CONTACTNO", OracleDbType.Varchar2).Value = (object?)complain.CONTACTNO ?? DBNull.Value;
                        command.Parameters.Add("P_LOCATION", OracleDbType.Varchar2).Value = (object?)complain.LOCATION ?? DBNull.Value;

                        command.Parameters.Add("P_STATUSID", OracleDbType.Int32).Value = complain.STATUSID;

                        command.Parameters.Add("P_COMPANYID", OracleDbType.Int32).Value = complain.COMPANYID;

                        command.Parameters.Add("P_COMPLAINDATE", OracleDbType.Date).Value =
                            complain.COMPLAINDATE == null ? DBNull.Value : complain.COMPLAINDATE;

                        command.Parameters.Add("P_TECHNICIANID", OracleDbType.Int32).Value =
                            complain.TECHNICIANID == null ? DBNull.Value : complain.TECHNICIANID;

                        command.Parameters.Add("P_PROBLEMTYPEID", OracleDbType.Int32).Value =
                            complain.PROBLEMTYPEID == null ? DBNull.Value : complain.PROBLEMTYPEID;

                        command.Parameters.Add("P_ISSENDFEEDBACK", OracleDbType.Int32).Value =
                            complain.ISSENDFEEDBACK == null ? DBNull.Value : complain.ISSENDFEEDBACK;

                        command.Parameters.Add("P_SHOWROOM", OracleDbType.Varchar2).Value =
                            (object?)complain.SHOWROOM ?? DBNull.Value;

                        command.Parameters.Add("P_ENTRYBY", OracleDbType.Varchar2).Value =
                            (object?)complain.ENTRYBY ?? DBNull.Value;

                        command.Parameters.Add("P_ENTRYDATE", OracleDbType.Date).Value =
                            complain.ENTRYDATE == null ? DBNull.Value : complain.ENTRYDATE;

                        command.Parameters.Add("P_TICKETID", OracleDbType.Int32).Value =
                            complain.TICKETID;

                        command.Parameters.Add("P_ZONEID", OracleDbType.Int32).Value =
                            complain.ZONEID;
                        command.Parameters.Add("P_ISACTIVE", OracleDbType.Int32).Value =
                            complain.ISACTIVE;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                Console.WriteLine(ex.Message);
                isSuccess= false;
            }
            return isSuccess;
        }
        public List<CompalinModel> GetALlComplainList(int CompanyID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("SP_GET_ALL_COMPLAINS", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.Add("P_COMPANYID", OracleDbType.Int32).Value = CompanyID;
                  //  command.Parameters.Add("P_SHOWROOM", OracleDbType.Varchar2).Value = showroomcode;
                    command.Parameters.Add("P_RECORDSET", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<CompalinModel>(dt).ToList();
        }
        public CompalinModel GetComplainListUsingID(int TickedID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("SP_GET_COMPLAIN_BY_ID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.Add("P_TICKETID", OracleDbType.Int32).Value = TickedID;
                   // command.Parameters.Add("P_SHOWROOM", OracleDbType.Varchar2).Value = showroomcode;

                    command.Parameters.Add("P_RECORDSET", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<CompalinModel>(dt).FirstOrDefault();
        }
        public bool UpdateCompalin(CompalinModel complain)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    connection.Open();

                    using (OracleCommand command = new OracleCommand("ESERV.SP_UPDATE_COMPLAIN", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.BindByName = true;
                        command.CommandTimeout = 300;
                        command.Parameters.Add("P_TICKETID", OracleDbType.Int32).Value = complain.TICKETID;
                        command.Parameters.Add("P_CUSTOMERNAME", OracleDbType.Varchar2).Value = (object?)complain.CUSTOMERNAME ?? DBNull.Value;
                        command.Parameters.Add("P_CONTACTNO", OracleDbType.Varchar2).Value = (object?)complain.CONTACTNO ?? DBNull.Value;
                        command.Parameters.Add("P_LOCATION", OracleDbType.Varchar2).Value = (object?)complain.LOCATION ?? DBNull.Value;
                        command.Parameters.Add("P_PROBLEMTYPEID", OracleDbType.Int32).Value = (object?)complain.PROBLEMTYPEID ?? DBNull.Value;
                        command.Parameters.Add("P_STATUSID", OracleDbType.Int32).Value = (object?)complain.STATUSID ?? DBNull.Value;
                        command.Parameters.Add("P_MODIFIEDBY", OracleDbType.Varchar2).Value = (object?)complain.MODIFIEDBY ?? DBNull.Value;
                        command.Parameters.Add("P_MODIFIEDPC", OracleDbType.Varchar2).Value = (object?)complain.MODIFIEDPC ?? DBNull.Value;
                        command.Parameters.Add("P_COMPLAINDATE", OracleDbType.Date).Value = (object?)complain.COMPLAINDATE ?? DBNull.Value;
                        command.Parameters.Add("P_ZONEID", OracleDbType.Int32).Value = (object?)complain.ZONEID ?? DBNull.Value;
                        command.Parameters.Add("P_TECHNICIANID", OracleDbType.Int32).Value = (object?)complain.TECHNICIANID ?? DBNull.Value;
                        command.Parameters.Add("P_TICKETCODE", OracleDbType.Varchar2).Value = (object?)complain.TICKETCODE ?? DBNull.Value;
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public CompalinModel GetComplainUsingTickedID(string TickedID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.SP_GET_COMPLAIN_BY_TicketCode", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.Add("P_TICKETID", OracleDbType.Varchar2).Value = TickedID;

                    command.Parameters.Add("P_RECORDSET", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<CompalinModel>(dt).FirstOrDefault();
        }
        public void InsertComplainDetails(ComplainProblemModel model)
    {
        try
        {
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                using (OracleCommand cmd = new OracleCommand("ESERV.SP_INSERT_COMPLAINDETAILS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("P_TICKETCODE", OracleDbType.Varchar2).Value = model.TICKETCODE;
                    cmd.Parameters.Add("P_PROBLEMID", OracleDbType.Int32).Value = model.PROBLEMID;
                    cmd.Parameters.Add("P_QUANTITY", OracleDbType.Int32).Value = model.QUANTITY;
                    cmd.Parameters.Add("P_REMARKS", OracleDbType.Varchar2).Value =
                        string.IsNullOrEmpty(model.REMARKS) ? (object)DBNull.Value : model.REMARKS;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
        public List<ComplainProblemModel> GetAllPromlemDetails(string TickedID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.SP_GET_COMPLAIN_DETAILS", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.Add("P_TICKETCODE", OracleDbType.Varchar2).Value = TickedID;


                    command.Parameters.Add("P_RECORDSET", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<ComplainProblemModel>(dt).ToList();
        }
        public bool DeleteProblemList(string TicketCode)
        {
            bool isDeleted = true;
            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    connection.Open();

                    using (OracleCommand command = new OracleCommand("ESERV.SP_ComplainProblem", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.BindByName = true;
                        command.CommandTimeout = 300;
                        command.Parameters.Add("P_TICKETCODE", OracleDbType.Varchar2).Value = TicketCode;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                isDeleted = false;
            }
            return isDeleted;
        }
    }
}
