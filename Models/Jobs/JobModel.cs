
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Models.Jobs
{

    public class JobModel
    {
        [Key]
        [Column("job_id")]
        public long job_id { get; set; }
        public long user_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal? budget { get; set; }
        public DateTime? eta { get; set; }
        public string postcode { get; set; }
        public string job_status { get; set; }
        public long? assigned_to { get; set; }
        public string? business_name { get; set; }
    }

}