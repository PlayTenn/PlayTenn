using System.Collections.Generic;

namespace PlayTen.DAL.Entities
{
    public class TennisLevel
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
