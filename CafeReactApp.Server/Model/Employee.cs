using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafeReactApp.Server.Model
{
    public class Employee
    {
        public string EmployeeId { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime JoinDate { get; set; }

        public string CafeId { get; set; }

        public Cafe Cafe { get; set; }
         
    }
}
