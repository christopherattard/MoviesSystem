using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Models
{
	/// <summary>
	/// MovieApiData object
	/// </summary>
	public class MovieApiData
	{
		/// <summary>
		/// A unique key identifying a movie entry.
		/// </summary>
		public string Key { get; set; }
		/// <summary>
		/// Name of the movie.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Description of the movie.
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// List of genres that categorize this movie.
		/// </summary>
		public List<string> Genres { get; set; }
		/// <summary>
		/// Movie rating.
		/// </summary>
		public decimal Rate { get; set; }
		/// <summary>
		/// Length of the movie.
		/// </summary>
		public string Length { get; set; }
		/// <summary>
		/// Cover photo of the movie.
		/// </summary>
		public string Img { get; set; }
		/// <summary>
		/// Stores any error message raised during the API operation.
		/// </summary>
		public string ErrorMessage { get; set; }
	}
}
