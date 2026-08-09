using Oracle.ManagedDataAccess.Client;
using QCMS.Models;
using RetailCare.Models;
using System.Data;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RetailCare.Common
{
    public interface ICommonMethod
    {
        public UserInfoModel GetUser();
    }
    public interface IReportingMethods
    {
        byte[] Export<T>(IEnumerable<T> data);
    }
    public class SessionHelper: ICommonMethod
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public UserInfoModel GetUser()
        {
            var session = _httpContextAccessor.HttpContext.Session;

            return new UserInfoModel
            {
                USERID = session.GetString("USERID"),
                USERNAME = session.GetString("USERNAME"),
                COMPANYID = session.GetInt32("CompanyID") ?? 0,
                ZONEID = session.GetInt32("ZoneID") ?? 0,
                STAFFID = session.GetInt32("StaffID") ?? 0,
                OUTLETNAME = session.GetString("EMPLOYEE_NAME"),
                CONTACTNO = session.GetString("CONTACT"),
                ADDRESS = session.GetString("ADDRESS"),
                EMPLOYEE_CODE= session.GetString("EMPLOYEE_CODE"),
                EMPLOYEE_NAME= session.GetString("EMPLOYEE_NAME"),
                CONTACT = session.GetString("CONTACT") ?? "",
                BUSINESS_UNIT = session.GetString("BUSINESS_UNIT") ?? ""


                // USERTYPEID = session.GetString("USERTYPEID")
            };
        }
    }
    public class ExtractData
    {
        public static List<T> Convert<T>(DataTable table) where T : new()
        {
            var list = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                T obj = new T();

                foreach (var prop in typeof(T).GetProperties())
                {
                    if (!table.Columns.Contains(prop.Name) || row[prop.Name] == DBNull.Value)
                        continue;

                    object value = row[prop.Name];
                    Type targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                    if (value.GetType() != targetType)
                    {
                        value = System.Convert.ChangeType(value, targetType);
                    }

                    prop.SetValue(obj, value);
                }

                list.Add(obj);
            }

            return list;
        }
    }
    public class ReportingMethods: IReportingMethods
    {
        public byte[] Export<T>(IEnumerable<T> data)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();

            var ws = package.Workbook.Worksheets.Add("Report");

            var properties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .ToList();

            // Header
            for (int col = 0; col < properties.Count; col++)
            {
                var prop = properties[col];

                var display = prop.GetCustomAttribute<DisplayAttribute>();

                ws.Cells[1, col + 1].Value =
                    display?.Name ?? prop.Name;

                ws.Cells[1, col + 1].Style.Font.Bold = true;
            }

            // Data
            int row = 2;

            foreach (var item in data)
            {
                for (int col = 0; col < properties.Count; col++)
                {
                    var value = properties[col].GetValue(item);

                    ws.Cells[row, col + 1].Value = value;
                }

                row++;
            }

            ws.Cells.AutoFitColumns();

            return package.GetAsByteArray();
        }
    }
}
