
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.search.models
{
public class OzPostcode
{
    public int Id { get; set; }
    public string Postcode { get; set; }
    public string? Locality { get; set; }
    public string? Mystate { get; set; }
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
}
}