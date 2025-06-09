using System;
using System.Collections.Generic;
using System.Linq;
using TournoiSalade.Utils;

namespace TournoiSalade.Data
{
	public class Tour
	{
        private static Random _random = new Random();

        public List<Match> Matches { get; set; } = new();
        public List<Player> ExcludedPlayers { get; set; } = new();
        private bool _isVache;

        public void New(bool isVache)
        {
            _isVache = isVache;
            Matches?.Clear();
            ExcludedPlayers?.Clear();
        }

		public void Generate(bool isVache, List<Player> players, List<Player> forcePlayers, out List<Player> excludedPlayers)
        {
            _isVache = isVache;

            ExcludedPlayers?.Clear();
            players.Shuffle();
            List<Team> teams = GenerateTeams(players, forcePlayers);
            GenerateMatches(teams, forcePlayers);

            excludedPlayers = ExcludedPlayers;
        }

        private List<Team> GenerateTeams(List<Player> players, List<Player> forcePlayers)
        {
            // Mix players randomly
            Random rng = new Random();
            players = players.OrderBy(a => rng.Next()).ToList();

            List<Team> teams = new List<Team>();

            if (! _isVache)
            {
                teams = GenerateTeams2(players, forcePlayers);
            }
            else
            {
                int n = players.Count;

                // 2. Trouver le meilleur nombre pair d’équipes couvrant tous les joueurs
                // Taille d’équipe entre 2 et 4, équilibrée
                teams = EqualizeTeams(players);
            }

            return teams;
        }

        private static List<Team> EqualizeTeams(List<Player> players)
        {
            int n = players.Count;
            List<Team> bestRepartition = null;

            for (int nbEquipes = n; nbEquipes >= 2; nbEquipes--)
            {
                if (nbEquipes % 2 != 0) continue;

                int baseTaille = n / nbEquipes;
                int reste = n % nbEquipes;

                if (baseTaille < 2) continue; // min 2 joueurs par équipe

                var teams = new List<Team>();
                int index = 0;

                for (int i = 0; i < nbEquipes; i++)
                {
                    int taille = baseTaille + (reste-- > 0 ? 1 : 0);

                    var team = new Team();
                    team.Teammates.AddRange(players.Skip(index).Take(taille).ToList());
                    teams.Add(team);

                    index += taille;
                }

                if (index == n)
                {
                    bestRepartition = teams;
                    break; // On prend la 1re répartition valide la plus équilibrée avec le plus d’équipes
                }
            }

            if (bestRepartition == null)
                throw new Exception("Impossible de répartir les joueurs correctement.");
        
            return bestRepartition;
        }

        private void GenerateMatches(List<Team> teams, List<Player> forcePlayers)
        {
            if (! _isVache)
            {
                GenerateMatches2(teams, forcePlayers);
                return;
            }

            Matches = new List<Match>();
            for (int i = 0; i < teams.Count; i += 2)
            {
                Match match = new Match() { Team1 = teams[i], Team2 = teams[i + 1] };
                Matches.Add(match);
            }
        }

        private List<Team> GenerateTeams2(List<Player> players, List<Player> forcePlayers)
        {
            Random rng = new Random();
            players = players.OrderBy(a => rng.Next()).ToList();

            List<Team> teams = new List<Team>();

            int teamId = 1;
            for (int i = 0; i < players.Count / 2; i++)
            {
                var team = new Team() { Id = teamId++ };
                team.Teammates.AddRange(players.Skip(i * 2).Take(2));
                teams.Add(team);
            }

            if(teams.Count % 2 != 0)
            {
                teams.Remove(teams.Last());
            }

            // If there are remaining players, distribute them into the teams only if a team has more than 2 players
            var remainingPlayers = players.Skip(teams.Count * 2);

            ExcludedPlayers = remainingPlayers.ToList();
            if (ExcludedPlayers.Intersect(forcePlayers).Any()) 
            { 
                players.Shuffle(); 
                return GenerateTeams2(players, forcePlayers); 
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

        private void GenerateMatches2(List<Team> teams, List<Player> forcePlayers)
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

