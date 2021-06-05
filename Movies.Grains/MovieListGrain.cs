using Movies.Contracts;
using Movies.Models;
using Orleans;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Grains
{
	[StorageProvider(ProviderName = "grain-store")]
	public class MovieListGrain : Grain<MovieListState>, IMovieListGrain
	{
		public async Task AddMovie(MovieInfo movieInfo)
		{
			Console.WriteLine($"-- MovieListGrain AddMovie() for movie #{movieInfo.Id} [{movieInfo.Name}] --");

			// ensure movie list is not null
			if (State.MovieList == null)
			{
				State.MovieList = new List<MovieInfo>();
			}

			// check to see if movie already exists, and if so, removed, before inserting new movie info
			int index = State.MovieList.FindIndex(movie => movie.Id == movieInfo.Id);
			if (index >= 0)
			{
				State.MovieList.RemoveAt(index);
			}

			// add this event key to our active list
			State.MovieList.Add(movieInfo);

			Console.WriteLine($"-- MovieListGrain.AddMovie() - call WriteStateAsync for new movie #{movieInfo.Id} --");
			return base.WriteStateAsync();			
		}

		public async Task DeleteMovie(string movieId)
		{
			Console.WriteLine($"-- MovieListGrain.DeleteMovie() for movie #{movieId} --");

			// check args
			if (String.IsNullOrWhiteSpace(movieId))
			{
				return;
			}

			if (State.MovieList == null)
			{
				State.MovieList = new List<MovieInfo>();
			}

			// delete this event key from our active list if it exists 
			int index = State.MovieList.FindIndex(movie => movie.Id == movieId);
			if (index >= 0)
			{
				State.MovieList.RemoveAt(index);  // remove it

				Console.WriteLine($"** -- MovieListGrain.DeleteMovie() - call WriteStateAsync for deleted movie #{movieId} --");
				await base.WriteStateAsync();
			}

			return;
		}

		public async Task<List<MovieInfo>> ListMovies()
		{
			Console.WriteLine($"-- MovieListGrain.ListMovies() --");

			if (State.MovieList == null)
			{
				State.MovieList = new List<MovieInfo>();
			}

			return State.MovieList;			
		}
	}
}
