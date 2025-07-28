using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BiohackingApi.Web.Data;
using BiohackingApi.Web.Models;
using BiohackingApi.Web.Dtos;

namespace BiohackingApi.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MotivationBiohacksController : ControllerBase
{
    private readonly BiohackingDbContext _context;

    public MotivationBiohacksController(BiohackingDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all motivation-biohack relationships
    /// </summary>
    /// <returns>List of all motivation-biohack relationships</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadMotivationBiohackDto>>> GetMotivationBiohacks()
    {
        var motivationBiohacks = await _context.MotivationBiohacks
            .Include(mb => mb.Motivation)
            .Include(mb => mb.Biohack)
            .Select(mb => new ReadMotivationBiohackDto
            {
                MotivationId = mb.MotivationId,
                MotivationName = mb.Motivation.Name,
                BiohackId = mb.BiohackId,
                BiohackName = mb.Biohack.Name
            })
            .ToListAsync();

        return Ok(motivationBiohacks);
    }

    /// <summary>
    /// Get a specific motivation-biohack relationship
    /// </summary>
    /// <param name="motivationId">Motivation ID</param>
    /// <param name="biohackId">Biohack ID</param>
    /// <returns>Motivation-biohack relationship details</returns>
    [HttpGet("{motivationId}/{biohackId}")]
    public async Task<ActionResult<ReadMotivationBiohackDto>> GetMotivationBiohack(int motivationId, int biohackId)
    {
        var motivationBiohack = await _context.MotivationBiohacks
            .Include(mb => mb.Motivation)
            .Include(mb => mb.Biohack)
            .Where(mb => mb.MotivationId == motivationId && mb.BiohackId == biohackId)
            .Select(mb => new ReadMotivationBiohackDto
            {
                MotivationId = mb.MotivationId,
                MotivationName = mb.Motivation.Name,
                BiohackId = mb.BiohackId,
                BiohackName = mb.Biohack.Name
            })
            .FirstOrDefaultAsync();

        if (motivationBiohack == null)
        {
            return NotFound($"Motivation-biohack relationship with Motivation ID {motivationId} and Biohack ID {biohackId} not found.");
        }

        return Ok(motivationBiohack);
    }

    /// <summary>
    /// Create a new motivation-biohack relationship
    /// </summary>
    /// <param name="createMotivationBiohackDto">Motivation-biohack creation data</param>
    /// <returns>Created motivation-biohack relationship</returns>
    [HttpPost]
    public async Task<ActionResult<ReadMotivationBiohackDto>> CreateMotivationBiohack(CreateMotivationBiohackDto createMotivationBiohackDto)
    {
        // Validate motivation exists
        var motivationExists = await _context.Motivations.AnyAsync(m => m.Id == createMotivationBiohackDto.MotivationId);
        if (!motivationExists)
        {
            return BadRequest($"Motivation with ID {createMotivationBiohackDto.MotivationId} does not exist.");
        }

        // Validate biohack exists
        var biohackExists = await _context.Biohacks.AnyAsync(b => b.Id == createMotivationBiohackDto.BiohackId);
        if (!biohackExists)
        {
            return BadRequest($"Biohack with ID {createMotivationBiohackDto.BiohackId} does not exist.");
        }

        // Check if relationship already exists
        var existingRelationship = await _context.MotivationBiohacks
            .AnyAsync(mb => mb.MotivationId == createMotivationBiohackDto.MotivationId && mb.BiohackId == createMotivationBiohackDto.BiohackId);

        if (existingRelationship)
        {
            return Conflict($"Relationship between Motivation ID {createMotivationBiohackDto.MotivationId} and Biohack ID {createMotivationBiohackDto.BiohackId} already exists.");
        }

        var motivationBiohack = new MotivationBiohack
        {
            MotivationId = createMotivationBiohackDto.MotivationId,
            BiohackId = createMotivationBiohackDto.BiohackId
        };

        _context.MotivationBiohacks.Add(motivationBiohack);
        await _context.SaveChangesAsync();

        // Load navigation properties for response
        await _context.Entry(motivationBiohack)
            .Reference(mb => mb.Motivation)
            .LoadAsync();
        await _context.Entry(motivationBiohack)
            .Reference(mb => mb.Biohack)
            .LoadAsync();

        var result = new ReadMotivationBiohackDto
        {
            MotivationId = motivationBiohack.MotivationId,
            MotivationName = motivationBiohack.Motivation.Name,
            BiohackId = motivationBiohack.BiohackId,
            BiohackName = motivationBiohack.Biohack.Name
        };

        return CreatedAtAction(nameof(GetMotivationBiohack), 
            new { motivationId = motivationBiohack.MotivationId, biohackId = motivationBiohack.BiohackId }, 
            result);
    }

    /// <summary>
    /// Delete a motivation-biohack relationship
    /// </summary>
    /// <param name="motivationId">Motivation ID</param>
    /// <param name="biohackId">Biohack ID</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{motivationId}/{biohackId}")]
    public async Task<IActionResult> DeleteMotivationBiohack(int motivationId, int biohackId)
    {
        var motivationBiohack = await _context.MotivationBiohacks
            .FirstOrDefaultAsync(mb => mb.MotivationId == motivationId && mb.BiohackId == biohackId);

        if (motivationBiohack == null)
        {
            return NotFound($"Motivation-biohack relationship with Motivation ID {motivationId} and Biohack ID {biohackId} not found.");
        }

        _context.MotivationBiohacks.Remove(motivationBiohack);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Link a motivation to a biohack
    /// </summary>
    /// <param name="linkDto">Link data</param>
    /// <returns>Created motivation-biohack relationship</returns>
    [HttpPost("link")]
    public async Task<ActionResult<ReadMotivationBiohackDto>> LinkMotivationBiohack(LinkMotivationBiohackDto linkDto)
    {
        var createDto = new CreateMotivationBiohackDto
        {
            MotivationId = linkDto.MotivationId,
            BiohackId = linkDto.BiohackId
        };

        return await CreateMotivationBiohack(createDto);
    }

    /// <summary>
    /// Unlink a motivation from a biohack
    /// </summary>
    /// <param name="linkDto">Unlink data</param>
    /// <returns>No content if successful</returns>
    [HttpPost("unlink")]
    public async Task<IActionResult> UnlinkMotivationBiohack(LinkMotivationBiohackDto linkDto)
    {
        return await DeleteMotivationBiohack(linkDto.MotivationId, linkDto.BiohackId);
    }
}
