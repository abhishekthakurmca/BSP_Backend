using Microsoft.EntityFrameworkCore;
using MyBackendApp.Models;
using MyBackendApp.Models.HomeProfile;
using MyBackendApp.Models.Jobs;
using MyBackendApp.Models.BusinessNameSpace;
using MyBackendApp.Models.BusinessModels;
using MyBackendApp.Models.Search;


namespace MyBackendApp.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        var connectionString = "";
    //        optionsBuilder
    //            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
    //            .LogTo(Console.WriteLine, LogLevel.Information);
    //    }
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Profession> Profession { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ProfessionAncillary> ProfessionAncillary { get; set; }
    public DbSet<SupplierType> SupplierType { get; set; }
    public DbSet<HomeUser> HomeUsers { get; set; }
    public DbSet<BusinessUserModel> BusinessUser { get; set; }
    public DbSet<UserProfession> UserProfession { get; set; }

    // HomeUser Profile
    public DbSet<HomeUserProfileModel> HomeUserProfile { get; set; }
    public DbSet<GenderModel> Genders { get; set; }
    public DbSet<AgeGroup> AgeGroups { get; set; }
    public DbSet<WorkingStatusEntity> WorkingStatuses { get; set; }
    public DbSet<LivingStatusModel> LivingStatuses { get; set; }
    public DbSet<WorkingIndustry> Industries { get; set; }

    public DbSet<HomeUserLookingForProfession> HomeUserLookingForProfessions { get; set; }
    public DbSet<HomeUserLookingForServices> HomeUserLookingForServices { get; set; }
    public DbSet<HomeUserLookingForSupplier> HomeUserLookingForSuppliers { get; set; }
    public DbSet<HomeUserLookingForProfessionAncillary> HomeUserLookingForProfessionAncillaries { get; set; }


    public DbSet<ConstructionInterest> ConstructionInterests { get; set; }
    public DbSet<PersonalInterest> PersonalInterests { get; set; }
    public DbSet<HomeUserConstructionInterest> HomeUserConstructionInterests { get; set; }
    public DbSet<HomeUserPersonalInterest> HomeUserPersonalInterests { get; set; }

    public DbSet<Business> Businesses { get; set; }

    public DbSet<BusinessProfession> BusinessProfessions { get; set; }
    public DbSet<BusinessAncillary> BusinessAncillaries { get; set; }
    public DbSet<BusinessService> BusinessServices { get; set; }

    public DbSet<BusinessSupplier> BusinessSuppliers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<BusinessProducts> BusinessProducts { get; set; }
    public DbSet<Certification> Certifications { get; set; }
    public DbSet<BusinessCertification> BusinessCertifications { get; set; }
    public DbSet<Qualification> Qualifications { get; set; }
    public DbSet<BusinessQualification> BusinessQualifications { get; set; }
    public DbSet<IndustryMembership> IndustryMemberships { get; set; }
    public DbSet<BusinessMembership> BusinessMemberships { get; set; }

    public DbSet<OzPostcode> OzPostcodes { get; set; }
    public DbSet<JobModel> Jobs { get; set; }
    public DbSet<ReviewOfBusiness> ReviewOfBusiness { get; set; }
    public DbSet<MediaForBusiness> MediaForBusiness { get; set; }
}