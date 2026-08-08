using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using RetailCare.Common;
using RetailCare.Interface.ServiceInterface;
using RetailCare.Models;
using RetailCare.Models.ServiceModel;
using System.ComponentModel.Design;
using System.Data;

namespace RetailCare.Repositories.ServiceRepository
{
    public class TechnicianRepository: ITechnicianRepository
    {
        private readonly string _connectionString;

        public TechnicianRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("hps");
        }
        public int AddTechnician(TechnicianModel technician)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    connection.Open();

                    using (OracleCommand command = new OracleCommand("ESERV.SP_INSERT_TECHNICIANOUT", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.BindByName = true;

                        // OUT Parameter
                        command.Parameters.Add("P_TECHNICIANID", OracleDbType.Int32)
                            .Direction = ParameterDirection.Output;

                        // IN Parameters
                        command.Parameters.Add("P_TECHNICIANCODE", OracleDbType.Int32).Value =
                            technician.TECHNICIANCODE.ToString();

                        command.Parameters.Add("P_TECHNICIANNAME", OracleDbType.Varchar2).Value =
                            (object?)technician.TECHNICIANNAME ?? DBNull.Value;

                        command.Parameters.Add("P_STAFFID", OracleDbType.Int32).Value =
                            technician.STAFFID?.ToString() ?? (object)DBNull.Value;

                        command.Parameters.Add("P_CONTACTNO", OracleDbType.Varchar2).Value =
                            (object?)technician.CONTACTNO ?? DBNull.Value;

                        command.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value =
                            (object?)technician.EMAIL ?? DBNull.Value;

                        command.Parameters.Add("P_ADDRESS", OracleDbType.Varchar2).Value =
                            (object?)technician.ADDRESS ?? DBNull.Value;

                        command.Parameters.Add("P_DEPARTMENTID", OracleDbType.Int32).Value =
                            technician.DEPARTMENTID ?? (object)DBNull.Value;

                        command.Parameters.Add("P_DESIGNATIONID", OracleDbType.Int32).Value =
                            technician.DESIGNATIONID ?? (object)DBNull.Value;

                        command.Parameters.Add("P_SUPERVISORID", OracleDbType.Int32).Value =
                            technician.SUPERVISORID ?? (object)DBNull.Value;

                        command.Parameters.Add("P_ZONEID", OracleDbType.Int32).Value =
                            technician.ZONEID ?? (object)DBNull.Value;

                        command.Parameters.Add("P_GROUPID", OracleDbType.Int32).Value =
                            technician.GROUPID ?? (object)DBNull.Value;

                        command.Parameters.Add("P_COMPANYID", OracleDbType.Int32).Value =
                            technician.COMPANYID ?? (object)DBNull.Value;

                        command.Parameters.Add("P_ACTIVE", OracleDbType.Int32).Value =
                            technician.ACTIVE ?? (object)DBNull.Value;

                        command.Parameters.Add("P_ENTRYBY", OracleDbType.Varchar2).Value =
                            (object?)technician.ENTRYBY ?? DBNull.Value;

                        //command.Parameters.Add("P_ENTRYPC", OracleDbType.Varchar2).Value =
                        //    Environment.MachineName;

                        command.ExecuteNonQuery();

                        OracleDecimal technicianId = (OracleDecimal)command.Parameters["P_TECHNICIANID"].Value;

                        return technicianId.ToInt32();
                    }
                }
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                throw new Exception($"Oracle Error ({ex.Number}): {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting technician: " + ex.Message, ex);
            }
        }
        public void AddAssignClass(TECHNICIANSASSIGNPRODUCT Technicians)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    connection.Open();

                    using (OracleCommand command = new OracleCommand("ESERV.SP_INSERT_ASSIGNCLASS", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.BindByName = true;
                        command.CommandTimeout = 300;
                        command.Parameters.Add("P_TECHNICIANID", OracleDbType.Int32).Value = Technicians.TECHNICIANID;
                        command.Parameters.Add("P_PRODUCTID", OracleDbType.Int32).Value = Technicians.PRODUCTID;
                        command.Parameters.Add("P_ACTIVE", OracleDbType.Int32).Value = Technicians.ACTIVE;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public List<TechnicianModel> GetAllTechnicianData(int CompanyID,int ClassID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.SP_GET_TECHNICIAN_BY_ITEM", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.Add("P_ITEMID", OracleDbType.Int32).Value = ClassID;
                    command.Parameters.Add("P_COMPANYID", OracleDbType.Int32).Value = CompanyID;

                    command.Parameters.Add("P_RECORDSET", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<TechnicianModel>(dt).ToList();
        }
        public bool DeletePreviousPermission(int TechnicioanID)
        {
            bool isDeleted = true;
            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    connection.Open();

                    using (OracleCommand command = new OracleCommand("ESERV.SP_DELETE_TECHNICIAN_ASSIGNCLASS", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.BindByName = true;
                        command.CommandTimeout = 300;
                        command.Parameters.Add("P_TECHNICIANID", OracleDbType.Int32).Value = TechnicioanID;
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
        public bool UpdateTechnician(TechnicianModel technician)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    connection.Open();

                    using (OracleCommand command = new OracleCommand("ESERV.SP_UPDATE_TECHNICIAN", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.BindByName = true;

                        // Primary Key
                        command.Parameters.Add("P_TECHNICIANID", OracleDbType.Int32).Value =
                            technician.TECHNICIANID;

                        command.Parameters.Add("P_TECHNICIANCODE", OracleDbType.Int32).Value =
                            technician.TECHNICIANCODE;

                        command.Parameters.Add("P_TECHNICIANNAME", OracleDbType.Varchar2).Value =
                            (object?)technician.TECHNICIANNAME ?? DBNull.Value;

                        command.Parameters.Add("P_STAFFID", OracleDbType.Int32).Value =
                            technician.STAFFID ?? (object)DBNull.Value;

                        command.Parameters.Add("P_CONTACTNO", OracleDbType.Varchar2).Value =
                            (object?)technician.CONTACTNO ?? DBNull.Value;

                        command.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value =
                            (object?)technician.EMAIL ?? DBNull.Value;

                        command.Parameters.Add("P_ADDRESS", OracleDbType.Varchar2).Value =
                            (object?)technician.ADDRESS ?? DBNull.Value;

                        command.Parameters.Add("P_DEPARTMENTID", OracleDbType.Int32).Value =
                            technician.DEPARTMENTID ?? (object)DBNull.Value;

                        command.Parameters.Add("P_DESIGNATIONID", OracleDbType.Int32).Value =
                            technician.DESIGNATIONID ?? (object)DBNull.Value;

                        command.Parameters.Add("P_SUPERVISORID", OracleDbType.Int32).Value =
                            technician.SUPERVISORID ?? (object)DBNull.Value;

                        command.Parameters.Add("P_ZONEID", OracleDbType.Int32).Value =
                            technician.ZONEID ?? (object)DBNull.Value;

                        command.Parameters.Add("P_GROUPID", OracleDbType.Int32).Value =
                            technician.GROUPID ?? (object)DBNull.Value;

                        command.Parameters.Add("P_COMPANYID", OracleDbType.Int32).Value =
                            technician.COMPANYID ?? (object)DBNull.Value;

                        command.Parameters.Add("P_ACTIVE", OracleDbType.Int32).Value =
                            technician.ACTIVE ?? (object)DBNull.Value;

                        command.Parameters.Add("P_MODIFIEDBY", OracleDbType.Varchar2).Value =
                            (object?)technician.MODIFIEDBY ?? DBNull.Value;

                        command.ExecuteNonQuery();

                        return true;
                    }
                }
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                throw new Exception($"Oracle Error ({ex.Number}): {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating technician: " + ex.Message, ex);
            }
        }
        public List<TechnicianModel> GetAllTechniciansList(int CompanyID)
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

            return ExtractData.Convert<TechnicianModel>(dt).ToList();
        }
        public TechnicianModel GetSingleTechnicians(int CompanyID, int TechniciansID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetSingleTechnician", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.Add("p_CompanyID", OracleDbType.Int32).Value = CompanyID;
                    command.Parameters.Add("p_TECHNICIANID", OracleDbType.Int32).Value = TechniciansID;

                    command.Parameters.Add("p_Result", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<TechnicianModel>(dt).FirstOrDefault();
        }
        public List<TECHNICIANSASSIGNPRODUCT> GetTechniciansAssignClassList(int TechniciansID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetAllAssignClassList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_TECHNICIANID", OracleDbType.Int32).Value = TechniciansID;

                    command.Parameters.Add("p_Result", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<TECHNICIANSASSIGNPRODUCT>(dt).ToList();
        }
        public TechnicianModel GetSingleTechnicianUsingStaffID(int StaffID)
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("ESERV.GetSingleTechnicianUsingStaffID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 300;

                    command.Parameters.Add("p_TECHNICIANID", OracleDbType.Varchar2).Value = StaffID;

                    command.Parameters.Add("p_Result", OracleDbType.RefCursor)
                           .Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return ExtractData.Convert<TechnicianModel>(dt).FirstOrDefault();
        }
    }
}
