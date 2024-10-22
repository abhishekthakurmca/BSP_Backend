
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MyBackendApp.Models.HomeProfile;
namespace MyBackendApp.Models
{

    [Table("product")]
    public class Product
    {
        [Key]
        [Column("productid")]
        public int ProductId { get; set; }

        public string? ProductName { get; set; }
        public int? SupplierTypeId { get; set; }
        public string? Descripttion { get; set; }

    }

}