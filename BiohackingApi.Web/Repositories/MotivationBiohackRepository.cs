using Microsoft.EntityFrameworkCore;
using BiohackingApi.Web.Data;
using BiohackingApi.Web.Models;

namespace BiohackingApi.Web.Repositories;

public class MotivationBiohackRepository : IMotivationBiohackRepository
{
    private readonly BiohackingDbContext _context;

    public MotivationBiohackRepository(BiohackingDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MotivationBiohack>> GetAllAsync()
    {
        return await _context.MotivationBiohacks
            .Include(mb => mb.Motivation)
            .Include(mb => mb.Biohack)
            .ToListAsync();
    }

    public async Task<MotivationBiohack?> GetByIdAsync(int motivationId, int biohackId)
    {
        return await _context.MotivationBiohacks
            .Include(mb => mb.Motivation)
            .Include(mb => mb.Biohack)
            .FirstOrDefaultAsync(mb => mb.MotivationId == motivationId && mb.BiohackId == biohackId);
    }

    public async Task<MotivationBiohack> CreateAsync(MotivationBiohack entity)
    {
        _context.MotivationBiohacks.Add(entity);
        await _context.SaveChangesAsync();
        
        // Load navigation properties
        await _context.Entry(entity)
            .Reference(mb => mb.Motivation)
            .LoadAsync();
        await _context.Entry(entity)
            .Reference(mb => mb.Biohack)
            .LoadAsync();
            
        return entity;
    }

    public async Task<bool> DeleteAsync(int motivationId, int biohackId)
    {
        var entity = await _context.MotivationBiohacks
            .FirstOrDefaultAsync(mb => mb.MotivationId == motivationId && mb.BiohackId == biohackId);
        
        if (entity == null)
            return false;

        _context.MotivationBiohacks.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int motivationId, int biohackId)
    {
        return await _context.MotivationBiohacks
            .AnyAsync(mb => mb.MotivationId == motivationId && mb.BiohackId == biohackId);
    }

    public async Task<IEnumerable<Biohack>> GetBiohacksByMotivationIdAsync(int motivationId)
    {
        return await _context.MotivationBiohacks
            .Where(mb => mb.MotivationId == motivationId)
            .Select(mb => mb.Biohack)
            .ToListAsync();
    }
}
