// See https://aka.ms/new-console-template for more information
using TournoiSalade.Data;

Console.WriteLine("Hello, World!");

Player p1 = new Player() { Name = "Frédéric" };
Player p2 = new Player() { Name = "Anne-Sophie" };
Player p3 = new Player() { Name = "Camille" };
Player p4 = new Player() { Name = "Simon" };

List<Player> players = new List<Player>() { p1, p2, p3, p4 };

Tournament tournament = new Tournament();
tournament.Players = players;

for (int i = 0; i < 5; i++)
{
    tournament.NextTour();
    foreach (var match in tournament.CurrentTour.Matches)
    {
        match.SetResult(8, 4);
    }

    var ranks = tournament.GetPlayerRanks();
    foreach (var player in ranks)
    {
        Console.WriteLine($"Player: {player}, Score: {player.Score}");
    }
}

Console.WriteLine("========================");

Player p5 = new Player() { Name = "Jocelyne" };

players.Add(p5);

for (int i = 0; i < 5; i++)
{
    tournament.NextTour();
    foreach (var match in tournament.CurrentTour.Matches)
    {
        match.SetResult(8, 4);
    }

    var ranks = tournament.GetPlayerRanks();
    foreach (var player in ranks)
    {
        Console.WriteLine($"Player: {player}, Score: {player.Score}");
    }

}