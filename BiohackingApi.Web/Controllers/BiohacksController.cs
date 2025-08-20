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
                Title = b.Title,
                Technique = b.Technique,
                Category = b.Category,
                Difficulty = b.Difficulty,
                TimeRequired = b.TimeRequired,
                Action = b.Action,
                Mechanism = b.Mechanism,
                ResearchStudies = b.ResearchStudies,
                Biology = b.Biology,
                ColorGradient = b.ColorGradient,
                CreatedDate = b.CreatedDate,
                UpdatedDate = b.UpdatedDate
            })
            .ToListAsync();

        return Ok(biohacks);
    }

    /// <summary>
    /// Get filtered biohacks based on various criteria
    /// </summary>
    /// <param name="filter">Filter criteria</param>
    /// <returns>Filtered list of biohacks</returns>
    [HttpPost("filter")]
    public async Task<ActionResult<IEnumerable<ReadBiohackDto>>> GetFilteredBiohacks([FromBody] BiohackFilterDto filter)
    {
        var query = _context.Biohacks.AsQueryable();

        // Filter by category if specified
        if (filter.Category.HasValue)
        {
            query = query.Where(b => b.Category == filter.Category.Value);
        }

        // Filter by technique if specified
        if (!string.IsNullOrWhiteSpace(filter.Technique))
        {
            query = query.Where(b => b.Technique != null && b.Technique.ToLower().Contains(filter.Technique.ToLower()));
        }

        // Filter by difficulty if specified
        if (!string.IsNullOrWhiteSpace(filter.Difficulty))
        {
            query = query.Where(b => b.Difficulty != null && b.Difficulty.ToLower().Contains(filter.Difficulty.ToLower()));
        }

        // Filter by time required if specified
        if (!string.IsNullOrWhiteSpace(filter.TimeRequired))
        {
            query = query.Where(b => b.TimeRequired != null && b.TimeRequired.ToLower().Contains(filter.TimeRequired.ToLower()));
        }

        // Search in title, mechanism, or biology if search term is specified
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchLower = filter.SearchTerm.ToLower();
            query = query.Where(b => 
                b.Title.ToLower().Contains(searchLower) ||
                (b.Mechanism != null && b.Mechanism.ToLower().Contains(searchLower)) ||
                (b.Biology != null && b.Biology.ToLower().Contains(searchLower)));
        }

        var biohacks = await query
            .Select(b => new ReadBiohackDto
            {
                Id = b.Id,
                Title = b.Title,
                Technique = b.Technique,
                Category = b.Category,
                Difficulty = b.Difficulty,
                TimeRequired = b.TimeRequired,
                Action = b.Action,
                Mechanism = b.Mechanism,
                ResearchStudies = b.ResearchStudies,
                Biology = b.Biology,
                ColorGradient = b.ColorGradient,
                CreatedDate = b.CreatedDate,
                UpdatedDate = b.UpdatedDate
            })
            .ToListAsync();

        return Ok(biohacks);
    }

    /// <summary>
    /// Get biohacks by category (simple filter endpoint)
    /// </summary>
    /// <param name="category">Category to filter by</param>
    /// <returns>List of biohacks in the specified category</returns>
    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<ReadBiohackDto>>> GetBiohacksByCategory(BiohackCategory category)
    {
        var biohacks = await _context.Biohacks
            .Where(b => b.Category == category)
            .Select(b => new ReadBiohackDto
            {
                Id = b.Id,
                Title = b.Title,
                Technique = b.Technique,
                Category = b.Category,
                Difficulty = b.Difficulty,
                TimeRequired = b.TimeRequired,
                Action = b.Action,
                Mechanism = b.Mechanism,
                ResearchStudies = b.ResearchStudies,
                Biology = b.Biology,
                ColorGradient = b.ColorGradient,
                CreatedDate = b.CreatedDate,
                UpdatedDate = b.UpdatedDate
            })
            .ToListAsync();

        return Ok(biohacks);
    }

    /// <summary>
    /// Get biohacks by technique
    /// </summary>
    /// <param name="technique">Technique to filter by</param>
    /// <returns>List of biohacks with the specified technique</returns>
    [HttpGet("technique/{technique}")]
    public async Task<ActionResult<IEnumerable<ReadBiohackDto>>> GetBiohacksByTechnique(string technique)
    {
        var biohacks = await _context.Biohacks
            .Where(b => b.Technique != null && b.Technique.ToLower().Contains(technique.ToLower()))
            .Select(b => new ReadBiohackDto
            {
                Id = b.Id,
                Title = b.Title,
                Technique = b.Technique,
                Category = b.Category,
                Difficulty = b.Difficulty,
                TimeRequired = b.TimeRequired,
                Action = b.Action,
                Mechanism = b.Mechanism,
                ResearchStudies = b.ResearchStudies,
                Biology = b.Biology,
                ColorGradient = b.ColorGradient,
                CreatedDate = b.CreatedDate,
                UpdatedDate = b.UpdatedDate
            })
            .ToListAsync();

        return Ok(biohacks);
    }

    /// <summary>
    /// Get all available biohack categories
    /// </summary>
    /// <returns>List of all available categories</returns>
    [HttpGet("categories")]
    public ActionResult<IEnumerable<object>> GetCategories()
    {
        var categories = Enum.GetValues<BiohackCategory>()
            .Select(c => new { 
                Value = (int)c, 
                Name = c.ToString(),
                DisplayName = GetCategoryDisplayName(c)
            });

        return Ok(categories);
    }

    private static string GetCategoryDisplayName(BiohackCategory category)
    {
        return category switch
        {
            BiohackCategory.WellnessAndBalance => "Wellness & Balance",
            BiohackCategory.PerformanceAndProductivity => "Performance & Productivity",
            BiohackCategory.FitnessAndPhysicalVitality => "Fitness & Physical Vitality",
            BiohackCategory.TransformationAndSelfDiscovery => "Transformation & Self-Discovery",
            BiohackCategory.SocialGrowthAndConnection => "Social Growth & Connection",
            BiohackCategory.TimeManagement => "Time Management",
            BiohackCategory.NutritionAndSupplementation => "Nutrition & Supplementation",
            BiohackCategory.SleepOptimization => "Sleep Optimization",
            BiohackCategory.StressManagement => "Stress Management",
            BiohackCategory.CognitiveEnhancement => "Cognitive Enhancement",
            _ => category.ToString()
        };
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
                Title = b.Title,
                Technique = b.Technique,
                Category = b.Category,
                Difficulty = b.Difficulty,
                TimeRequired = b.TimeRequired,
                Action = b.Action,
                Mechanism = b.Mechanism,
                ResearchStudies = b.ResearchStudies,
                Biology = b.Biology,
                ColorGradient = b.ColorGradient,
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
            Title = createBiohackDto.Title,
            Technique = createBiohackDto.Technique,
            Category = createBiohackDto.Category,
            Difficulty = createBiohackDto.Difficulty,
            TimeRequired = createBiohackDto.TimeRequired,
            Action = createBiohackDto.Action,
            Mechanism = createBiohackDto.Mechanism,
            ResearchStudies = createBiohackDto.ResearchStudies,
            Biology = createBiohackDto.Biology,
            ColorGradient = createBiohackDto.ColorGradient,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _context.Biohacks.Add(biohack);
        await _context.SaveChangesAsync();

        var result = new ReadBiohackDto
        {
            Id = biohack.Id,
            Title = biohack.Title,
            Technique = biohack.Technique,
            Category = biohack.Category,
            Difficulty = biohack.Difficulty,
            TimeRequired = biohack.TimeRequired,
            Action = biohack.Action,
            Mechanism = biohack.Mechanism,
            ResearchStudies = biohack.ResearchStudies,
            Biology = biohack.Biology,
            ColorGradient = biohack.ColorGradient,
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
        if (updateBiohackDto.Title != null)
            biohack.Title = updateBiohackDto.Title;
        if (updateBiohackDto.Technique != null)
            biohack.Technique = updateBiohackDto.Technique;
        if (updateBiohackDto.Category != null)
            biohack.Category = updateBiohackDto.Category;
        if (updateBiohackDto.Difficulty != null)
            biohack.Difficulty = updateBiohackDto.Difficulty;
        if (updateBiohackDto.TimeRequired != null)
            biohack.TimeRequired = updateBiohackDto.TimeRequired;
        if (updateBiohackDto.Action != null)
            biohack.Action = updateBiohackDto.Action;
        if (updateBiohackDto.Mechanism != null)
            biohack.Mechanism = updateBiohackDto.Mechanism;
        if (updateBiohackDto.ResearchStudies != null)
            biohack.ResearchStudies = updateBiohackDto.ResearchStudies;
        if (updateBiohackDto.Biology != null)
            biohack.Biology = updateBiohackDto.Biology;
        if (updateBiohackDto.ColorGradient != null)
            biohack.ColorGradient = updateBiohackDto.ColorGradient;

        biohack.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var result = new ReadBiohackDto
        {
            Id = biohack.Id,
            Title = biohack.Title,
            Technique = biohack.Technique,
            Category = biohack.Category,
            Difficulty = biohack.Difficulty,
            TimeRequired = biohack.TimeRequired,
            Action = biohack.Action,
            Mechanism = biohack.Mechanism,
            ResearchStudies = biohack.ResearchStudies,
            Biology = biohack.Biology,
            ColorGradient = biohack.ColorGradient,
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
