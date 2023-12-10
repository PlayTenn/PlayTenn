namespace PlayTen.BLL.DTO
{
    public class TrainingDTO
    {
        public TrainingInfoDTO Training { get; set; }
        public bool IsUserTrainingOwner { get; set; }
        public bool IsUserParticipant { get; set; }
        public bool IsUserApprovedParticipant { get; set; }
        public bool IsUserUndeterminedParticipant { get; set; }
        public bool IsUserRejectedParticipant { get; set; }
        public bool IsTrainingFinished { get; set; }
    }
}
