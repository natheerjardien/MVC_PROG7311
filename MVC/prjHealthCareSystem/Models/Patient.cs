using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prjHealthCareSystem.Models
{
    public class Patient
    {
        [Key]

        public int Id {get;set;}
        public string Name {get; set;}
        public string ContactDetails {get; set;} 
        public string Region { get; set; } //Where they come from, dankie :)

        public List<InsurancePolicy> Policies { get; set; } = new();
    }
}