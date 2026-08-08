using Oracle.ManagedDataAccess.Client;
using RetailCare.Common;
using RetailCare.Interface;
using RetailCare.Models;
using System.Data;

namespace RetailCare.Repositories
{
    public class ZoneRepository: IZoneRepository
    {
        private readonly string _connectionString;

        public ZoneRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("hps");
        }

        public List<ZoneModel> GetAllZoneDetails ()
        {
            DataTable dt = new DataTable();

            using (OracleConnection connection = new OracleConnection(_connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand("GetAllZoneDetails", connection))
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

            return ExtractData.Convert<ZoneModel>(dt).ToList();
        }
    }
}
