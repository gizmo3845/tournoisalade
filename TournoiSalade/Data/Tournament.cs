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

		public List<Player> LastExcludedPlayers { get; set; } = new List<Player>();
		public int TourNumber { get; set; } = 0;

		public async Task New()
        {
			CurrentTour.New();
			Players.Clear();
			TourNumber = 0;
			LastExcludedPlayers = new();
			await Save();
		}

		public async Task NextTour()
        {
			CurrentTour.Generate(Players, LastExcludedPlayers, out List<Player> lastExcludedPlayers);
			LastExcludedPlayers = lastExcludedPlayers;

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

			var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "db", "tournament.json");
			if(!File.Exists(path))
				return false;

			var jsonString = await File.ReadAllTextAsync(path);
			var tournament = JsonSerializer.Deserialize<Tournament>(jsonString);
			Players = tournament.Players;
			CurrentTour = tournament.CurrentTour;
			LastExcludedPlayers = tournament.LastExcludedPlayers;
			TourNumber = tournament.TourNumber;

			_isLoaded = true;

			return true;
        }

        public async Task<bool> Save()
        {
			string jsonString = JsonSerializer.Serialize(this);

			var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "db", "tournament.json");

			await File.WriteAllTextAsync(path, jsonString);

			return jsonString != null;	
		}
    }
}

