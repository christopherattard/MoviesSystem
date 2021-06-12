using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Movies.Contracts;
using Movies.Core;
using Movies.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Server.Controllers
{
	/// <summary>
	/// Provides functionality to obtain access to the MovieSystem API together with addition and modification of movie entries.
	/// </summary>
	[Route("api/[controller]")]
	[Authorize]
	public class MovieController : Controller
	{
		private readonly IMovieGrainClient _client;
		private readonly IAppInfo _appInfo;
		private const byte SECURITYTOKEN_EXPIRYHOURS = 6;
		public MovieController(IMovieGrainClient client, IAppInfo appInfo)
		{
			_client = client;
			_appInfo = appInfo;
		}

		/// <summary>
		/// Gets a security token for API authorization.
		/// </summary>
		/// <param name="loginModel">a <see cref="LoginModel"/> object specified in the body.</param>
		/// <returns>a security token.</returns>
		[HttpPost("getToken")]
		[AllowAnonymous]
		public async Task<string> GetToken([FromBody] LoginModel loginModel)
		{
			try
			{
				if (loginModel != null && loginModel.Username == _appInfo.ApiUsername && loginModel.Password == _appInfo.ApiPassword)
				{
					var tokenHandler = new JwtSecurityTokenHandler();
					var symmetricKey = Encoding.ASCII.GetBytes(_appInfo.SymmetricKey);
					var tokenDescriptor = new SecurityTokenDescriptor
					{
						Subject = new ClaimsIdentity(new Claim[]
						{
						new Claim(ClaimTypes.Name, loginModel.Username)
						}),
						Expires = DateTime.UtcNow.AddHours(SECURITYTOKEN_EXPIRYHOURS),
						SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
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

		/// <summary>
		/// Create a movie entry and add it to the movie system.
		/// </summary>
		/// <param name="movieApiData">MovieApiData instance with the movie information.</param>
		/// <returns>The created movie.</returns>
		[HttpPost]		
		public async Task<MovieApiData> CreateMovie([FromBody] MovieApiData movieApiData)
		{
			//Clean the key
			movieApiData.Key = movieApiData.Key.Trim().ToLower();

			var result = await _client.CreateMovie(movieApiData).ConfigureAwait(false);
			return result;
		}

		/// <summary>
		/// Updates a movie entry.
		/// </summary>
		/// <param name="movieApiData">MovieApiData instance with the updated information.</param>
		/// <returns>The updated movie.</returns>
		[HttpPut]
		public async Task<MovieApiData> UpdateMovie([FromBody] MovieApiData movieApiData)
		{
			//Clean the key
			movieApiData.Key = movieApiData.Key.Trim().ToLower();

			var result = await _client.UpdateMovie(movieApiData).ConfigureAwait(false);
			return result;
		}		
	}
}
