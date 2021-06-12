using Movies.Models;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieListGrain : IGrainWithStringKey
	{
		Task AddMovie(MovieInfo movieInfo);
		Task DeleteMovie(string movieId);
		Task<List<MovieInfo>> GetAllMovies();
		Task<List<MovieInfo>> GetTopMovies(int topCount);
		Task<List<MovieInfo>> GetMoviesByGenre(string genre);
		Task<List<MovieInfo>> GetMoviesBySearch(string search);
		Task<MovieInfo> GetMovieDetails(string movieKey);
	}
}
