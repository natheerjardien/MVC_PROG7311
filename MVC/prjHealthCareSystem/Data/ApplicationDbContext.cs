using Microsoft.EntityFrameworkCore;
using prjHealthCareSystem.Models;

namespace prjHealthCareSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<InsurancePolicy> InsurancePolicies { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
    }
}