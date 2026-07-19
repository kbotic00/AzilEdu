using AzilEdu.Api.Data;
using AzilEdu.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzilEdu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DonorStatusesController : ControllerBase
{
    private readonly AzilEduDbContext _db;

    public DonorStatusesController(AzilEduDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<List<LookupDto>> GetAll()
    {
        return await _db.DonorStatuses
            .Select(x => new LookupDto { Id = x.Id, Name = x.Name })
            .ToListAsync();
    }
}
