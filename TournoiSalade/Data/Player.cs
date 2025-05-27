using System;
namespace TournoiSalade.Data
{
	public class Player
	{
		public string Name { get; set; }
		public int Score { get; set; }

        public bool IsChild { get; set; }

        public void AddScore(bool winner)
        {
            if (winner)
                Score += IsChild ? 4 : 3;
            else
                Score += IsChild ? 1 : 0;
        }

        public override bool Equals(object? player)
        {
			if (player == null)
				return false;

			Player? p = (Player?) player;
			return p.Name.Equals(Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

