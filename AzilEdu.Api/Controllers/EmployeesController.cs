using AzilEdu.Api.Data;
using AzilEdu.Shared.DTOs;
using AzilEdu.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzilEdu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly AzilEduDbContext _db;

    public EmployeesController(AzilEduDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<List<EmployeeDto>> GetAll()
    {
        return await _db.Employees
            .Include(e => e.EmployeePosition)
            .Include(e => e.EmployeeStatus)
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                EmployeeNumber = e.EmployeeNumber,
                HireDate = e.HireDate,
                Notes = e.Notes,
                EmployeePositionId = e.EmployeePositionId,
                EmployeePositionName = e.EmployeePosition != null ? e.EmployeePosition.Name : string.Empty,
                EmployeeStatusId = e.EmployeeStatusId,
                EmployeeStatusName = e.EmployeeStatus != null ? e.EmployeeStatus.Name : string.Empty
            })
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        var e = await _db.Employees
            .Include(x => x.EmployeePosition)
            .Include(x => x.EmployeeStatus)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (e is null) return NotFound();

        return new EmployeeDto
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            Phone = e.Phone,
            EmployeeNumber = e.EmployeeNumber,
            HireDate = e.HireDate,
            Notes = e.Notes,
            EmployeePositionId = e.EmployeePositionId,
            EmployeePositionName = e.EmployeePosition?.Name ?? string.Empty,
            EmployeeStatusId = e.EmployeeStatusId,
            EmployeeStatusName = e.EmployeeStatus?.Name ?? string.Empty
        };
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Create(SaveEmployeeDto dto)
    {
        var employee = new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            EmployeeNumber = dto.EmployeeNumber,
            HireDate = dto.HireDate,
            Notes = dto.Notes,
            EmployeePositionId = dto.EmployeePositionId,
            EmployeeStatusId = dto.EmployeeStatusId
        };

        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();

        return await GetById(employee.Id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, SaveEmployeeDto dto)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee is null) return NotFound();

        employee.FirstName = dto.FirstName;
        employee.LastName = dto.LastName;
        employee.Email = dto.Email;
        employee.Phone = dto.Phone;
        employee.EmployeeNumber = dto.EmployeeNumber;
        employee.HireDate = dto.HireDate;
        employee.Notes = dto.Notes;
        employee.EmployeePositionId = dto.EmployeePositionId;
        employee.EmployeeStatusId = dto.EmployeeStatusId;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee is null) return NotFound();

        _db.Employees.Remove(employee);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
