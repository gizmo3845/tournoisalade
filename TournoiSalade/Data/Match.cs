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
				Team1.Player1.Score += Team1.Player1.IsChild ? 4 : 3;
				Team1.Player2.Score += Team1.Player2.IsChild ? 4 : 3;

				Team2.Player1.Score += Team2.Player1.IsChild ? 1 : 0;
				Team2.Player2.Score += Team2.Player2.IsChild ? 1 : 0;
			}
            else
            {
				Team2.Player1.Score += Team2.Player1.IsChild ? 4 : 3;
				Team2.Player2.Score += Team2.Player2.IsChild ? 4 : 3;

				Team1.Player1.Score += Team1.Player1.IsChild ? 1 : 0;
				Team1.Player2.Score += Team1.Player2.IsChild ? 1 : 0;
			}
        }

	}
}

