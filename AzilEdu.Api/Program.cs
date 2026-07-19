using AzilEdu.Api.Data;
using Microsoft.EntityFrameworkCore;
using AzilEdu.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AzilEduDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AzilEduDbContext>();

    await db.Database.MigrateAsync();

    if (!await db.HousingUnits.AnyAsync())
    {
        db.HousingUnits.AddRange(
            new HousingUnit
            {
                Name = "Boks A1",
                UnitType = "boks",
                Capacity = 2,
                Occupied = 2,
                LastCleanedAt = DateTime.Now.AddDays(-1),
                IsActive = true,
                ImageUrl = "/images/housing-units/box-1.webp",
                Note = "Puni kapacitet"
            },
            new HousingUnit
            {
                Name = "Boks A2",
                UnitType = "boks",
                Capacity = 3,
                Occupied = 1,
                LastCleanedAt = DateTime.Now.AddDays(-3),
                IsActive = true,
                ImageUrl = "/images/housing-units/box-2.webp",
                Note = "Jedan pas smješten"
            },
            new HousingUnit
            {
                Name = "Karantena A3",
                UnitType = "karantena",
                Capacity = 1,
                Occupied = 1,
                LastCleanedAt = null,
                IsActive = true,
                ImageUrl = "/images/housing-units/quarantine.webp",
                Note = "Pas u promatranju"
            },
            new HousingUnit
            {
                Name = "Boks A4",
                UnitType = "boks",
                Capacity = 2,
                Occupied = 0,
                LastCleanedAt = DateTime.Now.AddDays(-10),
                IsActive = false,
                ImageUrl = "/images/housing-units/inactive-unit.webp",
                Note = "Izvan uporabe zbog održavanja"
            },
            new HousingUnit
            {
                Name = "Soba za mačke A5",
                UnitType = "macke",
                Capacity = 4,
                Occupied = 2,
                LastCleanedAt = DateTime.Now.AddHours(-12),
                IsActive = true,
                ImageUrl = "/images/housing-units/cat-room.webp",
                Note = "Prostor za mačke"
            },
            new HousingUnit
            {
                Name = "Dvorišni prostor A6",
                UnitType = "dvoriste",
                Capacity = 3,
                Occupied = 0,
                LastCleanedAt = DateTime.Now.AddDays(-5),
                IsActive = true,
                ImageUrl = "/images/housing-units/yard-unit.webp",
                Note = "Vanjski prostor za životinje"
            }
        );

        await db.SaveChangesAsync();
    }

    if (!await db.VolunteerStatuses.AnyAsync())
    {
        db.VolunteerStatuses.AddRange(
            new VolunteerStatus { Name = "Novi" },
            new VolunteerStatus { Name = "Aktivan" },
            new VolunteerStatus { Name = "Privremeno nedostupan" },
            new VolunteerStatus { Name = "Neaktivan" }
        );
        await db.SaveChangesAsync();
    }

    if (!await db.DonorTypes.AnyAsync())
    {
        db.DonorTypes.AddRange(
            new DonorType { Name = "Fizička osoba" },
            new DonorType { Name = "Pravna osoba" },
            new DonorType { Name = "Udruga" }
        );
        await db.SaveChangesAsync();
    }

    if (!await db.DonorStatuses.AnyAsync())
    {
        db.DonorStatuses.AddRange(
            new DonorStatus { Name = "Aktivan" },
            new DonorStatus { Name = "Neaktivan" },
            new DonorStatus { Name = "Potencijalni" }
        );
        await db.SaveChangesAsync();
    }

    if (!await db.EmployeePositions.AnyAsync())
    {
        db.EmployeePositions.AddRange(
            new EmployeePosition { Name = "Veterinar" },
            new EmployeePosition { Name = "Njegovatelj" },
            new EmployeePosition { Name = "Administrativni radnik" },
            new EmployeePosition { Name = "Vozač" },
            new EmployeePosition { Name = "Čistač" }
        );
        await db.SaveChangesAsync();
    }

    if (!await db.EmployeeStatuses.AnyAsync())
    {
        db.EmployeeStatuses.AddRange(
            new EmployeeStatus { Name = "Aktivan" },
            new EmployeeStatus { Name = "Na bolovanju" },
            new EmployeeStatus { Name = "Na godišnjem" },
            new EmployeeStatus { Name = "Neaktivan" }
        );
        await db.SaveChangesAsync();
    }
}
app.Run();