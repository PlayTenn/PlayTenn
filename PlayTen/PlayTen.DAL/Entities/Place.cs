using System.Collections.Generic;

namespace PlayTen.DAL.Entities
{
    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string PhotoUrl { get; set; }
        public string PhotoFilename { get; set; }
        public int SurfaceId { get; set; }
    }
}
