using BiohackingApi.Web.Models;

namespace BiohackingApi.Web.Repositories;

public interface IMotivationBiohackRepository
{
    Task<IEnumerable<MotivationBiohack>> GetAllAsync();
    Task<MotivationBiohack?> GetByIdAsync(int motivationId, int biohackId);
    Task<MotivationBiohack> CreateAsync(MotivationBiohack entity);
    Task<bool> DeleteAsync(int motivationId, int biohackId);
    Task<bool> ExistsAsync(int motivationId, int biohackId);
    Task<IEnumerable<Biohack>> GetBiohacksByMotivationIdAsync(int motivationId);
}
