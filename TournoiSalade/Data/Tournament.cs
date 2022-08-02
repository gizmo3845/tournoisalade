using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TournoiSalade.Data
{
	public class Tournament : ITournament
	{
		private bool _isLoaded = false;

		public List<Player> Players { get; set; } = new List<Player>();
		public Tour CurrentTour { get; set; } = new Tour();

		public Player? LastExcludedPlayer { get; set; }
		public int TourNumber { get; set; } = 0;

		public async Task New()
        {
			CurrentTour.New();
			Players.Clear();
			TourNumber = 0;
			LastExcludedPlayer = null;
			await Save();
		}

		public async Task NextTour()
        {
			if (LastExcludedPlayer != null && !Players.Contains(LastExcludedPlayer))
				LastExcludedPlayer = null;

			CurrentTour.Generate(Players, LastExcludedPlayer, out Player? lastExcludedPlayer);
			LastExcludedPlayer = lastExcludedPlayer;

			TourNumber++;

			await Save();
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
			if (_isLoaded)
				return true;

			var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "tournament.json");
			if(!File.Exists(path))
				return false;

			var jsonString = await File.ReadAllTextAsync(path);
			var tournament = JsonSerializer.Deserialize<Tournament>(jsonString);
			Players = tournament.Players;
			CurrentTour = tournament.CurrentTour;
			LastExcludedPlayer = tournament.LastExcludedPlayer;
			TourNumber = tournament.TourNumber;

			_isLoaded = true;

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

