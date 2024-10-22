using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models
{
    public class UserProfession
    {
        [Key]
        public int Id { get; set; }
        [Column("user_id")] // Maps the property to the correct column in the database
        public int UserId { get; set; } 
        
        [Column("profession_id")] 
        public int ProfessionId { get; set; } // Upd your table has an Id column

    }
}