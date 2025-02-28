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
    public DbSet<DoctorVisitRequest> DoctorVisitRequests { get; set; }
    public DbSet<VisitRequestMedicine> VisitRequestMedicine { get; set; }
    public DbSet<VisitRequest> VisitRequests { get; set; }
    public DbSet<RegularVisitDate> RegularVisitDates { get; set; }
    public DbSet<Visit> Visits { get; set; }
    public DbSet<ScheduleType> ScheduleTypes { get; set; }
    public DbSet<ScheduleDayMapping> ScheduleDayMappings { get; set; }
    public DbSet<ScheduleDay> ScheduleDays { get; set; }
    public DbSet<DoctorScheduleAssignment> DoctorScheduleAssignments { get; set; }
    public DbSet<ScheduleException> ScheduleExceptions { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

  
   modelBuilder.Entity<AppUser>()
        .HasMany(ur => ur.UserRoles)
        .WithOne(u => u.User)
        .HasForeignKey(ur => ur.UserId)
        .OnDelete(DeleteBehavior.Cascade)
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

    modelBuilder.Entity<DoctorVisitRequest>()
        .HasKey(dvr => new { dvr.DoctorId, dvr.VisitRequestId });

    modelBuilder.Entity<DoctorVisitRequest>()
        .HasOne(dvr => dvr.Doctor)
        .WithMany(d => d.DoctorVisitRequests)
        .HasForeignKey(dvr => dvr.DoctorId)
        .OnDelete(DeleteBehavior.Cascade); // Каскадне видалення запитів на візити

    modelBuilder.Entity<DoctorVisitRequest>()
        .HasOne(dvr => dvr.VisitRequest)
        .WithMany(vr => vr.DoctorVisitRequests)
        .HasForeignKey(dvr => dvr.VisitRequestId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Doctor>()
        .HasMany(d => d.ScheduleAssignments)
        .WithOne(sa => sa.Doctor)
        .HasForeignKey(sa => sa.DoctorId)
        .OnDelete(DeleteBehavior.Cascade); 

    modelBuilder.Entity<ScheduleType>()
        .HasMany(st => st.DoctorScheduleAssignments)
        .WithOne(dsa => dsa.ScheduleType)
        .HasForeignKey(dsa => dsa.ScheduleTypeId)
        .OnDelete(DeleteBehavior.Cascade); 

    modelBuilder.Entity<ScheduleDayMapping>()
        .HasKey(mapping => new { mapping.ScheduleTypeId, mapping.ScheduleDayId });

    modelBuilder.Entity<ScheduleDayMapping>()
        .HasOne(mapping => mapping.ScheduleType)
        .WithMany(schedule => schedule.ScheduleDayMappings)
        .HasForeignKey(mapping => mapping.ScheduleTypeId)
        .OnDelete(DeleteBehavior.Restrict); 

    modelBuilder.Entity<ScheduleDayMapping>()
        .HasOne(mapping => mapping.ScheduleDay)
        .WithMany(day => day.ScheduleDayMappings)
        .HasForeignKey(mapping => mapping.ScheduleDayId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<ScheduleDay>()
        .Property(sd => sd.StartTime)
        .IsRequired();

    modelBuilder.Entity<ScheduleDay>()
        .Property(sd => sd.EndTime)
        .IsRequired();

  
    
    modelBuilder.Entity<Medicine>().HasData(
        new Medicine
        {
            Id = 1,
            Name = "Paracetamol",
            ImageUrl = "https://compendium.com.ua/_ipx/q_80&fit_fill&s_620x680/https://compendium.com.ua/img/dec/194013_105_45_18_17.jpg",
            Description = "Pain reliever and fever reducer.",
            UnitPrice = 5.99,
            Dosage = "500mg"
        },
        new Medicine
        {
            Id = 2,
            Name = "Ibuprofen",
            ImageUrl = "https://compendium.com.ua/_ipx/q_80&fit_fill&s_620x680/https://compendium.com.ua/img/dec/716756_89_81_35_27.jpg",
            Description = "Nonsteroidal anti-inflammatory drug used to reduce pain and inflammation.",
            UnitPrice = 8.49,
            Dosage = "200mg"
        },
        new Medicine
        {
            Id = 3,
            Name = "Amoxicillin",
            ImageUrl = "https://compendium.com.ua/_ipx/q_80&fit_fill&s_620x680/https://compendium.com.ua/img/dec/20789_100_46_18_17.jpg",
            Description = "Antibiotic used to treat bacterial infections.",
            UnitPrice = 12.75,
            Dosage = "250mg"
        },
        new Medicine
        {
            Id = 4,
            Name = "Cetirizine",
            ImageUrl = "https://compendium.com.ua/_ipx/q_80&fit_fill&s_620x680/https://compendium.com.ua/img/dec/486129_84_33_33_14.jpg",
            Description = "Antihistamine used to relieve allergy symptoms.",
            UnitPrice = 4.50,
            Dosage = "10mg"
        },
        new Medicine
        {
            Id = 5,
            Name = "Aspirin",
            ImageUrl = "https://compendium.com.ua/_ipx/q_80&fit_fill&s_620x680/https://compendium.com.ua/img/dec/12077_93_50_28_45.jpg",
            Description = "Pain reliever and anti-inflammatory.",
            UnitPrice = 6.25,
            Dosage = "300mg"
        },
        new Medicine
        {
            Id = 6,
            Name = "Metformin",
            ImageUrl = "https://compendium.com.ua/_ipx/q_80&fit_fill&s_620x680/https://compendium.com.ua/img/dec/139502_115_90_45_104.jpg",
            Description = "Medication for type 2 diabetes.",
            UnitPrice = 9.30,
            Dosage = "500mg"
        },
        new Medicine
        {
            Id = 7,
            Name = "Omeprazole",
            ImageUrl = "https://compendium.com.ua/_ipx/q_80&fit_fill&s_620x680/https://compendium.com.ua/img/dec/643744_43_92_40_33.jpg",
            Description = "Used to treat acid reflux and ulcers.",
            UnitPrice = 7.80,
            Dosage = "20mg"
        },
        new Medicine
        {
            Id = 8,
            Name = "Salbutamol",
            ImageUrl = "https://compendium.com.ua/_ipx/q_80&fit_fill&s_620x680/https://compendium.com.ua/img/dec/116721_102_50_35_48.jpg",
            Description = "Bronchodilator used for asthma and COPD.",
            UnitPrice = 11.50,
            Dosage = "100mcg"
        }
    );
    

    
}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=DESKTOP-3HPCOO1\\SQLEXPRESS;Database=DoctorOnCall;TrustServerCertificate=True;Integrated Security=True",
            x => x.UseNetTopologySuite());
    }

}
