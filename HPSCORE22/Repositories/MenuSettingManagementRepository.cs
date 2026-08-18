using AlphaCare.Interface;
using AlphaCare.Models;
using Oracle.ManagedDataAccess.Client;
using RetailCare.Common;
using RetailCare.Models;
using System.Data;

namespace AlphaCare.Repositories
{
    public class MenuSettingManagementRepository : IMenuSettingManagementRepository
    {
        private readonly string _connectionString;

        public MenuSettingManagementRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("hps");
        }
        public List<RoleWiseMenuPermission> GetAllRoleWiseMenu(int RoleID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.SP_GET_USER_MENUS", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.Add("P_COMPANYID", OracleDbType.Int32).Value = RoleID;

                    command.Parameters.Add("p_Recordset", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<RoleWiseMenuPermission>(dt).ToList();
        }
        public bool AddNewRole(RoleModel RoleManagement)
        {
            bool isSuccess = true;
            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    connection.Open();

                    using (OracleCommand command = new OracleCommand("ESERV.SP_INSERT_SERVICETYPE", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("P_TYPECODE", OracleDbType.Varchar2)
                            .Value = RoleManagement.TYPECODE;

                        command.Parameters.Add("P_TYPENAME", OracleDbType.Varchar2)
                            .Value = RoleManagement.TYPENAME;

                        command.Parameters.Add("P_SERVICETYPE", OracleDbType.Varchar2)
                            .Value = RoleManagement.SERVICETYPE;

                        command.Parameters.Add("P_ENTRYBY", OracleDbType.Int32)
                            .Value = RoleManagement.ENTRYBY;

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return isSuccess;
        }
        public bool UpdateRole(RoleModel RoleManagement)
        {
            bool isSuccess = true;
            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    connection.Open();
                    using (OracleCommand command = new OracleCommand("ESERV.SP_UPDATE_SERVICETYPE", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("P_TYPEID", OracleDbType.Int32)
                            .Value = RoleManagement.TYPEID;
                        command.Parameters.Add("P_TYPECODE", OracleDbType.Varchar2)
                            .Value = RoleManagement.TYPECODE;
                        command.Parameters.Add("P_TYPENAME", OracleDbType.Varchar2)
                            .Value = RoleManagement.TYPENAME;
                        command.Parameters.Add("P_SERVICETYPE", OracleDbType.Varchar2)
                            .Value = RoleManagement.SERVICETYPE;
                        command.Parameters.Add("P_MODIFIEDBY", OracleDbType.Int32)
                            .Value = RoleManagement.MODIFIEDBY;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return isSuccess;
        }
        public List<RoleModel> GetAllRoles()
        {
            DataTable dt = new DataTable();
            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using (OracleCommand command = new OracleCommand("ESERV.SP_GET_ALL_USERTYPE", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;
                    command.Parameters.Add("p_Recordset", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;
                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return ExtractData.Convert<RoleModel>(dt).ToList();
        }

    }
}
