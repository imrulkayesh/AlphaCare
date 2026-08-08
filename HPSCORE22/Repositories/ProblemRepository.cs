using Microsoft.CodeAnalysis;
using Oracle.ManagedDataAccess.Client;
using RetailCare.Common;
using RetailCare.Interface;
using RetailCare.Models;
using System.Data;

namespace RetailCare.Repositories
{
    public class ProblemRepository: IProblemRepository
    {
        private readonly string _connectionString;

        public ProblemRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("hps");
        }

        public List<ProblemModel> GetAllProblemList(int CompanyID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("GetAllProblem", connection))
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

            return ExtractData.Convert<ProblemModel>(dt).ToList();
        }
        public List<ProblemModel> GetAllProblemUsingProductID(int ProductID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetAllProblemUsingProductID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_PRODUCTID", OracleDbType.Int32).Value = ProductID;

                    command.Parameters.Add("p_Result", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<ProblemModel>(dt).ToList();
        }
        public List<Subproblem> GetAllSubproblemProblemWise(int ProblemID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetAllSubproblemProblemWise", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_PROBLEMID", OracleDbType.Int32).Value = ProblemID;

                    command.Parameters.Add("p_Result", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<Subproblem>(dt).ToList();
        }
    }
}
