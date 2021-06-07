using Movies.Contracts;
using Movies.Models;
using Orleans;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Grains
{
	[StorageProvider(ProviderName = "Default")]
	public class MovieListGrain : Grain<MovieListState>, IMovieListGrain
	{
		public async Task AddMovie(MovieInfo movieInfo)
		{
			Console.WriteLine($"-- MovieListGrain AddMovie() for movie [{movieInfo.Name}] with key #{movieInfo.Key} --");

			// ensure movie list is not null
			if (State.MovieList == null)
			{
				State.MovieList = new List<MovieInfo>();
			}

			// check to see if movie already exists, and if so, removed, before inserting new movie info
			int index = State.MovieList.FindIndex(movie => movie.Key == movieInfo.Key);
			if (index >= 0)
			{
				State.MovieList.RemoveAt(index);
			}

			// add this event key to our active list
			State.MovieList.Add(movieInfo);

			Console.WriteLine($"-- MovieListGrain.AddMovie() - call WriteStateAsync for new movie with key #{movieInfo.Key} --");
			await base.WriteStateAsync();
			
			return;
		}

		public async Task DeleteMovie(string movieKey)
		{
			Console.WriteLine($"-- MovieListGrain.DeleteMovie() for movie with key #{movieKey} --");

			// check args
			if (string.IsNullOrWhiteSpace(movieKey))
			{
				return;
			}

			if (State.MovieList == null)
			{
				State.MovieList = new List<MovieInfo>();
			}

			// delete this event key from our active list if it exists 
			int index = State.MovieList.FindIndex(movie => movie.Key == movieKey);
			if (index >= 0)
			{
				State.MovieList.RemoveAt(index);  // remove it

				Console.WriteLine($"** -- MovieListGrain.DeleteMovie() - call WriteStateAsync for deleted movie with key #{movieKey} --");
				await base.WriteStateAsync();
			}

			return;
		}

		public async Task<List<MovieInfo>> GetAllMovies()
		{
			Console.WriteLine($"-- MovieListGrain.ListMovies() --");

			if (State.MovieList == null)
			{
				State.MovieList = new List<MovieInfo>();
			}

			return State.MovieList;			
		}

		public async Task<List<MovieInfo>> GetTopMovies(int topCount)
		{
			Console.WriteLine($"-- MovieListGrain.ListTopMovies({topCount}) --");

			if (State.MovieList == null)
			{
				State.MovieList = new List<MovieInfo>();				
			}

			if (State.MovieList.Count == 0)
			{
				return State.MovieList;
			}

			List<MovieInfo> resultList = State.MovieList.OrderByDescending(x => x.Rate).Take(topCount).ToList();
			return resultList;
		}

		public async Task<List<MovieInfo>> GetMoviesByGenre(string genre)
		{
			//Clean up the genre
			if (string.IsNullOrWhiteSpace(genre))
			{ 
				return null;
			}

			string cleanGenre = genre.Trim().ToLower();
			if (string.IsNullOrWhiteSpace(cleanGenre))
			{
				return null;
			}
			
			Console.WriteLine($"-- MovieListGrain.GetMoviesByGenre({cleanGenre}) --");			

			if (State.MovieList == null)
			{
				State.MovieList = new List<MovieInfo>();
			}

			if (State.MovieList.Count == 0)
			{
				return State.MovieList;
			}

			List<MovieInfo> resultList = new List<MovieInfo>();

			foreach (MovieInfo movieInfo in State.MovieList)
			{
				if (movieInfo.Genres.Contains(cleanGenre))
				{
					resultList.Add(movieInfo);
				}				
			}

			return resultList;
		}

		public async Task<List<MovieInfo>> GetMoviesBySearch(string searchFilter)
		{
			Console.WriteLine($"-- MovieListGrain.GetMoviesBySearch({searchFilter}) --");

			if (State.MovieList == null)
			{
				State.MovieList = new List<MovieInfo>();
			}

			if (State.MovieList.Count == 0)
			{
				return State.MovieList;
			}

			List<MovieInfo> resultList = new List<MovieInfo>();

			foreach (MovieInfo movieInfo in State.MovieList)
			{
				if (movieInfo.Key.IndexOf(searchFilter) > -1)
				{
					resultList.Add(movieInfo);
				}
				
			}
			return resultList;
		}

		public async Task<MovieApiData> GetMovieDetails(string movieKey)
		{
			Console.WriteLine($"-- MovieListGrain.GetMovieDetails({movieKey}) --");
			MovieApiData movieApiData = null;
			
			if (State.MovieList == null)
			{
				State.MovieList = new List<MovieInfo>();
			}

			if (State.MovieList.Count == 0)
			{
				return movieApiData;
			}

			MovieInfo movieInfo = State.MovieList.Where(x => x.Key == movieKey).SingleOrDefault();
			if (movieInfo != null)
			{
				var movieGrain = GrainFactory.GetGrain<IMovieGrain>(movieKey);
				movieApiData = await movieGrain.GetMovieDetails();
				
				//Copy the Genres from the movieInfo - this could be improved by creating a Genre grain
				movieApiData.Genres = new List<string>();
				movieApiData.Genres.AddRange(movieInfo.Genres);
				return movieApiData;
			}
			else
			{
				return movieApiData;
			}

		}
	}
}
