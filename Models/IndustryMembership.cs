using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models
{
    [Table("industrymembership")]
    public class IndustryMembership
    {
        [Key] // Marks this as the primary key
        public int MembershipId { get; set; }
        public int ProfessionId { get; set; } // Upd your table has an Id column
        public string Name { get; set; }
        // Add other columns from your table as needed
    }
}