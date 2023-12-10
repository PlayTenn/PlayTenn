using System.Collections.Generic;

namespace PlayTen.BLL.DTO
{
    public class TennisLevelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserDTO> Users { get; set; }
    }
}
