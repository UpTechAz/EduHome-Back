using WebApplication2.Models;

namespace WebApplication2.Interfaces
{
    public interface ISettingService
    {
        Task<IEnumerable<Setting>> GetSettingsAsync();
    }
}
