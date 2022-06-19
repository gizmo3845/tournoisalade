using System;
using TournoiSalade.Utils;

namespace TournoiSalade.Data
{
	public class Tour
	{
        private static Random _random = new Random();

        public List<Match> Matches { get; set; }

		public void Generate(List<Player> players, Player? forcePlayer, out Player? excludedPlayer)
        {
            players.Shuffle();
            List<Team> teams = GenerateTeams(players, forcePlayer, out excludedPlayer);
            GenerateMatches(teams);
        }

        private List<Team> GenerateTeams(List<Player> players, Player? forcePlayer, out Player? excludedPlayer)
        {
            List<Team> teams = new List<Team>();
            var teamPlayers = new List<Player>();
            int teamId = 1;
            for (int i = 0; i < players.Count / 2; i++)
            {
                Team team = new Team() { Id = teamId, Player1 = players[i], Player2 = players[players.Count - i - 1] };
                teams.Add(team);
                teamPlayers.Add(team.Player1);
                teamPlayers.Add(team.Player2);

                teamId++;
            }

            var excludedPlayers = players.Except(teamPlayers);
            excludedPlayer = excludedPlayers?.FirstOrDefault();

            if (excludedPlayer != null && excludedPlayer.Equals(forcePlayer))
            {
                players.Shuffle();
                return GenerateTeams(players, forcePlayer, out excludedPlayer);
            }

            return teams;
        }

        public void SetMatchResult(int team1Id, int team1Result, int team2Id, int team2Result)
        {
            var match = Matches.First(m => m.Team1.Id == team1Id && m.Team2.Id == team2Id);
            match.SetResult(team1Result, team2Result);
        }

        private void GenerateMatches(List<Team> teams)
        {
            Matches = new List<Match>();
            for (int i = 0; i < teams.Count / 2; i++)
            {
                Match match = new Match() { Team1 = teams[i], Team2 = teams[teams.Count - i - 1] };
                Matches.Add(match);
            }
        }

        public override string ToString()
        {
            string output = "";
            foreach (var match in Matches)
            {
                output += $"({match.Team1.Player1.Name},{match.Team1.Player2.Name}) VS ({match.Team2.Player1.Name},{match.Team2.Player2.Name})" + Environment.NewLine;
            }

            return output;
        }
    }
}

