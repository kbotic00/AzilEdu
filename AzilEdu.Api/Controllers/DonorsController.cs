using AzilEdu.Api.Data;
using AzilEdu.Shared.DTOs;
using AzilEdu.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzilEdu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DonorsController : ControllerBase
{
    private readonly AzilEduDbContext _db;

    public DonorsController(AzilEduDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<List<DonorDto>> GetAll()
    {
        return await _db.Donors
            .Include(d => d.DonorType)
            .Include(d => d.DonorStatus)
            .Select(d => new DonorDto
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                OrganizationName = d.OrganizationName,
                Email = d.Email,
                Phone = d.Phone,
                Address = d.Address,
                City = d.City,
                Notes = d.Notes,
                CreatedAt = d.CreatedAt,
                DonorTypeId = d.DonorTypeId,
                DonorTypeName = d.DonorType != null ? d.DonorType.Name : string.Empty,
                DonorStatusId = d.DonorStatusId,
                DonorStatusName = d.DonorStatus != null ? d.DonorStatus.Name : string.Empty
            })
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DonorDto>> GetById(int id)
    {
        var d = await _db.Donors
            .Include(x => x.DonorType)
            .Include(x => x.DonorStatus)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (d is null) return NotFound();

        return new DonorDto
        {
            Id = d.Id,
            FirstName = d.FirstName,
            LastName = d.LastName,
            OrganizationName = d.OrganizationName,
            Email = d.Email,
            Phone = d.Phone,
            Address = d.Address,
            City = d.City,
            Notes = d.Notes,
            CreatedAt = d.CreatedAt,
            DonorTypeId = d.DonorTypeId,
            DonorTypeName = d.DonorType?.Name ?? string.Empty,
            DonorStatusId = d.DonorStatusId,
            DonorStatusName = d.DonorStatus?.Name ?? string.Empty
        };
    }

    [HttpPost]
    public async Task<ActionResult<DonorDto>> Create(SaveDonorDto dto)
    {
        var donor = new Donor
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            OrganizationName = dto.OrganizationName,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address,
            City = dto.City,
            Notes = dto.Notes,
            CreatedAt = DateTime.Now,
            DonorTypeId = dto.DonorTypeId,
            DonorStatusId = dto.DonorStatusId
        };

        _db.Donors.Add(donor);
        await _db.SaveChangesAsync();

        return await GetById(donor.Id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, SaveDonorDto dto)
    {
        var donor = await _db.Donors.FindAsync(id);
        if (donor is null) return NotFound();

        donor.FirstName = dto.FirstName;
        donor.LastName = dto.LastName;
        donor.OrganizationName = dto.OrganizationName;
        donor.Email = dto.Email;
        donor.Phone = dto.Phone;
        donor.Address = dto.Address;
        donor.City = dto.City;
        donor.Notes = dto.Notes;
        donor.DonorTypeId = dto.DonorTypeId;
        donor.DonorStatusId = dto.DonorStatusId;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var donor = await _db.Donors.FindAsync(id);
        if (donor is null) return NotFound();

        _db.Donors.Remove(donor);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
