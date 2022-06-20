using System;
namespace TournoiSalade.Data
{
	public class Tournament : ITournament
	{
		public List<Player> Players { get; set; }
		public Tour CurrentTour { get; set; }

		private Player? LastExcludedPlayer { get; set; }
		private int TourNumber { get; set; } = 1;

        public Tournament()
        {
			Players = new();
			CurrentTour = new();
		}

		public void NextTour()
        {
			Console.WriteLine($"TOUR {TourNumber} --------------");

			if (LastExcludedPlayer != null && !Players.Contains(LastExcludedPlayer))
				LastExcludedPlayer = null;

			CurrentTour.Generate(Players, LastExcludedPlayer, out Player? lastExcludedPlayer);
			LastExcludedPlayer = lastExcludedPlayer;
			Console.WriteLine(CurrentTour.ToString());
			Console.WriteLine($"Exluded : {LastExcludedPlayer}");

			TourNumber++;
		}

		public List<Player> GetPlayerRanks()
        {
			return Players.OrderByDescending(p => p.Score).ToList();
        }
	}
}

