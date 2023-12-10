using System.ComponentModel.DataAnnotations;
using System.IO;

namespace PlayTen.BLL.DTO
{ 
    public class BlobDTO
    {
        public string? Uri { get; set; }
        public string? Name { get; set; }
        public bool Error { get; set; }
    }
}
