namespace ApiChessMeet.DTO
{
    public class TournamentDetailsDTO
    {
        public string Guid { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Place { get; set; } = string.Empty;
        public int PlayersMin { get; set; }
        public int PlayersMax { get; set; }
        public int EloMin { get; set; }
        public int EloMax { get; set; }
        public string[] Categories { get; set; }
        public string Status { get; set; } = string.Empty;
        public int CurrentRound { get; set; }
        public bool WomenOnly { get; set; }
        public DateTime EndRegistration { get; set; }

        public IEnumerable<DalChessMeet.Entities.MemberDetails> PlayersDetails { get; set; } 

        public TournamentDetailsDTO(DalChessMeet.Entities.TournamentDetails tournament)
        {
            Guid = tournament.Guid;
            Name = tournament.Name;
            Place = tournament.Place;
            PlayersMin = tournament.PlayersMin;
            PlayersMax = tournament.PlayersMax;
            EloMin = tournament.EloMin;
            EloMax = tournament.EloMax;
            Categories = tournament.Categories.Split(',');
            Status = tournament.Status;
            CurrentRound = tournament.CurrentRound;
            WomenOnly = tournament.WomenOnly;
            EndRegistration = tournament.EndRegistration;
            PlayersDetails = tournament.PlayersDetails;
        }
    }
}
