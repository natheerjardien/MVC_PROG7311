using System.ComponentModel.DataAnnotations;

namespace prjHealthCareSystem.Models
{

    public class InsurancePolicy
    {
        public int Id { get; set; }
        [Display(Name = "Policy Name")] 
        public string PolicyName { get; set; }
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        [DataType(DataType.Date)] public DateTime StartDate { get; set; }

        [DataType(DataType.Date)] //This forces the data type to be a date 
        public DateTime EndDate { get; set; }

        public string Status { get; set; } = "Draft"; //Default it will be drafted
        public string ServiceLevel { get; set; }
        public string? SignedConsentPath { get; set; }
    }
}