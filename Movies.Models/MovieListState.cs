using System.Collections.Generic;

namespace Movies.Models
{
	public class MovieListState
	{
		public List<MovieInfo> MovieList { get; set; }
	}

	public class MovieInfo
	{ 		
		public string Key { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Rate { get; set; }
		public List<string> Genres { get; set; }
	}

	/// <summary>
	/// This class is used only for deserialization from movies.json file.
	/// </summary>
	public class Root
	{
		public List<MovieState> movies { get; set; }
	}

}
