using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MyBackendApp.Models.HomeProfile;

namespace MyBackendApp.Models
{

    [Table("business")]
    public class Business
    {
        [Key]
        [Column("businessid")]
        public int BusinessId { get; set; }
        public string? BusinessName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }


        public string? Overview { get; set; }
        public string? Description { get; set; }

        public string? BusinessType { get; set; }
        public int? Residential { get; set; }
        public int? Commercial { get; set; }
        public int? Government { get; set; }

        public string? LicenseNumber { get; set; }
        public DateTime? LicenseExpiry { get; set; }

        public string FormattedLicenseExpiry { get; set; }
        public bool? FullyInsured { get; set; }

        public string? RegistrationNumber { get; set; }
        public int? YearsExperience { get; set; }

        public string? StreetNumber { get; set; }
        public string? StreetName { get; set; }
        public string? Suburb { get; set; }
        public string? MyState { get; set; }
        public string? Country { get; set; }
        public string? PostCode { get; set; }

        // Lat/Long for display in Google Maps
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public decimal? OverallRating { get; set; }

        public int? AfterHours { get; set; }
        public int? Weekends { get; set; }

        public List<Certification>? certifications { get; set; }

        /*
        public ICollection<BusinessProfession> BusinessProfessions { get; set; }
        public ICollection<BusinessAncillary> BusinessAncillary { get; set; }

        public ICollection<BusinessService> BusinessService { get; set; }

        public ICollection<BusinessSupplier> BusinessSupplier { get; set; }
        */
    }

}