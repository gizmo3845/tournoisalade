using System;
namespace TournoiSalade.Data
{
	public class Team
	{
		public int Id { get; set; }

		public List<Player> Teammates { get; set; } = new List<Player>();

		public void SetScore(bool winner)
        {
            foreach (var player in Teammates)
            {
                player.AddScore(winner);
            }
        }
    }
}

