using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Contracts;
using Movies.Models;
using System.Threading.Tasks;

namespace Movies.Server.Controllers
{
	[Route("api/[controller]")]
	[Authorize]
	public class MovieController : Controller
	{
		private readonly IMovieGrainClient _client;
		public MovieController(IMovieGrainClient client)
		{
			_client = client;
		}		

		[HttpPost]
		public async Task<MovieApiData> CreateMovie([FromBody] MovieApiData movieApiData)
		{
			//Clean the key
			movieApiData.Key = movieApiData.Key.Trim().ToLower();

			var result = await _client.CreateMovie(movieApiData).ConfigureAwait(false);
			return result;
		}

		[HttpPut]
		public async Task<MovieApiData> UpdateMovie([FromBody] MovieApiData movieApiData)
		{
			//Clean the key
			movieApiData.Key = movieApiData.Key.Trim().ToLower();

			var result = await _client.CreateMovie(movieApiData).ConfigureAwait(false);
			return result;
		}		
	}
}
