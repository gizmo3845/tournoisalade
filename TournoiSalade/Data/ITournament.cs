namespace TournoiSalade.Data
{
    public interface ITournament
    {
        List<Player> Players { get; set; }
        Tour CurrentTour { get; set; }
        int TourNumber { get; set; }

        Task New();
        Task NextTour();
        void ComputePlayerPoints();
        List<Player> GetPlayerRanks();

        Task<bool> Load();
        Task<bool> Save();
    }
}
