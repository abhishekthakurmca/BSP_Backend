
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{

    [Table("business_certification")]
    public class BusinessCertification
    {
        [Key]
        public long Id { get; set; }

        public long BusinessId { get; set; }

        public long CertificationId { get; set; }

    }

}