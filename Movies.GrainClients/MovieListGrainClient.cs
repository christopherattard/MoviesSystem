using Movies.Contracts;
using Movies.Core;
using Movies.Models;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.GrainClients
{
	public class MovieListGrainClient : IMovieListGrainClient
	{
		private readonly IGrainFactory _grainFactory;		
		private readonly string _grainPrimaryKey = "";

		public MovieListGrainClient(IAppInfo appInfo, IGrainFactory grainFactory)
		{
			_grainFactory = grainFactory;
			_grainPrimaryKey = appInfo.GrainPrimaryKey;
		}

		public Task AddMovie(MovieInfo movieInfo)
		{
			try
			{
				var grain = _grainFactory.GetGrain<IMovieListGrain>(_grainPrimaryKey);
				return grain.AddMovie(movieInfo);
			}
			catch (Exception ex)
			{
				return Task.FromException(ex);
			}
		}		

		public async Task<List<MovieApiData>> GetAllMovies()
		{
			try
			{
				var grain = _grainFactory.GetGrain<IMovieListGrain>(_grainPrimaryKey);
				List<MovieInfo> movieInfoList = await grain.GetAllMovies();
				return convertToApiData(movieInfoList);
			}
			catch (Exception ex)
			{
				return sendErrorMessageInList(ex);
			}
		}

		public async Task<List<MovieApiData>> GetTopMovies(int topCount)
		{
			try
			{
				var grain = _grainFactory.GetGrain<IMovieListGrain>(_grainPrimaryKey);
				List<MovieInfo> movieInfoList = await grain.GetTopMovies(topCount);
				return convertToApiData(movieInfoList);
			}
			catch (Exception ex)
			{
				return sendErrorMessageInList(ex);
			}

		}

		public async Task<List<MovieApiData>> GetMoviesBySearch(string search)
		{
			try
			{
				var grain = _grainFactory.GetGrain<IMovieListGrain>(_grainPrimaryKey);
				List<MovieInfo> movieInfoList = await grain.GetMoviesBySearch(search);
				return convertToApiData(movieInfoList);
			}
			catch (Exception ex)
			{
				return sendErrorMessageInList(ex);
			}
		}

		public async Task<List<MovieApiData>> GetMoviesByGenre(string genres)
		{
			try
			{
				var grain = _grainFactory.GetGrain<IMovieListGrain>(_grainPrimaryKey);
				List<MovieInfo> movieInfoList = await grain.GetMoviesByGenre(genres);
				return convertToApiData(movieInfoList);
			}
			catch (Exception ex)
			{
				return sendErrorMessageInList(ex);
			}
		}

		public async Task<MovieApiData> GetMovieDetails(string movieKey)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(movieKey))
				{
					throw new Exception("Invalid movie key.");
				}
				
				//Clean the key
				movieKey = movieKey.Trim().ToLower();

				var grain = _grainFactory.GetGrain<IMovieListGrain>(_grainPrimaryKey);
				var movieInfo = await grain.GetMovieDetails(movieKey);

				if (movieInfo == null)
				{
					throw new Exception($"Movie ({movieKey}) not found.");
				}
				
				return convertToApiData(movieInfo);
			}
			catch (Exception ex)
			{
				return new MovieApiData
				{
					Errors = ex.Flatten()
				};
			}
		}

		#region Helper methods
		private List<MovieApiData> convertToApiData(List<MovieInfo> movieInfoList)
		{
			List<MovieApiData> movieApiDataList = new List<MovieApiData>();
			try
			{				
				foreach (var movieInfo in movieInfoList)
				{
					movieApiDataList.Add(convertToApiData(movieInfo));
				}
				return movieApiDataList;
			}
			catch (Exception ex)
			{
				var movieApiData = new MovieApiData
				{
					Errors = ex.Flatten()
				};
				movieApiDataList.Add(movieApiData);
				return movieApiDataList;
			}
		}

		private MovieApiData convertToApiData(MovieInfo movieInfo)
		{
			try
			{
				List<string> genres = new List<string>();
				genres.AddRange(movieInfo.Genres);

				return new MovieApiData
				{
					Key = movieInfo.Key,
					Name = movieInfo.Name,
					Description = movieInfo.Description,
					Genres = genres,
					Rate = movieInfo.Rate,
					Length = "",
					Img = "",
					Errors = ""
				};
			}
			catch (Exception ex)
			{
				return new MovieApiData 
				{ 
					Errors = ex.Flatten() 
				};
			}
		}

		private List<MovieApiData> sendErrorMessageInList(Exception ex)
		{
			List<MovieApiData> movieApiDataList = new List<MovieApiData>
			{
				new MovieApiData { Errors = ex.Flatten() }
			};
			return movieApiDataList;
		}
		#endregion
	}
}
