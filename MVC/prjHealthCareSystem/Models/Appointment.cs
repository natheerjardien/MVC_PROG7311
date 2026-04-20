using System.ComponentModel.DataAnnotations;

namespace prjHealthCareSystem.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public int InsurancePolicyId {get;set;}
        public InsurancePolicy? Policy { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; set; }
    }
}