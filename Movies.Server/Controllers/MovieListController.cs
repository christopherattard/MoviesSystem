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
		public async Task<IActionResult> GetAllMovies()
		{
			var result = await _client.GetAllMovies();
			return Ok(result);
		}

		/// <summary>
		/// Get details about a specific movie.
		/// </summary>
		/// <param name="movieKey">the key of the movie.</param>
		/// <returns>The details of the movie.</returns>
		[HttpGet("{movieKey}")]		
		public async Task<IActionResult> GetMovieDetails(string movieKey)
		{
			var result = await _client.GetMovieDetails(movieKey);
			return checkAndReturnResult(result);
		}

		/// <summary>
		/// Get those movies associated with the specified genre/s.
		/// </summary>
		/// <param name="genres">a string of genres separated by whitespaces or commas.</param>
		/// <returns>List of associated movies.</returns>
		[HttpGet("genre/{genres}")]		
		public async Task<IActionResult> GetMoviesByGenre(string genres)
		{
			var result = await _client.GetMoviesByGenre(genres);
			return Ok(result);
		}

		/// <summary>
		/// Get those movies associated with the search keywords.
		/// </summary>
		/// <param name="search">a string of search keywords separated by whitespaces or commas.</param>
		/// <returns>List of associated movies.</returns>
		[HttpGet("search/{search}")]		
		public async Task<IActionResult> GetMoviesBySearch(string search)
		{
			var result = await _client.GetMoviesBySearch(search);
			return Ok(result);
		}

		/// <summary>
		/// Get the top movies by rate.
		/// </summary>
		/// <param name="topCount">the number of top movies to get.</param>
		/// <returns>list of top movies.</returns>
		[HttpGet("top/{topCount}")]		
		public async Task<IActionResult> GetTopMovies(int topCount)
		{
			var result = await _client.GetTopMovies(topCount).ConfigureAwait(false);
			return Ok(result);
		}


		#region Helper methods

		private IActionResult checkAndReturnResult(MovieApiData result)
		{
			if (string.IsNullOrWhiteSpace(result.Errors))
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(new MovieApiData { Errors = result.Errors });
			}
		}	
			
		#endregion


	}
}
