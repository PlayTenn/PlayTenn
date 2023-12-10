namespace PlayTen.BLL.DTO
{
    public class TournamentDTO
    {
        public TournamentInfoDTO Tournament { get; set; }
        public bool IsUserTournamentOwner { get; set; }
        public bool IsUserParticipant { get; set; }
        public bool IsUserApprovedParticipant { get; set; }
        public bool IsUserUndeterminedParticipant { get; set; }
        public bool IsUserRejectedParticipant { get; set; }
        public bool IsRegistrationFinished { get; set; }
    }
}
