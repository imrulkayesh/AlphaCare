using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Oracle.ManagedDataAccess.Client;
using RetailCare.Common;
using RetailCare.Interface.ServiceInterface;
using RetailCare.Models.CRMModels;
using RetailCare.Models.ServiceModel;
using System.Data;

namespace RetailCare.Repositories.ServiceRepository
{
    public class FeedbackRepository: IFeedbackRepository
    {
        private readonly string _connectionString;

        public FeedbackRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("hps");
        }
        public List<CompalinModel> GetAllFeedbackList(int CompanyID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("GetALlTechniciansList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_CompanyID", OracleDbType.Int32).Value = CompanyID;

                    command.Parameters.Add("p_Result", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<CompalinModel>(dt).ToList();
        }
        public bool AddNewFeedback(FeedabackModel Feedback)
        {
            bool IsAdded = true;
            if (Feedback != null) {
            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    connection.Open();

                    using (OracleCommand cmd = new OracleCommand("SP_INSERT_FEEDBACK", connection))
                    {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.BindByName = true;
                            cmd.CommandTimeout = 300;
                            cmd.Parameters.Add("P_TICKETID", OracleDbType.Varchar2).Value = Feedback.TICKETID;
                            cmd.Parameters.Add("P_TICKETCODE", OracleDbType.Int64).Value = Feedback.TICKETCODE;
                            cmd.Parameters.Add("P_FEEDBACKINFO", OracleDbType.Varchar2).Value = Feedback.FEEDBACKINFO;
                            cmd.Parameters.Add("P_WORKINGDATE", OracleDbType.Date).Value = Feedback.WORKINGDATE;
                            cmd.Parameters.Add("P_PURCHASEDATE", OracleDbType.Date).Value = Feedback.PURCHASEDATE;
                            cmd.Parameters.Add("P_PROBLEMID", OracleDbType.Int32).Value = Feedback.PROBLEMID;
                            cmd.Parameters.Add("P_ACTUALPROBLEMID", OracleDbType.Int32).Value = Feedback.ACTUALPROBLEMID;
                            cmd.Parameters.Add("P_SUBPROBLEM", OracleDbType.Varchar2).Value = Feedback.SUBPROBLEM;
                            cmd.Parameters.Add("P_SOLVEDBY", OracleDbType.Varchar2).Value = Feedback.SOLVEDBY;
                            cmd.Parameters.Add("P_ITEMID", OracleDbType.Int32).Value = Feedback.ITEMID;
                            cmd.Parameters.Add("P_PRODUCTID", OracleDbType.Int32).Value = Feedback.PRODUCTID;
                            cmd.Parameters.Add("P_PRMODELID", OracleDbType.Int32).Value = Feedback.PRMODELID;
                            cmd.Parameters.Add("P_SERIALNO", OracleDbType.Varchar2).Value = Feedback.SERIALNO;
                            cmd.Parameters.Add("P_PRODUCTQTY", OracleDbType.Int32).Value = Feedback.PRODUCTQTY;
                            cmd.Parameters.Add("P_USEDSPAREPARTS", OracleDbType.Varchar2).Value = Feedback.USEDSPAREPARTS;
                            cmd.Parameters.Add("P_REMARKS", OracleDbType.Varchar2).Value = Feedback.REMARKS;
                            cmd.Parameters.Add("P_STATUSID", OracleDbType.Int32).Value = Feedback.STATUSID;
                            cmd.Parameters.Add("P_TECHID1", OracleDbType.Int32).Value = Feedback.TECHID1;
                            cmd.Parameters.Add("P_BILLAMOUNT", OracleDbType.Int32).Value = Feedback.BILLAMOUNT;
                            cmd.Parameters.Add("P_TECHID2", OracleDbType.Int32).Value = Feedback.TECHID2;
                            cmd.Parameters.Add("P_TECHID3", OracleDbType.Int32).Value = Feedback.TECHID3;
                            cmd.Parameters.Add("P_BARCODENO", OracleDbType.Varchar2).Value = Feedback.BARCODENO;
                            cmd.Parameters.Add("P_PSMODE", OracleDbType.Varchar2).Value = Feedback.PSMODE;
                            cmd.Parameters.Add("P_SRMODE", OracleDbType.Varchar2).Value = Feedback.SRMODE;
                            cmd.Parameters.Add("P_ISAPPROVED", OracleDbType.Int32).Value = Feedback.ISAPPROVED;
                            cmd.Parameters.Add("P_SENDCOST", OracleDbType.Int32).Value = Feedback.SENDCOST;
                            cmd.Parameters.Add("P_FILEPATH", OracleDbType.Varchar2).Value = Feedback.FILEPATH;
                            cmd.ExecuteNonQuery();

                        return true;
                    }
                }
            }
              catch (Exception Ex)
               {
                    throw new Exception(Ex.ToString());
                    IsAdded = false;
               }

            }
            else
            {
                IsAdded = false;
            }
            return IsAdded;
        }
    }
}
