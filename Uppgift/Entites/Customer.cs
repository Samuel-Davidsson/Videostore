using System.Collections.Generic;

namespace Uppgift.Entites
{
    public class Customer
    {
        public Customer()
        {
            Movies = new List<Movie>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsMember { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
