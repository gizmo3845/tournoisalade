using System;
namespace TournoiSalade.Data
{
	public class Match
	{
		public Team? Team1 { get; set; }
		public Team? Team2 { get; set; }

		public int Team1Result { get; set; }
		public int Team2Result { get; set; }

		public void UpdatePlayerScore()
        {
			if (Team1Result > Team2Result)
            {
				Team1?.SetScore(true);
                Team2?.SetScore(false);
			}
            else
            {
                Team1?.SetScore(false);
                Team2?.SetScore(true);
			}
        }
	}
}

