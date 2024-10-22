
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.search.models
{
public class SearchRequest
{
    public string SearchTerm { get; set; }
    public string PostCode { get; set; }

    public string? KeywordSearch { get; set; }  
    public int? Within { get; set; } = 25; 

    public WorkScopeFilter? WorkScope { get; set; }

    public string? Ratings { get; set; }

    public Options? Options { get; set; }
}
}