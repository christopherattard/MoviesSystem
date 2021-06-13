using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Movies.Contracts;
using Movies.Core;
using Movies.Models;
using System;
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
		private readonly IAppInfo _appInfo;
		private readonly IMemoryCache _memoryCache;
		private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;

		public MovieListController(IMovieListGrainClient client, IAppInfo appInfo, IMemoryCache memoryCache)
		{
			_client = client;
			_appInfo = appInfo;
			_memoryCache = memoryCache;
			_memoryCacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(15));
		}

		/// <summary>
		/// Gets a list of all movies in the system.
		/// </summary>
		/// <returns>List of all movies.</returns>
		[HttpGet("")]		
		public async Task<IActionResult> GetAllMovies()
		{			
			string cacheKey = $"all-{_appInfo.ApiUsername}";

			if (!_memoryCache.TryGetValue(cacheKey, out List<MovieApiData> result))
			{
				result = await _client.GetAllMovies();
				_memoryCache.Set(cacheKey, result, _memoryCacheEntryOptions);
			}
			
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
			if (string.IsNullOrWhiteSpace(movieKey))
			{
				return BadRequest("Invalid movie key.");
			}

			string cacheKey = $"details-{movieKey}-{_appInfo.ApiUsername}";

			if (!_memoryCache.TryGetValue(cacheKey, out MovieApiData result))
			{
				result = await _client.GetMovieDetails(movieKey);
				_memoryCache.Set(cacheKey, result, _memoryCacheEntryOptions);
			}
			
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
			if (string.IsNullOrWhiteSpace(genres))
			{
				return BadRequest("Invalid genres value.");
			}

			string cacheKey = $"genre-{genres}-{_appInfo.ApiUsername}";

			if (!_memoryCache.TryGetValue(cacheKey, out List<MovieApiData> result))
			{
				result = await _client.GetMoviesByGenre(genres);
				_memoryCache.Set(cacheKey, result, _memoryCacheEntryOptions);
			}
			
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
			if (string.IsNullOrWhiteSpace(search))
			{
				return BadRequest("Invalid search value.");
			}

			string cacheKey = $"search-{search}-{_appInfo.ApiUsername}";

			if (!_memoryCache.TryGetValue(cacheKey, out List<MovieApiData> result))
			{
				result = await _client.GetMoviesBySearch(search);
				_memoryCache.Set(cacheKey, result, _memoryCacheEntryOptions);
			}
			
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
			if (topCount == 0)
			{
				return BadRequest("Invalid topCount value.");
			}

			string cacheKey = $"top-{topCount}-{_appInfo.ApiUsername}";

			if (!_memoryCache.TryGetValue(cacheKey, out List<MovieApiData> result))
			{
				result = await _client.GetTopMovies(topCount);
				_memoryCache.Set(cacheKey, result, _memoryCacheEntryOptions);
			}
			
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
