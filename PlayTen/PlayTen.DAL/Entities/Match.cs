namespace PlayTen.DAL.Entities
{
    public class Match
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int Round { get; set; }
        public string? Score { get; set; }
        public string? Player1Id { get; set; }
        public string? Player2Id { get; set; }
        public string? WinnerId { get; set; }
        public string? LooserId { get; set; }

        public Tournament Tournament { get; set; }
        public User Player1 { get; set; }
        public User Player2 { get; set; }
        public User Winner { get; set; }
        public User Looser { get; set; }
    }
}
