using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TournoiSalade.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<ITournament, Tournament>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

Player p1 = new Player() { Name = "Frédéric" };
Player p2 = new Player() { Name = "Anne-Sophie" };
Player p3 = new Player() { Name = "Camille" };
Player p4 = new Player() { Name = "Simon" };

List<Player> players = new List<Player>() { p1, p2, p3, p4 };

var sp = builder.Services.BuildServiceProvider();

// This will succeed.
var tournament = sp.GetService<ITournament>();

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

app.Run();

