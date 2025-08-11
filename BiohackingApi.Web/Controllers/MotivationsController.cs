using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BiohackingApi.Web.Data;
using BiohackingApi.Web.Models;
using BiohackingApi.Web.Dtos;

namespace BiohackingApi.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MotivationsController : ControllerBase
{
    private readonly BiohackingDbContext _context;

    public MotivationsController(BiohackingDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all motivations
    /// </summary>
    /// <returns>List of all motivations</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadMotivationDto>>> GetMotivations()
    {
        var motivations = await _context.Motivations
            .Select(m => new ReadMotivationDto
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                CreatedDate = m.CreatedDate,
                UpdatedDate = m.UpdatedDate
            })
            .ToListAsync();

        return Ok(motivations);
    }

    /// <summary>
    /// Get a specific motivation by ID
    /// </summary>
    /// <param name="id">Motivation ID</param>
    /// <returns>Motivation details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ReadMotivationDto>> GetMotivation(int id)
    {
        var motivation = await _context.Motivations
            .Where(m => m.Id == id)
            .Select(m => new ReadMotivationDto
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                CreatedDate = m.CreatedDate,
                UpdatedDate = m.UpdatedDate
            })
            .FirstOrDefaultAsync();

        if (motivation == null)
        {
            return NotFound($"Motivation with ID {id} not found.");
        }

        return Ok(motivation);
    }

    /// <summary>
    /// Get all biohacks for a specific motivation
    /// </summary>
    /// <param name="id">Motivation ID</param>
    /// <returns>List of biohacks linked to the motivation</returns>
    [HttpGet("{id}/biohacks")]
    public async Task<ActionResult<IEnumerable<ReadBiohackDto>>> GetBiohacksByMotivation(int id)
    {
        var motivationExists = await _context.Motivations.AnyAsync(m => m.Id == id);
        if (!motivationExists)
        {
            return NotFound($"Motivation with ID {id} not found.");
        }

        var biohacks = await _context.MotivationBiohacks
            .Where(mb => mb.MotivationId == id)
            .Select(mb => new ReadBiohackDto
            {
                Id = mb.Biohack.Id,
                Title = mb.Biohack.Title,
                Technique = mb.Biohack.Technique,
                Category = mb.Biohack.Category,
                Difficulty = mb.Biohack.Difficulty,
                TimeRequired = mb.Biohack.TimeRequired,
                Action = mb.Biohack.Action,
                Mechanism = mb.Biohack.Mechanism,
                ResearchStudies = mb.Biohack.ResearchStudies,
                Biology = mb.Biohack.Biology,
                ColorGradient = mb.Biohack.ColorGradient,
                CreatedDate = mb.Biohack.CreatedDate,
                UpdatedDate = mb.Biohack.UpdatedDate
            })
            .ToListAsync();

        return Ok(biohacks);
    }

    /// <summary>
    /// Create a new motivation
    /// </summary>
    /// <param name="createMotivationDto">Motivation creation data</param>
    /// <returns>Created motivation</returns>
    [HttpPost]
    public async Task<ActionResult<ReadMotivationDto>> CreateMotivation(CreateMotivationDto createMotivationDto)
    {
        var motivation = new Motivation
        {
            Title = createMotivationDto.Title,
            Description = createMotivationDto.Description,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _context.Motivations.Add(motivation);
        await _context.SaveChangesAsync();

        var result = new ReadMotivationDto
        {
            Id = motivation.Id,
            Title = motivation.Title,
            Description = motivation.Description,
            CreatedDate = motivation.CreatedDate,
            UpdatedDate = motivation.UpdatedDate
        };

        return CreatedAtAction(nameof(GetMotivation), new { id = motivation.Id }, result);
    }

    /// <summary>
    /// Update an existing motivation
    /// </summary>
    /// <param name="id">Motivation ID</param>
    /// <param name="updateMotivationDto">Motivation update data</param>
    /// <returns>Updated motivation</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ReadMotivationDto>> UpdateMotivation(int id, UpdateMotivationDto updateMotivationDto)
    {
        var motivation = await _context.Motivations.FindAsync(id);
        if (motivation == null)
        {
            return NotFound($"Motivation with ID {id} not found.");
        }

        // Update only provided fields
        if (updateMotivationDto.Title != null)
            motivation.Title = updateMotivationDto.Title;
        if (updateMotivationDto.Description != null)
            motivation.Description = updateMotivationDto.Description;

        motivation.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var result = new ReadMotivationDto
        {
            Id = motivation.Id,
            Title = motivation.Title,
            Description = motivation.Description,
            CreatedDate = motivation.CreatedDate,
            UpdatedDate = motivation.UpdatedDate
        };

        return Ok(result);
    }

    /// <summary>
    /// Delete a motivation
    /// </summary>
    /// <param name="id">Motivation ID</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMotivation(int id)
    {
        var motivation = await _context.Motivations.FindAsync(id);
        if (motivation == null)
        {
            return NotFound($"Motivation with ID {id} not found.");
        }

        _context.Motivations.Remove(motivation);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
