using Microsoft.EntityFrameworkCore;
using DoctorOnCall.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DoctorOnCall;

public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole, int, 
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, 
    IdentityRoleClaim<int>, IdentityUserToken<int>>(options)
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Medicine> Medicines { get; set; }
    public DbSet<VisitRequestMedicine> VisitRequestMedicine { get; set; }
    public DbSet<VisitRequest> VisitRequests { get; set; }
    public DbSet<Visit> Visits { get; set; }
    public DbSet<ScheduleModel> Schedules { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

  
    modelBuilder.Entity<AppUser>()
        .HasMany(ur => ur.UserRoles)
        .WithOne(u => u.User)
        .HasForeignKey(ur => ur.UserId)
        .IsRequired();
        
        
    modelBuilder.Entity<AppRole>()
        .HasMany(ur => ur.UserRoles)
        .WithOne(u => u.Role)
        .HasForeignKey(ur => ur.RoleId)
        .IsRequired();

    modelBuilder.Entity<Doctor>()
        .HasOne(d => d.User)
        .WithOne()
        .HasForeignKey<Doctor>(d => d.UserId)
        .OnDelete(DeleteBehavior.Cascade); 

    modelBuilder.Entity<Patient>()
        .HasOne(p => p.User)
        .WithOne()
        .HasForeignKey<Patient>(p => p.UserId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Visit>()
        .HasOne(vr => vr.VisitRequest)
        .WithOne()
        .HasForeignKey<Visit>(vr => vr.VisitRequestId)
        .OnDelete(DeleteBehavior.NoAction);
    
    modelBuilder.Entity<VisitRequestMedicine>()
        .HasOne(vrm => vrm.VisitRequest)
        .WithMany(vr => vr.RequestedMedicines) 
        .OnDelete(DeleteBehavior.Cascade); 
    
    
    modelBuilder.Entity<Medicine>().HasData(
        new Medicine
        {
            Id = 1,
            Name = "Paracetamol",
            Description = "Pain reliever and fever reducer.",
            UnitPrice = 5.99m,
            Dosage = "500mg"
        },
        new Medicine
        {
            Id = 2,
            Name = "Ibuprofen",
            Description = "Nonsteroidal anti-inflammatory drug used to reduce pain and inflammation.",
            UnitPrice = 8.49m,
            Dosage = "200mg"
        },
        new Medicine
        {
            Id = 3,
            Name = "Amoxicillin",
            Description = "Antibiotic used to treat bacterial infections.",
            UnitPrice = 12.75m,
            Dosage = "250mg"
        },
        new Medicine
        {
            Id = 4,
            Name = "Cetirizine",
            Description = "Antihistamine used to relieve allergy symptoms.",
            UnitPrice = 4.50m,
            Dosage = "10mg"
        },
        new Medicine
        {
            Id = 5,
            Name = "Aspirin",
            Description = "Pain reliever and anti-inflammatory.",
            UnitPrice = 6.25m,
            Dosage = "300mg"
        },
        new Medicine
        {
            Id = 6,
            Name = "Metformin",
            Description = "Medication for type 2 diabetes.",
            UnitPrice = 9.30m,
            Dosage = "500mg"
        },
        new Medicine
        {
            Id = 7,
            Name = "Omeprazole",
            Description = "Used to treat acid reflux and ulcers.",
            UnitPrice = 7.80m,
            Dosage = "20mg"
        },
        new Medicine
        {
            Id = 8,
            Name = "Salbutamol",
            Description = "Bronchodilator used for asthma and COPD.",
            UnitPrice = 11.50m,
            Dosage = "100mcg"
        }
    );

    
}

}
