using System;
using System.Linq;
using TournoiSalade.Utils;

namespace TournoiSalade.Data
{
	public class Tour
	{
        private static Random _random = new Random();

        public List<Match> Matches { get; set; } = new();
        public List<Player> ExcludedPlayers { get; set; } = new();
        private int _nbPlayerPerTeam;

        public void New(int nbPlayerPerTeam)
        {
            _nbPlayerPerTeam = nbPlayerPerTeam;
            Matches?.Clear();
            ExcludedPlayers?.Clear();
        }

		public void Generate(int nbPlayerPerTeam, List<Player> players, List<Player> forcePlayers, out List<Player> excludedPlayers)
        {
            _nbPlayerPerTeam = nbPlayerPerTeam;

            ExcludedPlayers?.Clear();
            players.Shuffle();
            List<Team> teams = GenerateTeams(players, forcePlayers);
            GenerateMatches(teams, forcePlayers);

            excludedPlayers = ExcludedPlayers;
        }

        private List<Team> GenerateTeams(List<Player> players, List<Player> forcePlayers)
        {
            Random rng = new Random();
            players = players.OrderBy(a => rng.Next()).ToList();

            List<Team> teams = new List<Team>();

            int teamId = 1;
            for (int i = 0; i < players.Count / _nbPlayerPerTeam; i++)
            {
                var team = new Team() { Id = teamId++ };
                team.Teammates.AddRange(players.Skip(i * _nbPlayerPerTeam).Take(_nbPlayerPerTeam));
                teams.Add(team);
            }

            if(teams.Count % 2 != 0)
            {
                teams.Remove(teams.Last());
            }

            // If there are remaining players, distribute them into the teams only if a team has more than 2 players
            List<Player> remainingPlayers = players.Skip(teams.Count * _nbPlayerPerTeam).ToList();

            if (_nbPlayerPerTeam > 2)
            {
                ExcludedPlayers?.Clear();
                for (int j = 0; j < remainingPlayers.Count; j++)
                {
                    teams[j % teams.Count].Teammates.Add(remainingPlayers[j]);
                }
            }
            else
            {
                ExcludedPlayers = remainingPlayers;
                if (ExcludedPlayers.Intersect(forcePlayers).Any()) 
                { 
                    players.Shuffle(); 
                    return GenerateTeams(players, forcePlayers); 
                } 
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
            // If not modulo 2 some teams should be excluded
            int extraTeamCount = teams.Count % 2;
            if (extraTeamCount > 0)
            {
                IEnumerable<Team>? teamsToRemove;
                int maxTry = 10;
                // Remove extra team at last of the list
                bool forcedPlayers = false;
                do
                {
                    teams.Shuffle();
                    teamsToRemove = teams.Skip(Math.Max(0, teams.Count() - extraTeamCount));
                    maxTry--;
                    // If teams to remove contains forcePlayers then do nothing and exclude other teams
                    foreach (var teamToRemove in teamsToRemove)
                    {
                        if (teamToRemove.Teammates.Any(p => forcePlayers.Contains(p)))
                        {
                            forcedPlayers = true;
                            break;
                        }
                    }

                } while (forcedPlayers && maxTry > 0);

                foreach (var team in teamsToRemove)
                {
                    ExcludedPlayers.AddRange(team.Teammates);
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
                output += "(";
                foreach (var team1Teammate in match.Team1.Teammates)
                {
                    output += $"{team1Teammate},";
                }

                output = output.TrimEnd(',') + ") VS (";

                foreach (var team2Teammate in match.Team2.Teammates)
                {
                    output += $"{team2Teammate},";
                }
                output = output.TrimEnd(',') + ")" + Environment.NewLine;
            }

            return output;
        }
    }
}

