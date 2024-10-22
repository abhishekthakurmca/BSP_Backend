
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models
{
    public class HomeUserModel
    {
        [Key]
        [Column("user_id")] // This marks the property as the primary key
        public int user_id { get; set; } 
        public string Email { get; set; }
        public string Pwd { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Postcode { get; set; }

        public string Suburb { get; set; }
    }

    public class HomeUserDTOModel
    {
        [Key] // Marks this as the primary key
        [Column("user_id")]         
        public int user_id { get; set; } // Primary Key

        public string email { get; set; } // Email Address

        public string phone { get; set; } // Phone Number
    }
}