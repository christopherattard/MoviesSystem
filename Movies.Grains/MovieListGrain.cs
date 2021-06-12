using Movies.Contracts;
using Movies.Core;
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

			await base.WriteStateAsync();

			return;
		}

		public async Task DeleteMovie(string movieKey)
		{

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

				await base.WriteStateAsync();
			}

			return;
		}

		public async Task<List<MovieInfo>> GetAllMovies()
		{

			if (State.MovieList == null)
			{
				State.MovieList = new List<MovieInfo>();
			}

			return State.MovieList;

		}

		public async Task<List<MovieInfo>> GetTopMovies(int topCount)
		{

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

		public async Task<List<MovieInfo>> GetMoviesByGenre(string genres)
		{
			//Clean up the genres
			List<string> cleanGenres = genres.SplitDistinct();
			if (cleanGenres == null || cleanGenres.Count == 0)
			{
				return null;
			}

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
				foreach (var genre in cleanGenres)
				{
					if (movieInfo.Genres.Contains(genre) && !resultList.Contains(movieInfo))
					{
						resultList.Add(movieInfo);
					}
				}
			}

			return resultList;
		}

		public async Task<List<MovieInfo>> GetMoviesBySearch(string search)
		{
			List<string> cleanWords = search.SplitDistinct();
			if (cleanWords == null || cleanWords.Count == 0)
			{
				return null;
			}

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
				foreach (var word in cleanWords)
				{
					if (movieInfo.Key.IndexOf(word) > -1 && !resultList.Contains(movieInfo))
					{
						resultList.Add(movieInfo);
					}
				}
			}
			return resultList;
		}

		public async Task<MovieInfo> GetMovieDetails(string movieKey)
		{		
			if (State.MovieList == null)
			{
				State.MovieList = new List<MovieInfo>();
			}

			if (State.MovieList.Count == 0)
			{
				return null;
			}

			return State.MovieList.Where(x => x.Key == movieKey).SingleOrDefault();			
		}		
	}
}
