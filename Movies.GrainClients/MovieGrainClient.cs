using Movies.Contracts;
using Movies.Core;
using Movies.Models;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Movies.GrainClients
{
	public class MovieGrainClient : IMovieGrainClient
	{
		private readonly IGrainFactory _grainFactory;
		private readonly IMovieListGrainClient _movieListGrainClient;
		public MovieGrainClient(IGrainFactory grainFactory, IMovieListGrainClient movieListGrainClient)
		{
			_grainFactory = grainFactory;
			_movieListGrainClient = movieListGrainClient;
		}

		public async Task<MovieApiData> CreateMovie(MovieApiData movieApiData) 
		{
			try
			{
				//Check the parameter
				if (movieApiData == null)
				{
					throw new Exception($"Invalid movie data.");
				}
				else if (string.IsNullOrWhiteSpace(movieApiData.Key))
				{
					throw new Exception($"Invalid movie key.");
				}
				
				//Clean the key
				movieApiData.Key = movieApiData.Key.Trim().ToLower();

				//Ensure the movie does not exist
				var movieGrain = _grainFactory.GetGrain<IMovieGrain>(movieApiData.Key);
				var checkMovieInfo = await movieGrain.GetMovieDetails();		
				if (checkMovieInfo.Activated)
				{
					throw new Exception($"Movie ({movieApiData.Key}) already exists.");
				}

				//Create the movie
				MovieState movieState = await movieGrain.CreateOrUpdate(movieApiData);

				//Add the movie to the movie list
				MovieInfo movieInfo = new MovieInfo
				{
					Key = movieState.Key,
					Name = movieState.Name,
					Description = movieState.Description,
					Genres = movieState.Genres,
					Rate = movieState.Rate
				};
				await _movieListGrainClient.AddMovie(movieInfo);

				//Return the created movie
				return new MovieApiData
				{
					Key = movieState.Key,
					Name = movieState.Name,
					Description = movieState.Description,
					Genres = movieState.Genres,
					Rate = movieState.Rate,
					Length = movieState.Length,
					Img = movieState.Img
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

		public async Task<MovieApiData> UpdateMovie(MovieApiData movieApiData)
		{
			try
			{
				//Check the parameter
				if (movieApiData == null)
				{
					throw new Exception($"Invalid movie data.");
				}
				else if (movieApiData.Key.IsNullOrEmpty())
				{
					throw new Exception($"Invalid movie key.");
				}

				//Clean the key
				movieApiData.Key = movieApiData.Key.Trim().ToLower();

				//Check if movie already exists
				var movieGrain = _grainFactory.GetGrain<IMovieGrain>(movieApiData.Key);
				var checkMovieInfo = await movieGrain.GetMovieDetails();

				if (!checkMovieInfo.Activated)
				{
					throw new Exception($"Movie ({movieApiData.Key}) does not exist.");
				}
				
				//Update the movie
				MovieState movieState = await movieGrain.CreateOrUpdate(movieApiData);				

				//Update movie list about this new movie
				MovieInfo movieInfo = new MovieInfo
				{
					Key = movieState.Key,
					Name = movieState.Name,
					Description = movieState.Description,
					Genres = movieState.Genres,
					Rate = movieState.Rate
				};

				await _movieListGrainClient.AddMovie(movieInfo);

				//Return the updated movie
				return new MovieApiData
				{
					Key = movieState.Key,
					Name = movieState.Name,
					Description = movieState.Description,
					Genres = movieState.Genres,
					Rate = movieState.Rate,
					Length = movieState.Length,
					Img = movieState.Img
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
	}
}
