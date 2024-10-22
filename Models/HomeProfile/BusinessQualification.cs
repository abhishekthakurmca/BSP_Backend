
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{

    [Table("business_qualification")]
    public class BusinessQualification
    {
        [Key]
        public int Id { get; set; }

        public int BusinessId { get; set; }

        public int QualificationId { get; set; }

    }

}