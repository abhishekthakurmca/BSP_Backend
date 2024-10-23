using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models;

[Table("supplier_type")]
public class SupplierType
{
    [Key]
    [Column("supplier_type_id")]
    public int SupplierTypeId { get; set; }
    public string Name { get; set; }
}