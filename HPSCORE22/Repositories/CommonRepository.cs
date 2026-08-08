using Dapper;
using QCMS.Services;

namespace QCMS.Repositories
{
    public interface ICommonServiceMethods
    {
        public int GeneratingTickedCode();
    }
    public class CommonRepository
    {
        private readonly DatabaseService _databaseService;

        public CommonRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        }
        public async Task<string> GenerateIdAsync(string tableName)
        {
            using var conn = _databaseService.GetConnection();
            await conn.OpenAsync();

            using var tran = conn.BeginTransaction();

            try
            {
                string sql = @"
            SELECT TABLE_SEQ, PREFIX 
            FROM TBL_SEQ 
            WHERE TABLE_NAME = :tableName AND ACT = 1
            FOR UPDATE";

                var result = await conn.QueryFirstOrDefaultAsync<dynamic>(
                    sql, new { tableName }, tran);

                if (result == null)
                    throw new Exception($"Sequence not found for {tableName}");

                long currentSeq = result.TABLE_SEQ;
                string prefix = result.PREFIX;

                long newSeq = currentSeq + 1;

                string updateSql = @"
            UPDATE TBL_SEQ 
            SET TABLE_SEQ = :seq 
            WHERE TABLE_NAME = :tableName";

                await conn.ExecuteAsync(updateSql,
                    new { seq = newSeq, tableName }, tran);

                tran.Commit();

                // 🔥 Final ID format
                return $"{prefix}-{newSeq.ToString("D4")}";
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

    }
    public class CommonServiceMethods: ICommonServiceMethods
    {
        private readonly DatabaseService _databaseService;

        public CommonServiceMethods(DatabaseService databaseService)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        }
        public int GeneratingTickedCode()
        {
            int ticketCode = 0;
            try
            {
                using (var connection = _databaseService.GetConnection())
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT SEQ_AlphaCare_COMPLAIN.NEXTVAL FROM DUAL";

                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating ticket code: " + ex.Message);
            }

            return ticketCode;
        }


    }

}
