using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BiohackingApi.Web.Data;
using BiohackingApi.Web.Models;
using BiohackingApi.Web.Dtos;

namespace BiohackingApi.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly BiohackingDbContext _context;

    public UsersController(BiohackingDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>List of all users</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadUserDto>>> GetUsers()
    {
        var users = await _context.Users
            .Include(u => u.Motivation)
            .Select(u => new ReadUserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Provider = u.Provider,
                ExternalId = u.ExternalId,
                SubId = u.SubId,
                MotivationId = u.MotivationId,
                MotivationName = u.Motivation != null ? u.Motivation.Name : null,
                CreatedDate = u.CreatedDate,
                UpdatedDate = u.UpdatedDate
            })
            .ToListAsync();

        return Ok(users);
    }

    /// <summary>
    /// Get a specific user by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ReadUserDto>> GetUser(int id)
    {
        var user = await _context.Users
            .Include(u => u.Motivation)
            .Where(u => u.Id == id)
            .Select(u => new ReadUserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Provider = u.Provider,
                ExternalId = u.ExternalId,
                SubId = u.SubId,
                MotivationId = u.MotivationId,
                MotivationName = u.Motivation != null ? u.Motivation.Name : null,
                CreatedDate = u.CreatedDate,
                UpdatedDate = u.UpdatedDate
            })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound($"User with ID {id} not found.");
        }

        return Ok(user);
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="createUserDto">User creation data</param>
    /// <returns>Created user</returns>
    [HttpPost]
    public async Task<ActionResult<ReadUserDto>> CreateUser(CreateUserDto createUserDto)
    {
        // Validate motivation exists if provided
        if (createUserDto.MotivationId.HasValue)
        {
            var motivationExists = await _context.Motivations
                .AnyAsync(m => m.Id == createUserDto.MotivationId.Value);
            if (!motivationExists)
            {
                return BadRequest($"Motivation with ID {createUserDto.MotivationId} does not exist.");
            }
        }

        var user = new User
        {
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            Email = createUserDto.Email,
            Provider = createUserDto.Provider,
            ExternalId = createUserDto.ExternalId,
            SubId = createUserDto.SubId,
            MotivationId = createUserDto.MotivationId,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _context.Users.Add(user);
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict("A user with this email already exists.");
        }

        // Load the motivation for response
        await _context.Entry(user)
            .Reference(u => u.Motivation)
            .LoadAsync();

        var result = new ReadUserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Provider = user.Provider,
            ExternalId = user.ExternalId,
            SubId = user.SubId,
            MotivationId = user.MotivationId,
            MotivationName = user.Motivation?.Name,
            CreatedDate = user.CreatedDate,
            UpdatedDate = user.UpdatedDate
        };

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, result);
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="updateUserDto">User update data</param>
    /// <returns>Updated user</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ReadUserDto>> UpdateUser(int id, UpdateUserDto updateUserDto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound($"User with ID {id} not found.");
        }

        // Validate motivation exists if provided
        if (updateUserDto.MotivationId.HasValue)
        {
            var motivationExists = await _context.Motivations
                .AnyAsync(m => m.Id == updateUserDto.MotivationId.Value);
            if (!motivationExists)
            {
                return BadRequest($"Motivation with ID {updateUserDto.MotivationId} does not exist.");
            }
        }

        // Update only provided fields
        if (updateUserDto.FirstName != null)
            user.FirstName = updateUserDto.FirstName;
        if (updateUserDto.LastName != null)
            user.LastName = updateUserDto.LastName;
        if (updateUserDto.Email != null)
            user.Email = updateUserDto.Email;
        if (updateUserDto.Provider != null)
            user.Provider = updateUserDto.Provider;
        if (updateUserDto.ExternalId != null)
            user.ExternalId = updateUserDto.ExternalId;
        if (updateUserDto.SubId != null)
            user.SubId = updateUserDto.SubId;
        if (updateUserDto.MotivationId.HasValue)
            user.MotivationId = updateUserDto.MotivationId;

        user.UpdatedDate = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict("A user with this email already exists.");
        }

        // Load the motivation for response
        await _context.Entry(user)
            .Reference(u => u.Motivation)
            .LoadAsync();

        var result = new ReadUserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Provider = user.Provider,
            ExternalId = user.ExternalId,
            SubId = user.SubId,
            MotivationId = user.MotivationId,
            MotivationName = user.Motivation?.Name,
            CreatedDate = user.CreatedDate,
            UpdatedDate = user.UpdatedDate
        };

        return Ok(result);
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound($"User with ID {id} not found.");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Link a user to a motivation
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="motivationId">Motivation ID to link</param>
    /// <returns>Updated user</returns>
    [HttpPost("{id}/link-motivation/{motivationId}")]
    public async Task<ActionResult<ReadUserDto>> LinkUserToMotivation(int id, int motivationId)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound($"User with ID {id} not found.");
        }

        // Validate motivation exists
        var motivationExists = await _context.Motivations.AnyAsync(m => m.Id == motivationId);
        if (!motivationExists)
        {
            return BadRequest($"Motivation with ID {motivationId} does not exist.");
        }

        user.MotivationId = motivationId;
        user.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Load the motivation for response
        await _context.Entry(user)
            .Reference(u => u.Motivation)
            .LoadAsync();

        var result = new ReadUserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Provider = user.Provider,
            ExternalId = user.ExternalId,
            SubId = user.SubId,
            MotivationId = user.MotivationId,
            MotivationName = user.Motivation?.Name,
            CreatedDate = user.CreatedDate,
            UpdatedDate = user.UpdatedDate
        };

        return Ok(result);
    }

    /// <summary>
    /// Unlink a user from their motivation
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>Updated user</returns>
    [HttpPost("{id}/unlink-motivation")]
    public async Task<ActionResult<ReadUserDto>> UnlinkUserFromMotivation(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound($"User with ID {id} not found.");
        }

        user.MotivationId = null;
        user.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var result = new ReadUserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Provider = user.Provider,
            ExternalId = user.ExternalId,
            SubId = user.SubId,
            MotivationId = user.MotivationId,
            MotivationName = null,
            CreatedDate = user.CreatedDate,
            UpdatedDate = user.UpdatedDate
        };

        return Ok(result);
    }

    /// <summary>
    /// Link a user to a motivation using request body
    /// </summary>
    /// <param name="linkDto">Link data containing user and motivation IDs</param>
    /// <returns>Updated user</returns>
    [HttpPost("link-motivation")]
    public async Task<ActionResult<ReadUserDto>> LinkUserToMotivationByBody(LinkUserMotivationDto linkDto)
    {
        return await LinkUserToMotivation(linkDto.UserId, linkDto.MotivationId);
    }

    /// <summary>
    /// Unlink a user from motivation using request body
    /// </summary>
    /// <param name="unlinkDto">Unlink data containing user ID</param>
    /// <returns>Updated user</returns>
    [HttpPost("unlink-motivation")]
    public async Task<ActionResult<ReadUserDto>> UnlinkUserFromMotivationByBody(UnlinkUserMotivationDto unlinkDto)
    {
        return await UnlinkUserFromMotivation(unlinkDto.UserId);
    }
}
