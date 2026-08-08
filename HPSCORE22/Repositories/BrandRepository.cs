using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Oracle.ManagedDataAccess.Client;
using QCMS.Services;
using RetailCare.Common;
using RetailCare.Interface;
using RetailCare.Models;
using RetailCare.Models.ServiceModel;
using System.ComponentModel.Design;
using System.Data;

namespace RetailCare.Repositories
{
    public class BrandRepository: IBrandRepository
    {
        private readonly string _connectionString;

        public BrandRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("hps");
        }
        public List<BrandModel> GetAllBrandList(int BrandID) 
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetAllBrand", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.Add("p_CompanyID", OracleDbType.Int32).Value = BrandID;

                    command.Parameters.Add("p_Recordset", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<BrandModel>(dt).ToList();
        }
        public BrandModel GetBrandUsngBrandID(int BrandID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetSinglebrandIDWise", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.Add("p_GROUPID", OracleDbType.Int32).Value = BrandID;

                    command.Parameters.Add("p_Recordset", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<BrandModel>(dt).FirstOrDefault();
        }
        public int InsertGroup(BrandModel model)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(_connectionString))
                {
                    con.Open();

                    using (OracleCommand cmd = new OracleCommand("ESERV.SP_INSERT_GROUPINFO", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("P_GROUPNAME", OracleDbType.Varchar2).Value = model.GROUPNAME;

                        OracleParameter outGroupId = new OracleParameter("P_GROUPID", OracleDbType.Int32);
                        outGroupId.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outGroupId);

                        cmd.ExecuteNonQuery();

                        return Convert.ToInt32(outGroupId.Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool UpdateGroup(BrandModel model)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(_connectionString))
                {
                    con.Open();

                    using (OracleCommand cmd = new OracleCommand("ESERV.SP_UPDATE_GROUPINFO", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("P_GROUPID", OracleDbType.Int32).Value = model.GROUPID;
                        cmd.Parameters.Add("P_GROUPNAME", OracleDbType.Varchar2).Value = model.GROUPNAME;

                        cmd.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
