using Oracle.ManagedDataAccess.Client;
using RetailCare.Common;
using RetailCare.Interface.ServiceInterface;
using RetailCare.Models;
using RetailCare.Models.CRMModels;
using RetailCare.Models.ServiceModel;
using System.Data;

namespace RetailCare.Repositories.ServiceRepository
{
    public class AssignmentManagementRepository: IAssignmentManagementRepository
    {
        private readonly string _connectionString;

        public AssignmentManagementRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("hps");
        }
        public int InsertAssignTechnician(AssignmentModel model)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    connection.Open();

                    using (OracleCommand command = new OracleCommand("ESERV.SP_INSERT_ASSIGNTECHNICIAN", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.BindByName = true;
                        command.Parameters.Add("P_TICKETID", OracleDbType.Varchar2).Value = model.TICKETID;
                        command.Parameters.Add("P_ASSIGNDATE", OracleDbType.Date).Value = model.ASSIGNDATE;
                        command.Parameters.Add("P_STATUSID", OracleDbType.Int32).Value = model.STATUSID;
                        command.Parameters.Add("P_TECHNICIANID", OracleDbType.Int32).Value = model.TECHNICIANID;
                        command.Parameters.Add("P_COMPANYID", OracleDbType.Int32).Value = model.COMPANYID;
                        command.Parameters.Add("P_CUSTOMERNAME", OracleDbType.Varchar2).Value = model.CUSTOMERNAME;
                        command.Parameters.Add("P_CUSTOMERCONTACTNO", OracleDbType.Varchar2).Value = model.CUSTOMERCONTACTNO;
                        command.Parameters.Add("P_CUSTOMERADDRESS", OracleDbType.Varchar2).Value = model.CUSTOMERADDRESS;
                        command.Parameters.Add("P_ISASSIGN", OracleDbType.Int32).Value = model.ISASSIGN;
                        command.Parameters.Add("P_SENDFEEDBACK", OracleDbType.Int32).Value = model.SENDFEEDBACK;
                        command.Parameters.Add("P_ENTRYBY", OracleDbType.Varchar2).Value = model.ENTRYBY;
                        command.Parameters.Add("P_PRODUCTID", OracleDbType.Int32).Value = model.PRODUCTID;
                        command.Parameters.Add("P_PROBLEMID", OracleDbType.Int32).Value = model.PROBLEMID;
                        // OUT Parameter
                        command.Parameters.Add("P_ASSIGNID", OracleDbType.Int32)
                               .Direction = ParameterDirection.Output;
                        command.ExecuteNonQuery();
                        return Convert.ToInt32(command.Parameters["P_ASSIGNID"].Value.ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<AssignmentModel> GetAllAssignmentListForFeedback(int CompanyID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetAllAssignTaskFeedack", connection))
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

            return ExtractData.Convert<AssignmentModel>(dt).ToList();
        }
        public AssignmentModel GetSingleTaskAssignList(int CompanyID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetSngleListAssign", connection))
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

            return ExtractData.Convert<AssignmentModel>(dt).FirstOrDefault();
        }
        public AssignmentModel GetSingleTaskUsingTickedID(string TicketID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetSngleListAssign", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.Add("p_TICKETID", OracleDbType.Varchar2).Value = TicketID;

                    command.Parameters.Add("p_Recordset", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return ExtractData.Convert<AssignmentModel>(dt).FirstOrDefault();
        }
        public bool UpdateAssign(AssignmentModel AssignModel)
        {
            bool IsUpdated = true;
            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    connection.Open();

                    using (OracleCommand command = new OracleCommand("ESERV.SP_UPDATE_ASSIGN_FEEDBACK", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.BindByName = true;
                        command.CommandTimeout = 300;
                        command.Parameters.Add("P_TICKETID", OracleDbType.Int32).Value = AssignModel.TICKETID;
                        command.Parameters.Add("P_STATUSID", OracleDbType.Int32).Value = (object?)AssignModel.STATUSID ?? DBNull.Value;
                        command.Parameters.Add("P_SENDFEEDBACK", OracleDbType.Date).Value = (object?)AssignModel.SENDFEEDBACK ?? DBNull.Value;
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IsUpdated = false;
              
            }
            return IsUpdated;
        }
        public List<AssignmentModel> GetAllAssignmentListForFeedbackTechnicianIDWise(int TechnicianID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetAllAssignTaskFeedackTechnicianIDWise", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.Add("P_TechnicianID", OracleDbType.Int32).Value = TechnicianID;

                    command.Parameters.Add("p_Recordset", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<AssignmentModel>(dt).ToList();
        }
        public bool UpdateAssignTechnician(AssignmentModel model)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    connection.Open();

                    using (OracleCommand command = new OracleCommand("ESERV.SP_UPDATE_ASSIGNTECHNICIAN", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.BindByName = true;
                        command.Parameters.Add("P_TICKETID", OracleDbType.Varchar2).Value = model.TICKETID;
                        command.Parameters.Add("P_ASSIGNDATE", OracleDbType.Date).Value = model.ASSIGNDATE;
                        command.Parameters.Add("P_STATUSID", OracleDbType.Int32).Value = model.STATUSID;
                        command.Parameters.Add("P_TECHNICIANID", OracleDbType.Int32).Value = model.TECHNICIANID;
                        command.Parameters.Add("P_COMPANYID", OracleDbType.Int32).Value = model.COMPANYID;
                        command.Parameters.Add("P_CUSTOMERNAME", OracleDbType.Varchar2).Value = model.CUSTOMERNAME;
                        command.Parameters.Add("P_CUSTOMERCONTACTNO", OracleDbType.Varchar2).Value = model.CUSTOMERCONTACTNO;
                        command.Parameters.Add("P_CUSTOMERADDRESS", OracleDbType.Varchar2).Value = model.CUSTOMERADDRESS;
                        command.Parameters.Add("P_ISASSIGN", OracleDbType.Int32).Value = model.ISASSIGN;
                        command.Parameters.Add("P_SENDFEEDBACK", OracleDbType.Int32).Value = model.SENDFEEDBACK;
                        command.Parameters.Add("P_PROBLEMID", OracleDbType.Int32).Value = model.PROBLEMID;

                        command.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception if needed
                throw;
            }
        }
    }
}
