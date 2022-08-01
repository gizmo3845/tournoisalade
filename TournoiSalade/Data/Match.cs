﻿using System;
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
				Team1.Player1.Score += 3;
				Team1.Player2.Score += 3;
			}
            else
            {
				Team2.Player1.Score += 3;
				Team2.Player2.Score += 3;
			}
        }

	}
}

