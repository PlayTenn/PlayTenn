using PlayTen.BLL.DTO;
using System.Collections.Generic;

namespace PlayTen.BLL.DTO
{
    public class TrainingUserDTO
    {
        public UserDTO User { get; set; }
        public ICollection<TrainingInfoDTO> PlanedTrainings { get; set; }
        public ICollection<TrainingInfoDTO> CreatedTrainings { get; set; }
        public ICollection<TrainingInfoDTO> VisitedTrainings { get; set; }
    }
}