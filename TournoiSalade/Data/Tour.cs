using System;
using TournoiSalade.Utils;

namespace TournoiSalade.Data
{
	public class Tour
	{
        private static Random _random = new Random();

        public List<Match> Matches { get; set; } = new();
        public List<Player> ExcludedPlayers { get; set; } = new();

        public void New()
        {
            Matches?.Clear();
            ExcludedPlayers?.Clear();
        }

		public void Generate(List<Player> players, List<Player> forcePlayers, out List<Player> excludedPlayers)
        {
            players.Shuffle();
            List<Team> teams = GenerateTeams(players, forcePlayers);
            GenerateMatches(teams, forcePlayers);

            excludedPlayers = ExcludedPlayers;
        }

        private List<Team> GenerateTeams(List<Player> players, List<Player> forcePlayers)
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

            ExcludedPlayers = players.Except(teamPlayers).ToList();

            if (ExcludedPlayers != null && ExcludedPlayers.Intersect(forcePlayers).Count() > 0)
            {
                players.Shuffle();
                return GenerateTeams(players, forcePlayers);
            }

            return teams;
        }

        public void SetMatchResult(int team1Id, int team1Result, int team2Id, int team2Result)
        {
            var match = Matches.FirstOrDefault(m => m.Team1.Id == team1Id && m.Team2.Id == team2Id);
            //if(match != null)
              //  match.SetResult(team1Result, team2Result);
        }

        public bool AllMatchHaveResult()
        {
            if (Matches.Count == 0)
                return false;

            var match = Matches.FirstOrDefault(m => m.Team1Result == 0 && m.Team2Result == 0);
            return (match == null);
        }

        private void GenerateMatches(List<Team> teams, List<Player> forcePlayers)
        {
            // If not modulo 4 some teams should be excluded
            int extraTeamCount = teams.Count % 2;
            if (extraTeamCount > 0)
            {
                IEnumerable<Team>? teamsToRemove;
                int maxTry = 10;
                // Remove extra team at last of the list
                do
                {
                    teams.Shuffle();
                    teamsToRemove = teams.Skip(Math.Max(0, teams.Count() - extraTeamCount));
                    maxTry--;
                    // If teams to remove contains forcePlayers then do nothing and exclude other teams
                }
                while (teamsToRemove.Any(t => forcePlayers.Contains(t.Player1) || forcePlayers.Contains(t.Player2)) || maxTry > 0);

                foreach (var team in teamsToRemove)
                {
                    ExcludedPlayers.Add(team.Player1);
                    ExcludedPlayers.Add(team.Player2);
                    teams.Remove(team);
                }
            }

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

