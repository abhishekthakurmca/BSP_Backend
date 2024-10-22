
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{

    [Table("business_profession")]
    public class BusinessProfession
    {
        [Key]
        public int Id { get; set; }

        public int BusinessId { get; set; }

        public int ProfessionId { get; set; }

        //public Business Business { get; set; }
        //public Profession Profession { get; set; }
    }

}