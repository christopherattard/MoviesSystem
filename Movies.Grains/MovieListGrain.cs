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
		private const string STRING_SEPARATORS = " ,";


		public async Task AddMovie(MovieInfo movieInfo)
		{
			try
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
			catch (Exception ex)
			{
				return;
			}			
		}

		public async Task DeleteMovie(string movieKey)
		{
			try
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
			catch (Exception ex)
			{
				return;
			}
		}

		public async Task<List<MovieInfo>> GetAllMovies()
		{
			try
			{
				if (State.MovieList == null)
				{
					State.MovieList = new List<MovieInfo>();
				}

				return State.MovieList;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<List<MovieInfo>> GetTopMovies(int topCount)
		{
			try
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
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<List<MovieInfo>> GetMoviesByGenre(string genres)
		{
			try
			{
				//Clean up the genres
				List<string> cleanGenres = _parseAndCleanString(genres);
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
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<List<MovieInfo>> GetMoviesBySearch(string search)
		{
			try
			{
				List<string> cleanWords = _parseAndCleanString(search);
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
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<MovieApiData> GetMovieDetails(string movieKey)
		{
			try
			{
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
			catch (Exception ex)
			{
				return null;
			}

		}

		private List<string> _parseAndCleanString(string str)
		{
			try
			{
				//Clean up the search words
				if (string.IsNullOrWhiteSpace(str))
				{
					return null;
				}

				List<string> cleanWords = new List<string>();
				var searchWords = str.Split(STRING_SEPARATORS.ToCharArray());
				foreach (string word in searchWords)
				{
					if (!string.IsNullOrWhiteSpace(word) && !cleanWords.Contains(word))
					{
						cleanWords.Add(word);
					}
				}

				if (cleanWords.Count == 0)
				{
					return null;
				}
				else
				{
					return cleanWords;
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}
	}
}
