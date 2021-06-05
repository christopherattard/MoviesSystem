using System.Collections.Generic;

namespace Movies.Models
{
	public class MovieListState
	{
		public List<MovieInfo> MovieList { get; set; }
	}

	public class MovieInfo
	{ 
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Rate { get; set; }
	}

}
