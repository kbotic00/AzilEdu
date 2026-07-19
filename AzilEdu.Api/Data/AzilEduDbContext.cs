using AzilEdu.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AzilEdu.Api.Data;

public class AzilEduDbContext : DbContext
{
    public AzilEduDbContext(DbContextOptions<AzilEduDbContext> options)
        : base(options)
    {
    }

    public DbSet<HousingUnit> HousingUnits => Set<HousingUnit>();

    public DbSet<Volunteer> Volunteers => Set<Volunteer>();
    public DbSet<VolunteerStatus> VolunteerStatuses => Set<VolunteerStatus>();

    public DbSet<Donor> Donors => Set<Donor>();
    public DbSet<DonorType> DonorTypes => Set<DonorType>();
    public DbSet<DonorStatus> DonorStatuses => Set<DonorStatus>();

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<EmployeePosition> EmployeePositions => Set<EmployeePosition>();
    public DbSet<EmployeeStatus> EmployeeStatuses => Set<EmployeeStatus>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Volunteer>()
            .HasOne(v => v.VolunteerStatus)
            .WithMany()
            .HasForeignKey(v => v.VolunteerStatusId);

        modelBuilder.Entity<Donor>()
            .HasOne(d => d.DonorType)
            .WithMany()
            .HasForeignKey(d => d.DonorTypeId);

        modelBuilder.Entity<Donor>()
            .HasOne(d => d.DonorStatus)
            .WithMany()
            .HasForeignKey(d => d.DonorStatusId);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.EmployeePosition)
            .WithMany()
            .HasForeignKey(e => e.EmployeePositionId);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.EmployeeStatus)
            .WithMany()
            .HasForeignKey(e => e.EmployeeStatusId);
    }
}