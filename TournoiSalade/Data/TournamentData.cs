namespace TournoiSalade.Data
{
    public class TournamentData
    {
        public List<Player> Players { get; set; } = new List<Player>();
        public Tour CurrentTour { get; set; } = new Tour();

        public List<Player> LastExcludedPlayers { get; set; } = new List<Player>();
        public int TourNumber { get; set; } = 0;
        public int NbPlayerPerTeam { get; set; } = 0;
    }
}
