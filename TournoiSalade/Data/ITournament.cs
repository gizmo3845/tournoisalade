namespace TournoiSalade.Data
{
    public interface ITournament
    {
        List<Player> Players { get; set; }
        Tour CurrentTour { get; set; }

        void New();
        void NextTour();
        List<Player> GetPlayerRanks();

        Task<bool> Load();
        Task<bool> Save();
    }
}
