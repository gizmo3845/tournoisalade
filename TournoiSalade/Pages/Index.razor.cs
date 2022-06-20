using System;
using Microsoft.AspNetCore.Components;
using TournoiSalade.Data;

namespace TournoiSalade.Pages
{
	public partial class Index : ComponentBase
    {
		[Inject]
		protected ITournament tournament { get; set; }

		public Index()
		{
		}
	}
}

