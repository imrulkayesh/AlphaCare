using Humanizer;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using QCMS.Services;
using RetailCare.Common;
using RetailCare.Interface;
using RetailCare.Models;
using System.Data;

namespace RetailCare.Repositories
{
    public class UserManagementRepository: IUserManagementRepository
    {
        private readonly string _connectionString;

        public UserManagementRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("hps");
        }
        public List<UserTypeModel> GetAllUserType()
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetAllUserType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_Result", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<UserTypeModel>(dt).ToList();
        }
        public List<DepartmentModel> GetAllDepartment()
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetAllDepartment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_Result", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<DepartmentModel>(dt).ToList();
        }
        public List<DesignationModel> GetAllDesignation()
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetAllDESIGNATION", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_Result", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<DesignationModel>(dt).ToList();
        }
        public UserInfoModel CheckUserUnique(int CompanyID,string UserID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.CheckUserDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.Add("p_CompanyID", OracleDbType.Int32).Value = CompanyID;
                    command.Parameters.Add("p_UserName", OracleDbType.Varchar2).Value = UserID;

                    command.Parameters.Add("p_Recordset", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<UserInfoModel>(dt).FirstOrDefault();
        }
        public int InsertUser(UserModel model)
    {
        int userCode = 0;

        using (OracleConnection con = new OracleConnection(_connectionString))
        {
             con.OpenAsync();

            using (OracleCommand cmd = new OracleCommand("SP_INSERT_USERINFO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("P_BU_CODE", OracleDbType.Varchar2).Value = model.BU_CODE;
                cmd.Parameters.Add("P_USERID", OracleDbType.Varchar2).Value = model.USERID;
                cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2).Value = model.USERNAME;
                cmd.Parameters.Add("P_STAFFID", OracleDbType.Varchar2).Value = model.STAFFID;
                cmd.Parameters.Add("P_USERTYPEID", OracleDbType.Int32).Value = model.USERTYPEID;
                cmd.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2).Value = model.PASSWORD;
                cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = model.EMAIL;
                cmd.Parameters.Add("P_CONTACTNO", OracleDbType.Varchar2).Value = model.CONTACTNO;
                cmd.Parameters.Add("P_ADDRESS", OracleDbType.Varchar2).Value = model.ADDRESS;
                cmd.Parameters.Add("P_COMPANYID", OracleDbType.Int32).Value = model.COMPANYID;
                cmd.Parameters.Add("P_DEPARTMENTID", OracleDbType.Int32).Value = model.DEPARTMENTID;
                cmd.Parameters.Add("P_DESIGNATIONID", OracleDbType.Int32).Value = model.DESIGNATIONID;
                cmd.Parameters.Add("P_ZONEID", OracleDbType.Int32).Value = model.ZONEID;
                cmd.Parameters.Add("P_DEPOTID", OracleDbType.Int32).Value = model.DEPOTID;
                cmd.Parameters.Add("P_DEPOACT", OracleDbType.Varchar2).Value = model.DEPOACT;
                cmd.Parameters.Add("P_ISACTIVE", OracleDbType.Int32).Value = model.ISACTIVE;
                cmd.Parameters.Add("P_ENTRYBY", OracleDbType.Int32).Value = model.ENTRYBY;
                cmd.Parameters.Add("P_MODIFIEDBY", OracleDbType.Int32).Value = model.MODIFIEDBY;

                // Output Parameter
                OracleParameter outParam = new OracleParameter("P_USERCODE", OracleDbType.Int32);
                outParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outParam);

                cmd.ExecuteNonQueryAsync();

                if (outParam.Value != DBNull.Value)
                {
                    userCode = Convert.ToInt32(((OracleDecimal)outParam.Value).Value);
                }
            }
        }

        return userCode;
    }
        public List<UserModel> GetAllUserList(int CompanyID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetAllUser", connection))
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

            return ExtractData.Convert<UserModel>(dt).ToList();
        }
        public bool InsertUserCompany(UserCampany model)
        {
            bool IsAdded = true;
            try
            {
                using (OracleConnection con = new OracleConnection(_connectionString))
                {
                    con.OpenAsync();

                    using (OracleCommand cmd = new OracleCommand("SP_INSERT_USERCOMPANY", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("P_USERID", OracleDbType.Varchar2).Value = model.USERID;
                        cmd.Parameters.Add("P_COMPANYID", OracleDbType.Int32).Value = model.COMPANYID;
                        cmd.Parameters.Add("P_ISACTIVE", OracleDbType.Int32).Value = model.ISACTIVE;
                        cmd.Parameters.Add("P_ENTRYBY", OracleDbType.Varchar2).Value = model.ENTRYBY;

                        cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
                IsAdded =false;
            }
            return IsAdded;
        }
        public UserModel GetUserDetailsUsingID(int UserCode)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetSingleUserUsingID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.Add("p_USERCODE", OracleDbType.Int32).Value = UserCode;

                    command.Parameters.Add("p_Recordset", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<UserModel>(dt).FirstOrDefault();
        }
        public bool UpdateUser(UserModel model)
        {
            bool IsUpdated = true;
            try
            {
                using (OracleConnection con = new OracleConnection(_connectionString))
                {
                    con.OpenAsync();

                    using (OracleCommand cmd = new OracleCommand("ESERV.SP_UPDATE_USERINFO", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("P_USERCODE", OracleDbType.Int32).Value = model.USERCODE;
                        cmd.Parameters.Add("P_BU_CODE", OracleDbType.Varchar2).Value = model.BU_CODE;
                        cmd.Parameters.Add("P_USERID", OracleDbType.Varchar2).Value = model.USERID;
                        cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2).Value = model.USERNAME;
                        cmd.Parameters.Add("P_STAFFID", OracleDbType.Varchar2).Value = model.STAFFID;
                        cmd.Parameters.Add("P_USERTYPEID", OracleDbType.Int32).Value = model.USERTYPEID;
                        cmd.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2).Value = model.PASSWORD;
                        cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = model.EMAIL;
                        cmd.Parameters.Add("P_CONTACTNO", OracleDbType.Varchar2).Value = model.CONTACTNO;
                        cmd.Parameters.Add("P_ADDRESS", OracleDbType.Varchar2).Value = model.ADDRESS;
                        cmd.Parameters.Add("P_COMPANYID", OracleDbType.Int32).Value = model.COMPANYID;
                        cmd.Parameters.Add("P_DEPARTMENTID", OracleDbType.Int32).Value = model.DEPARTMENTID;
                        cmd.Parameters.Add("P_DESIGNATIONID", OracleDbType.Int32).Value = model.DESIGNATIONID;
                        cmd.Parameters.Add("P_ZONEID", OracleDbType.Int32).Value = model.ZONEID;
                        cmd.Parameters.Add("P_DEPOTID", OracleDbType.Int32).Value = model.DEPOTID;
                        cmd.Parameters.Add("P_DEPOACT", OracleDbType.Varchar2).Value = model.DEPOACT;
                        cmd.Parameters.Add("P_ISACTIVE", OracleDbType.Int32).Value = model.ISACTIVE;
                        cmd.Parameters.Add("P_MODIFIEDBY", OracleDbType.Int32).Value = model.MODIFIEDBY;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                 IsUpdated = false;
            }
            return IsUpdated;
        }


        // User Wise Menu Permission
        public List<Menus> GetAllMenuList()
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetAllMenus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_Result", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<Menus>(dt).ToList();
        }
        public List<ParentMenus> GetAllParentsMenu()
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetAllParentsMenus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("p_Result", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<ParentMenus>(dt).ToList();
        }
        public bool DeletePreviousPermission(string UserID)
        {
            bool isDeleted = true;
            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    connection.Open();

                    using (OracleCommand command = new OracleCommand("ESERV.SP_UserWiseMenuDelete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.BindByName = true;
                        command.CommandTimeout = 300;
                        command.Parameters.Add("P_USERID", OracleDbType.Varchar2).Value = UserID;
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
        public bool ADDPermissionWiseMenuPermission(UserWiseMenuPer UserWiseMenu)
        {
            bool isDeleted = true;
            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    connection.Open();
                    using (OracleCommand cmd = new OracleCommand("ESERV.SP_INSERT_USERMENUPERMISSION", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_USERID", OracleDbType.Varchar2).Value = UserWiseMenu.USERID;
                        cmd.Parameters.Add("P_MENUID", OracleDbType.Int32).Value = UserWiseMenu.MENUID;
                        cmd.Parameters.Add("P_ACTIVE", OracleDbType.Int32).Value = UserWiseMenu.ACTIVE;
                        cmd.Parameters.Add("P_ENTRYBY", OracleDbType.Varchar2).Value = UserWiseMenu.ENTRYBY;
                        cmd.ExecuteNonQuery();
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
