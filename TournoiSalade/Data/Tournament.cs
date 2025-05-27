using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TournoiSalade.Data
{
	public class Tournament
	{
		private bool _isLoaded = false;
        private AuthenticationManagement _authenticationManagement;

		public TournamentData TournamentData { get; set; }

        public Tournament(AuthenticationManagement authenticationManagement)
        {
            _authenticationManagement = authenticationManagement;
            TournamentData = new TournamentData();
        }

		public async Task New()
        {
			int? nbPlayerPerTeam = await _authenticationManagement.GetNbPlayerPerTeam();

            TournamentData.CurrentTour.New(nbPlayerPerTeam.Value);
            TournamentData.Players.Clear();
            TournamentData.TourNumber = 0;
            TournamentData.LastExcludedPlayers = new();
            TournamentData.NbPlayerPerTeam = _authenticationManagement.GetNbPlayerPerTeam().Result ?? 0;
            await Save();
		}

		public async Task NextTour()
        {
            TournamentData.CurrentTour.Generate(TournamentData.NbPlayerPerTeam, TournamentData.Players, TournamentData.LastExcludedPlayers, out List<Player> lastExcludedPlayers);
            TournamentData.LastExcludedPlayers = lastExcludedPlayers;

            TournamentData.TourNumber++;

			await Save();
		}

		public void ComputePlayerPoints()
        {
            foreach (var match in TournamentData.CurrentTour.Matches)
            {
				match.UpdatePlayerScore();
            }
        }

		public List<Player> GetPlayerRanks()
        {
			return TournamentData.Players.OrderByDescending(p => p.Score).ToList();
        }

        public async Task<bool> Load()
        {
			if (_isLoaded)
				return true;

            try
            {
                var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "db", "tournament.json");
                if(!File.Exists(path))
                    return false;

                var jsonString = await File.ReadAllTextAsync(path);
                TournamentData = JsonSerializer.Deserialize<TournamentData>(jsonString);

                _isLoaded = true;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> Save()
        {
            var playerPerTeam = await _authenticationManagement.GetNbPlayerPerTeam();
            TournamentData.NbPlayerPerTeam = playerPerTeam ?? 0;

			string jsonString = JsonSerializer.Serialize(TournamentData);

			var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "db", "tournament.json");

			await File.WriteAllTextAsync(path, jsonString);

			return jsonString != null;	
		}
    }
}

