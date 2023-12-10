using System.Collections.Generic;

namespace PlayTen.DAL.Entities
{
    public class Surface
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public ICollection<Place> Places { get; set; }
    }
}
