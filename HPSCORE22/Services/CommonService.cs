using Microsoft.EntityFrameworkCore;
using QCMS.Models;
using QCMS.Repositories;

namespace QCMS.Services
{
    public class CommonService
    {
        private readonly CommonRepository _repo;

        public CommonService(CommonRepository repo)
        {
            _repo = repo;
        }

        public async Task<string> GenerateIdAsync(string tableName)
        {
            return await _repo.GenerateIdAsync(tableName);
        }
    }
}