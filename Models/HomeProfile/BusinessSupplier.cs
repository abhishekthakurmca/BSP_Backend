
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{

    [Table("business_supplier")]
    public class BusinessSupplier
    {
        [Key]
        public long Id { get; set; }

        public long BusinessId { get; set; }

        public long SupplierTypeId { get; set; }

    }

}