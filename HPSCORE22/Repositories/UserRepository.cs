using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Oracle.ManagedDataAccess.Client;
using QCMS.Models;
using QCMS.Services;
using RetailCare.Models;
using System.Text;

namespace QCMS.Repositories
{
    public class UserRepository
    {
        private readonly DatabaseService _databaseService;

        public UserRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        }

        //public User? GetUser(string userText, string userPass)
        //{
        //    using var connection = _databaseService.GetConnection();

        //    // Query Y/N fields as-is, keep strings
        //    string sql = @"SELECT USER_TEXT,USER_NAME,USER_PASS,ACTIVE,IUSER, EUSER,IDAT,EDAT,VER,UDT,SITE_ID,EMAIL_ADD,MOBILE_NO,USER_TYPE,USER_SUSR,USER_STAT, 
        //    USER_REAU,USER_ISAU,USER_MRAU,USER_POAU,USER_PRAU,USER_GUSR,USER_QUSR,SOFT_VERSION,HR_ACTIVE,HR_ACTIVE1,HR_ID,ACCEPTED, ASSIGN_BU
        //    FROM FC.T_ACCOUNTS_USERS WHERE UPPER(TRIM(USER_TEXT)) = :userText AND USER_PASS = :userPass";

        //    var user = connection.QueryFirstOrDefault<User>(sql,new{userText = userText.Trim().ToUpper(),userPass = userPass.Trim()});

        //    // Optional: trim all string fields to avoid whitespace issues
        //    if (user != null)
        //    {
        //        user.ACTIVE = user.ACTIVE?.Trim().ToUpper();
        //        user.USER_REAU = user.USER_REAU?.Trim().ToUpper();
        //        user.USER_ISAU = user.USER_ISAU?.Trim().ToUpper();
        //        user.USER_MRAU = user.USER_MRAU?.Trim().ToUpper();
        //        user.USER_POAU = user.USER_POAU?.Trim().ToUpper();
        //        user.USER_PRAU = user.USER_PRAU?.Trim().ToUpper();
        //        user.HR_ACTIVE = user.HR_ACTIVE?.Trim().ToUpper();
        //        user.HR_ACTIVE1 = user.HR_ACTIVE1?.Trim().ToUpper();
        //        user.ACCEPTED = user.ACCEPTED?.Trim().ToUpper();
        //    }

        //    return user;
        //}

        public List<UserInfoModel>? GetUserInfo(string username)
        {
            using var conn = _databaseService.GetConnection();
            StringBuilder sb = new StringBuilder();

            //string sql = @"SELECT U.USER_TEXT, U.USER_NAME, U.USER_TYPE
            //FROM QCMS.UC_USERS  U WHERE UPPER(U.USER_TEXT) = :Username AND U.PASSWORD = :Password AND U.IS_ACTIVE = '1'";

            //string sql = @" SELECT U.UserId, U.UserName, U.UserTypeId,U.CompanyId,U.ZoneId
            //        FROM UserInfo U
            //        WHERE U.UserId = @Username
            //        AND U.Password = @Password
            //        AND U.ISACTIVE = '1'";
            string sql = @"
                SELECT U.USERID, U.USERNAME, U.USERTYPEID,UC.CompanyId,U.ZoneId, U.STAFFID,U.USERTYPEID FROM USERINFO U , USERCOMPANY UC
                WHERE U.USERID = UC.USERID
                AND U.USERID = :Username AND U.ISACTIVE = '1'";
            return conn.Query<UserInfoModel>(sql, new
                {
                    username = username
                }).ToList();

            //return conn.QueryFirstOrDefault<UserInfoModel>(sql, new
            //{
            //    //Username = username.ToUpper(),
            //    Username = username,
            //    Password = password,
                
            //});
        }
        public UserInfoModel CheckUser(string username)
        {
            using var conn = _databaseService.GetConnection();
            string sql = @" SELECT * from USERINFO where USERID = :Username AND U.ISACTIVE = '1'";
            return conn.Query<UserInfoModel>(sql, new
            {
                username = username
            }).FirstOrDefault();
        }
        public async Task<string> CheckHRISAsync(string username)
        {
            using var conn = _databaseService.GetConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT I.HRIS_CHECK
            FROM FC.T_ACCOUNTS_USERS U JOIN HPS.T_DUSR HU ON U.USER_TEXT = HU.DUSR_USER JOIN HPS.DISTRIBUTOR_MASTER DU ON HU.DUSR_SITE = DU.DIST_ID 
            CROSS JOIN (SELECT * FROM HPS.T_INFO WHERE ROWNUM = 1 ) I WHERE UPPER(U.USER_TEXT) = :Username AND U.ACTIVE = 'Y'";

            cmd.Parameters.Add(new OracleParameter("username", username));

            var result = await cmd.ExecuteScalarAsync();

            return result?.ToString() ?? "N";
        }

        // newly added code 21.7.26
        public async Task<List<CompanyModel>> GetCompany(string username)
        {
            var companies = new List<CompanyModel>();

            using var conn = _databaseService.GetConnection();
            await conn.OpenAsync();   // <-- Open the connection

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
        SELECT C.COMPANYID,
               C.COMPANYNAME
        FROM USERCOMPANY UC
        INNER JOIN COMPANY C
            ON UC.COMPANYID = C.COMPANYID
        WHERE UC.USERID = :Username";

            cmd.Parameters.Add(new OracleParameter("Username", username));

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                companies.Add(new CompanyModel
                {
                    COMPANYID = Convert.ToInt32(reader["COMPANYID"]),
                    COMPANYNAME = reader["COMPANYNAME"]?.ToString()
                });
            }

            return companies;
        }

    }
}
