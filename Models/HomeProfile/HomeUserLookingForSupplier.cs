
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.HomeProfile
{
    [Table("homeuser_lookingfor_supplier")]

    public class HomeUserLookingForSupplier
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int SupplierTypeId { get; set; }
    }

}