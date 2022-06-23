using System;
namespace TournoiSalade.Data
{
	public interface ITournament
	{
		List<Player> Players { get; set; }
		Tour CurrentTour { get; set; }

		void NextTour();
		List<Player> GetPlayerRanks();
		void Clear();
		Task<bool> Save();
	}
}

