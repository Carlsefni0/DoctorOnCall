using Microsoft.EntityFrameworkCore;
using DoctorOnCall.Models;

namespace DoctorOnCall;

public class DataContext: DbContext
{
    public DbSet<DoctorModel> Doctors { get; set; }
    public DbSet<VisitRequestModel> VisitRequests { get; set; }

    public DataContext(DbContextOptions options) : base(options) { }
   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<DoctorModel>().ToTable("Doctors");

    }
}