namespace CafeReactApp.Server.Model
{
    public class Cafe
    {
        public string CafeId { get; set; }

        public string CafeName { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string Logo { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
