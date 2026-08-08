using Oracle.ManagedDataAccess.Client;
using RetailCare.Common;
using RetailCare.Interface.ServiceInterface;
using RetailCare.Models;
using RetailCare.Models.ServiceModel;
using System.Data;

namespace RetailCare.Repositories
{
    public class ItemRepository: IItemRepository
    {
        private readonly string _connectionString;

        public ItemRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("hps");
        }
        public List<ItemModel> GetAllItemList(int CompanyID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("GetAllItem", connection))
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

            return ExtractData.Convert<ItemModel>(dt).ToList();
        }
        public List<ItemModel> GetAllItemListBrandWise(int BrandID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetAllItemListBrandWise", connection))
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

            return ExtractData.Convert<ItemModel>(dt).ToList();
        }
        public ItemModel GetAllClassUsingID(int BrandID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetSingleClassUsingID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.Add("p_ITEMID", OracleDbType.Int32).Value = BrandID;

                    command.Parameters.Add("p_Recordset", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<ItemModel>(dt).FirstOrDefault();
        }
        public int InsertClass(ItemModel model)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(_connectionString))
                {
                    con.Open();

                    using (OracleCommand cmd = new OracleCommand("ESERV.SP_INSERT_ITEM", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("P_ITEMNAME", OracleDbType.Varchar2).Value = model.ITEMNAME;

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
        public bool UpdateClass(ItemModel model)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(_connectionString))
                {
                    con.Open();

                    using (OracleCommand cmd = new OracleCommand("ESERV.SP_UPDATE_ITEM", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("P_ITEMID", OracleDbType.Int32).Value = model.ITEMID;
                        cmd.Parameters.Add("P_ITEMNAME", OracleDbType.Varchar2).Value = model.ITEMNAME;

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
