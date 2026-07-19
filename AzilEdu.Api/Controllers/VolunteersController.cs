using AzilEdu.Api.Data;
using AzilEdu.Shared.DTOs;
using AzilEdu.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzilEdu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VolunteersController : ControllerBase
{
    private readonly AzilEduDbContext _db;

    public VolunteersController(AzilEduDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] int? statusId,
        [FromQuery] string? sortBy,
        [FromQuery] bool descending = false)
    {
        var query = _db.Volunteers
            .Include(v => v.VolunteerStatus)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(v =>
                v.FirstName.Contains(search) ||
                v.LastName.Contains(search) ||
                v.Email.Contains(search));

        if (statusId.HasValue)
            query = query.Where(v => v.VolunteerStatusId == statusId.Value);

        query = sortBy switch
        {
            "lastName" => descending ? query.OrderByDescending(v => v.LastName) : query.OrderBy(v => v.LastName),
            "email" => descending ? query.OrderByDescending(v => v.Email) : query.OrderBy(v => v.Email),
            _ => descending ? query.OrderByDescending(v => v.FirstName) : query.OrderBy(v => v.FirstName)
        };

        var result = await query.Select(v => new VolunteerDto
        {
            Id = v.Id,
            FirstName = v.FirstName,
            LastName = v.LastName,
            Email = v.Email,
            Phone = v.Phone,
            Skills = v.Skills,
            AvailableFrom = v.AvailableFrom,
            Notes = v.Notes,
            VolunteerStatusId = v.VolunteerStatusId,
            Status = v.VolunteerStatus!.Name
        }).ToListAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var v = await _db.Volunteers
            .Include(v => v.VolunteerStatus)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (v is null) return NotFound();

        return Ok(new VolunteerDto
        {
            Id = v.Id,
            FirstName = v.FirstName,
            LastName = v.LastName,
            Email = v.Email,
            Phone = v.Phone,
            Skills = v.Skills,
            AvailableFrom = v.AvailableFrom,
            Notes = v.Notes,
            VolunteerStatusId = v.VolunteerStatusId,
            Status = v.VolunteerStatus!.Name
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(SaveVolunteerDto dto)
    {
        var volunteer = new Volunteer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            Skills = dto.Skills,
            AvailableFrom = dto.AvailableFrom,
            Notes = dto.Notes,
            VolunteerStatusId = dto.VolunteerStatusId
        };

        _db.Volunteers.Add(volunteer);
        await _db.SaveChangesAsync();
        return Ok(volunteer.Id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SaveVolunteerDto dto)
    {
        var volunteer = await _db.Volunteers.FindAsync(id);
        if (volunteer is null) return NotFound();

        volunteer.FirstName = dto.FirstName;
        volunteer.LastName = dto.LastName;
        volunteer.Email = dto.Email;
        volunteer.Phone = dto.Phone;
        volunteer.Skills = dto.Skills;
        volunteer.AvailableFrom = dto.AvailableFrom;
        volunteer.Notes = dto.Notes;
        volunteer.VolunteerStatusId = dto.VolunteerStatusId;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var volunteer = await _db.Volunteers.FindAsync(id);
        if (volunteer is null) return NotFound();

        _db.Volunteers.Remove(volunteer);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}