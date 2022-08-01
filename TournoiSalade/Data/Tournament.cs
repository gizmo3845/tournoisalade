using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TournoiSalade.Data
{
	public class Tournament : ITournament
	{
		public List<Player> Players { get; set; } = new List<Player>();
		public Tour CurrentTour { get; set; } = new Tour();

		public Player? LastExcludedPlayer { get; set; }
		public int TourNumber { get; set; } = 1;

        public Tournament()
        {
			Load();
        }

		public void New()
        {
			CurrentTour.New();
			Players.Clear();
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

		public void ComputePlayerPoints()
        {
            foreach (var match in CurrentTour.Matches)
            {
				match.UpdatePlayerScore();
            }
        }

		public List<Player> GetPlayerRanks()
        {
			return Players.OrderByDescending(p => p.Score).ToList();
        }

        public async Task<bool> Load()
        {
			var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "tournament.json");
			if(!File.Exists(path))
				return false;

			var jsonString = await File.ReadAllTextAsync(path);
			var tournament = JsonSerializer.Deserialize<Tournament>(jsonString);
			Players = tournament.Players;
			CurrentTour = tournament.CurrentTour;
			LastExcludedPlayer = tournament.LastExcludedPlayer;
			TourNumber = tournament.TourNumber;

			return true;
        }

        public async Task<bool> Save()
        {
			string jsonString = JsonSerializer.Serialize(this);

			var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "tournament.json");

			await File.WriteAllTextAsync(path, jsonString);

			return jsonString != null;	
		}
    }
}

