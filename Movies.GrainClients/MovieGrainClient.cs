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
				//Clean the key
				movieApiData.Key = movieApiData.Key.Trim().ToLower();

				var movieGrain = _grainFactory.GetGrain<IMovieGrain>(movieApiData.Key);
				MovieState movieState = await movieGrain.Update(movieApiData);

				// update movie list about this new movie
				MovieInfo movieInfo = new MovieInfo
				{
					Key = movieState.Key,
					Name = movieState.Name,
					Description = movieState.Description,
					Genres = movieState.Genres,
					Rate = movieState.Rate
				};

				await _movieListGrainClient.AddMovie(movieInfo);

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
					ErrorMessage = ex.Flatten()
				};
			}
		}		
	}
}
