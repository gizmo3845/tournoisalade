using System;
using System.Text.Json;

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
			TourNumber = 1;
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

		public void Clear()
        {
			CurrentTour = new Tour();
			Players.Clear();
			LastExcludedPlayer = null;
			TourNumber = 1;
			DeleteTournamentFile();
		}

        public async Task<bool> Save()
        {
            try
            {
				DeleteTournamentFile();

				await using FileStream createStream = File.Create(@"tournament.json");
				await JsonSerializer.SerializeAsync(createStream, this);

				return true;
			}
            catch (Exception ex)
            {
				return false;
            }
        }

		public async Task<bool> Load()
		{
			try
			{
				await using FileStream createStream = File.OpenRead(@"tournament.json");
				var tournament = await JsonSerializer.DeserializeAsync<Tournament>(createStream);

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		private void DeleteTournamentFile()
        {
			if (File.Exists("tournament.json"))
				File.Delete("tournament.json");
		}
	}
}

