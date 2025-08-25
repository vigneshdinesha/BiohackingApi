using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BiohackingApi.Web.Data;
using BiohackingApi.Web.Models;
using BiohackingApi.Web.Dtos;

namespace BiohackingApi.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JournalsController : ControllerBase
{
    private readonly BiohackingDbContext _context;

    public JournalsController(BiohackingDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all journals
    /// </summary>
    /// <returns>List of all journals</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadJournalDto>>> GetJournals()
    {
        var journals = await _context.Journals
            .Include(j => j.User)
            .Include(j => j.Biohack)
            .Select(j => new ReadJournalDto
            {
                Id = j.Id,
                UserId = j.UserId,
                UserFirstName = j.User.FirstName,
                UserLastName = j.User.LastName,
                BiohackId = j.BiohackId,
                BiohackName = j.Biohack.Title,
                Notes = j.Notes,
                Rating = j.Rating,
                DateTime = j.DateTime,
                CreatedDate = j.CreatedDate,
                UpdatedDate = j.UpdatedDate
            })
            .ToListAsync();

        return Ok(journals);
    }

    /// <summary>
    /// Get a specific journal by ID
    /// </summary>
    /// <param name="id">Journal ID</param>
    /// <returns>Journal details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ReadJournalDto>> GetJournal(int id)
    {
        var journal = await _context.Journals
            .Include(j => j.User)
            .Include(j => j.Biohack)
            .Where(j => j.Id == id)
            .Select(j => new ReadJournalDto
            {
                Id = j.Id,
                UserId = j.UserId,
                UserFirstName = j.User.FirstName,
                UserLastName = j.User.LastName,
                BiohackId = j.BiohackId,
                BiohackName = j.Biohack.Title,
                Notes = j.Notes,
                Rating = j.Rating,
                DateTime = j.DateTime,
                CreatedDate = j.CreatedDate,
                UpdatedDate = j.UpdatedDate
            })
            .FirstOrDefaultAsync();

        if (journal == null)
        {
            return NotFound($"Journal with ID {id} not found.");
        }

        return Ok(journal);
    }

    /// <summary>
    /// Get all journals for a specific user and biohack combination
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="biohackId">Biohack ID</param>
    /// <returns>List of journals for the specified user and biohack</returns>
    [HttpGet("user/{userId}/biohack/{biohackId}")]
    public async Task<ActionResult<IEnumerable<ReadJournalDto>>> GetJournalsByUserAndBiohack(int userId, int biohackId)
    {
        // Validate user exists
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        // Validate biohack exists
        var biohackExists = await _context.Biohacks.AnyAsync(b => b.Id == biohackId);
        if (!biohackExists)
        {
            return NotFound($"Biohack with ID {biohackId} not found.");
        }

        var journals = await _context.Journals
            .Include(j => j.User)
            .Include(j => j.Biohack)
            .Where(j => j.UserId == userId && j.BiohackId == biohackId)
            .Select(j => new ReadJournalDto
            {
                Id = j.Id,
                UserId = j.UserId,
                UserFirstName = j.User.FirstName,
                UserLastName = j.User.LastName,
                BiohackId = j.BiohackId,
                BiohackName = j.Biohack.Title,
                Notes = j.Notes,
                Rating = j.Rating,
                DateTime = j.DateTime,
                CreatedDate = j.CreatedDate,
                UpdatedDate = j.UpdatedDate
            })
            .OrderByDescending(j => j.DateTime)
            .ToListAsync();

        return Ok(journals);
    }

    /// <summary>
    /// Create a new journal
    /// </summary>
    /// <param name="createJournalDto">Journal creation data</param>
    /// <returns>Created journal</returns>
    [HttpPost]
    public async Task<ActionResult<ReadJournalDto>> CreateJournal(CreateJournalDto createJournalDto)
    {
        // Validate user exists
        var userExists = await _context.Users.AnyAsync(u => u.Id == createJournalDto.UserId);
        if (!userExists)
        {
            return BadRequest($"User with ID {createJournalDto.UserId} does not exist.");
        }

        // Validate biohack exists
        var biohackExists = await _context.Biohacks.AnyAsync(b => b.Id == createJournalDto.BiohackId);
        if (!biohackExists)
        {
            return BadRequest($"Biohack with ID {createJournalDto.BiohackId} does not exist.");
        }

        // Validate rating if provided
        if (createJournalDto.Rating.HasValue && (createJournalDto.Rating < 1 || createJournalDto.Rating > 10))
        {
            return BadRequest("Rating must be between 1 and 10.");
        }

        var journal = new Journal
        {
            UserId = createJournalDto.UserId,
            BiohackId = createJournalDto.BiohackId,
            Notes = createJournalDto.Notes,
            Rating = createJournalDto.Rating,
            DateTime = createJournalDto.DateTime,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _context.Journals.Add(journal);
        await _context.SaveChangesAsync();

        // Load navigation properties for response
        await _context.Entry(journal)
            .Reference(j => j.User)
            .LoadAsync();
        await _context.Entry(journal)
            .Reference(j => j.Biohack)
            .LoadAsync();

        var result = new ReadJournalDto
        {
            Id = journal.Id,
            UserId = journal.UserId,
            UserFirstName = journal.User.FirstName,
            UserLastName = journal.User.LastName,
            BiohackId = journal.BiohackId,
            BiohackName = journal.Biohack.Title,
            Notes = journal.Notes,
            Rating = journal.Rating,
            DateTime = journal.DateTime,
            CreatedDate = journal.CreatedDate,
            UpdatedDate = journal.UpdatedDate
        };

        return CreatedAtAction(nameof(GetJournal), new { id = journal.Id }, result);
    }

    /// <summary>
    /// Update an existing journal
    /// </summary>
    /// <param name="id">Journal ID</param>
    /// <param name="updateJournalDto">Journal update data</param>
    /// <returns>Updated journal</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ReadJournalDto>> UpdateJournal(int id, UpdateJournalDto updateJournalDto)
    {
        var journal = await _context.Journals
            .Include(j => j.User)
            .Include(j => j.Biohack)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (journal == null)
        {
            return NotFound($"Journal with ID {id} not found.");
        }

        // Validate user exists if provided
        if (updateJournalDto.UserId.HasValue)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == updateJournalDto.UserId.Value);
            if (!userExists)
            {
                return BadRequest($"User with ID {updateJournalDto.UserId} does not exist.");
            }
        }

        // Validate biohack exists if provided
        if (updateJournalDto.BiohackId.HasValue)
        {
            var biohackExists = await _context.Biohacks.AnyAsync(b => b.Id == updateJournalDto.BiohackId.Value);
            if (!biohackExists)
            {
                return BadRequest($"Biohack with ID {updateJournalDto.BiohackId} does not exist.");
            }
        }

        // Validate rating if provided
        if (updateJournalDto.Rating.HasValue && (updateJournalDto.Rating < 1 || updateJournalDto.Rating > 10))
        {
            return BadRequest("Rating must be between 1 and 10.");
        }

        // Update only provided fields
        if (updateJournalDto.UserId.HasValue)
            journal.UserId = updateJournalDto.UserId.Value;
        if (updateJournalDto.BiohackId.HasValue)
            journal.BiohackId = updateJournalDto.BiohackId.Value;
        if (updateJournalDto.Notes != null)
            journal.Notes = updateJournalDto.Notes;
        if (updateJournalDto.Rating.HasValue)
            journal.Rating = updateJournalDto.Rating;
        if (updateJournalDto.DateTime.HasValue)
            journal.DateTime = updateJournalDto.DateTime.Value;

        journal.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Reload navigation properties if they changed
        if (updateJournalDto.UserId.HasValue || updateJournalDto.BiohackId.HasValue)
        {
            await _context.Entry(journal)
                .Reference(j => j.User)
                .LoadAsync();
            await _context.Entry(journal)
                .Reference(j => j.Biohack)
                .LoadAsync();
        }

        var result = new ReadJournalDto
        {
            Id = journal.Id,
            UserId = journal.UserId,
            UserFirstName = journal.User.FirstName,
            UserLastName = journal.User.LastName,
            BiohackId = journal.BiohackId,
            BiohackName = journal.Biohack.Title,
            Notes = journal.Notes,
            Rating = journal.Rating,
            DateTime = journal.DateTime,
            CreatedDate = journal.CreatedDate,
            UpdatedDate = journal.UpdatedDate
        };

        return Ok(result);
    }

    /// <summary>
    /// Delete a journal
    /// </summary>
    /// <param name="id">Journal ID</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJournal(int id)
    {
        var journal = await _context.Journals.FindAsync(id);
        if (journal == null)
        {
            return NotFound($"Journal with ID {id} not found.");
        }

        _context.Journals.Remove(journal);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
