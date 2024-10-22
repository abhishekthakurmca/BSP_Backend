using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace MyBackendApp.Models
{
    public class BusinessUserModel
    {
        [Key]
        public int businessuserid { get; set; }

        public int businessid { get; set; }

        [Column("business_name")]
        public string? BusinessName { get; set; }
        public string Email { get; set; }
        public string Pwd { get; set; }
        public string? Phone { get; set; }
        
        [Column("first_name")]
        public string? FirstName { get; set; }
        
        [Column("last_name")]
        public string? LastName { get; set; }
        public string? Postcode { get; set; }
    }

    public class BusinessUserDTOModel
    {
        [Key]
        [Column("businessuser_id")]
        public int businessuser_id { get; set; }

        public int business_id { get; set; }

        [Column("business_name")]
        public string? BusinessName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }
        public string? Postcode { get; set; }
    }
}