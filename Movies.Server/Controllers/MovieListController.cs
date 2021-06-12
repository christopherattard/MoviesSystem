using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Contracts;
using Movies.Core;
using Movies.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Server.Controllers
{
	/// <summary>
	/// Provides retrieval of movie entries from the movie list.
	/// </summary>
	[Route("api/[controller]")]
	[Authorize]
	public class MovieListController : Controller
	{ 
		private readonly IMovieListGrainClient _client;		
		
		public MovieListController(IMovieListGrainClient client, IAppInfo appInfo)
		{
			_client = client;			
		}

		/// <summary>
		/// Gets a list of all movies in the system.
		/// </summary>
		/// <returns>List of all movies.</returns>
		[HttpGet("")]
		//[ResponseCache(Duration = 30)]
		public async Task<List<MovieApiData>> GetAllMovies() => 
			await _client.GetAllMovies();

		/// <summary>
		/// Get details about a specific movie.
		/// </summary>
		/// <param name="movieKey">the key of the movie.</param>
		/// <returns>The details of the movie.</returns>
		[HttpGet("{movieKey}")]
		[ResponseCache(Duration = 30, VaryByQueryKeys = new string[] { "movieKey" })]
		public async Task<MovieApiData> GetMovieDetails(string movieKey) => 
			await _client.GetMovieDetails(movieKey).ConfigureAwait(false);

		/// <summary>
		/// Get those movies associated with the specified genre/s.
		/// </summary>
		/// <param name="genres">a string of genres separated by whitespaces or commas.</param>
		/// <returns>List of associated movies.</returns>
		[HttpGet("genre/{genres}")]
		[ResponseCache(Duration = 30, VaryByQueryKeys = new string[] { "genres" })]
		public async Task<List<MovieApiData>> GetMoviesByGenre(string genres) =>
			await _client.GetMoviesByGenre(genres).ConfigureAwait(false);		

		/// <summary>
		/// Get those movies associated with the search keywords.
		/// </summary>
		/// <param name="search">a string of search keywords separated by whitespaces or commas.</param>
		/// <returns>List of associated movies.</returns>
		[HttpGet("search/{search}")]
		[ResponseCache(Duration = 30, VaryByQueryKeys = new string[] { "search" })]
		public async Task<List<MovieApiData>> GetMoviesBySearch(string search) => 
			await _client.GetMoviesBySearch(search).ConfigureAwait(false);		

		/// <summary>
		/// Get the top movies by rate.
		/// </summary>
		/// <param name="topCount">the number of top movies to get.</param>
		/// <returns>list of top movies.</returns>
		[HttpGet("top/{topCount}")]
		[ResponseCache(Duration = 30, VaryByQueryKeys = new string[] { "topCount" })]
		public async Task<List<MovieApiData>> GetTopMovies(int topCount) =>
			await _client.GetTopMovies(topCount).ConfigureAwait(false);	
		
	}
}
