using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Helpers
{
    public class SettingService : ISettingService
    {
        private readonly AppDbContext _dbContext;

        public SettingService(AppDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public async Task<IEnumerable<Setting>> GetSettingsAsync()
        {
            return await _dbContext.Settings.ToListAsync();
        }
    }
}
