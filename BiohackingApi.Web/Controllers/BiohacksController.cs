using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BiohackingApi.Web.Data;
using BiohackingApi.Web.Models;
using BiohackingApi.Web.Dtos;

namespace BiohackingApi.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BiohacksController : ControllerBase
{
    private readonly BiohackingDbContext _context;

    public BiohacksController(BiohackingDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all biohacks
    /// </summary>
    /// <returns>List of all biohacks</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadBiohackDto>>> GetBiohacks()
    {
        var biohacks = await _context.Biohacks
            .Select(b => new ReadBiohackDto
            {
                Id = b.Id,
                Name = b.Name,
                InfoSections = b.InfoSectionsJson,
                CreatedDate = b.CreatedDate,
                UpdatedDate = b.UpdatedDate
            })
            .ToListAsync();

        return Ok(biohacks);
    }

    /// <summary>
    /// Get a specific biohack by ID
    /// </summary>
    /// <param name="id">Biohack ID</param>
    /// <returns>Biohack details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ReadBiohackDto>> GetBiohack(int id)
    {
        var biohack = await _context.Biohacks
            .Where(b => b.Id == id)
            .Select(b => new ReadBiohackDto
            {
                Id = b.Id,
                Name = b.Name,
                InfoSections = b.InfoSectionsJson,
                CreatedDate = b.CreatedDate,
                UpdatedDate = b.UpdatedDate
            })
            .FirstOrDefaultAsync();

        if (biohack == null)
        {
            return NotFound($"Biohack with ID {id} not found.");
        }

        return Ok(biohack);
    }

    /// <summary>
    /// Create a new biohack
    /// </summary>
    /// <param name="createBiohackDto">Biohack creation data</param>
    /// <returns>Created biohack</returns>
    [HttpPost]
    public async Task<ActionResult<ReadBiohackDto>> CreateBiohack(CreateBiohackDto createBiohackDto)
    {
        var biohack = new Biohack
        {
            Name = createBiohackDto.Name,
            InfoSectionsJson = createBiohackDto.InfoSections,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _context.Biohacks.Add(biohack);
        await _context.SaveChangesAsync();

        var result = new ReadBiohackDto
        {
            Id = biohack.Id,
            Name = biohack.Name,
            InfoSections = biohack.InfoSectionsJson,
            CreatedDate = biohack.CreatedDate,
            UpdatedDate = biohack.UpdatedDate
        };

        return CreatedAtAction(nameof(GetBiohack), new { id = biohack.Id }, result);
    }

    /// <summary>
    /// Update an existing biohack
    /// </summary>
    /// <param name="id">Biohack ID</param>
    /// <param name="updateBiohackDto">Biohack update data</param>
    /// <returns>Updated biohack</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ReadBiohackDto>> UpdateBiohack(int id, UpdateBiohackDto updateBiohackDto)
    {
        var biohack = await _context.Biohacks.FindAsync(id);
        if (biohack == null)
        {
            return NotFound($"Biohack with ID {id} not found.");
        }

        // Update only provided fields
        if (updateBiohackDto.Name != null)
            biohack.Name = updateBiohackDto.Name;
        
        if (updateBiohackDto.InfoSections != null)
            biohack.InfoSectionsJson = updateBiohackDto.InfoSections;

        biohack.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var result = new ReadBiohackDto
        {
            Id = biohack.Id,
            Name = biohack.Name,
            InfoSections = biohack.InfoSectionsJson,
            CreatedDate = biohack.CreatedDate,
            UpdatedDate = biohack.UpdatedDate
        };

        return Ok(result);
    }

    /// <summary>
    /// Delete a biohack
    /// </summary>
    /// <param name="id">Biohack ID</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBiohack(int id)
    {
        var biohack = await _context.Biohacks.FindAsync(id);
        if (biohack == null)
        {
            return NotFound($"Biohack with ID {id} not found.");
        }

        _context.Biohacks.Remove(biohack);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
