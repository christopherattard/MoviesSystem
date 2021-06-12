using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Movies.Contracts;
using Movies.Core;
using Movies.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Server.Controllers
{
	[Route("api/[controller]")]
	[Authorize]
	public class MovieListController : Controller
	{ 
		private readonly IMovieListGrainClient _client;
		private readonly IAppInfo _appInfo;
		
		public MovieListController(IMovieListGrainClient client, IAppInfo appInfo)
		{
			_client = client;
			_appInfo = appInfo;
		}

		[HttpGet("")]
		[ResponseCache(Duration = 30)]
		public async Task<List<MovieApiData>> GetAllMovies() => 
			await _client.GetAllMovies();

		[HttpGet("{movieKey}")]
		[ResponseCache(Duration = 30, VaryByQueryKeys = new string[] { "movieKey" })]
		public async Task<MovieApiData> GetMovieDetails(string movieKey) => 
			await _client.GetMovieDetails(movieKey).ConfigureAwait(false);

		[HttpGet("genre/{genre}")]
		[ResponseCache(Duration = 30, VaryByQueryKeys = new string[] { "genre" })]
		public async Task<List<MovieApiData>> GetMoviesByGenre(string genre) =>
			await _client.GetMoviesByGenre(genre).ConfigureAwait(false);		

		[HttpGet("search/{search}")]
		[ResponseCache(Duration = 30, VaryByQueryKeys = new string[] { "search" })]
		public async Task<List<MovieApiData>> GetMoviesBySearch(string search) => 
			await _client.GetMoviesBySearch(search).ConfigureAwait(false);		

		[HttpGet("top/{topCount}")]
		[ResponseCache(Duration = 30, VaryByQueryKeys = new string[] { "topCount" })]
		public async Task<List<MovieApiData>> GetTopMovies(int topCount) =>
			await _client.GetTopMovies(topCount).ConfigureAwait(false);	

		[HttpPost("getToken")]
		[AllowAnonymous]
		public async Task<string> GetToken([FromBody] LoginModel loginModel)
		{
			try
			{
				if (loginModel != null && loginModel.Username == _appInfo.ApiUsername && loginModel.Password == _appInfo.ApiPassword)
				{
					var tokenHandler = new JwtSecurityTokenHandler();
					var apiKey = Encoding.ASCII.GetBytes(_appInfo.ApiKey);
					var tokenDescriptor = new SecurityTokenDescriptor
					{
						Subject = new ClaimsIdentity(new Claim[]
						{
						new Claim(ClaimTypes.Name, loginModel.Username)
						}),
						Expires = DateTime.UtcNow.AddHours(6),
						SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(apiKey), SecurityAlgorithms.HmacSha256Signature)
					};
					var token = tokenHandler.CreateToken(tokenDescriptor);
					var tokenString = tokenHandler.WriteToken(token);

					return tokenString;

				}
				return "Unauthorized access.";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
	}
}
