using Movies.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieListGrainClient
	{
		Task AddMovie(MovieInfo movieInfo);
		Task DeleteMovie(string movieId);
		Task<MovieApiData> GetMovieDetails(string movieKey);
		Task<List<MovieInfo>> GetAllMovies();
		Task<List<MovieInfo>> GetTopMovies(int topCount);
		Task<List<MovieInfo>> GetMoviesByGenre(string genre);
		Task<List<MovieInfo>> GetMoviesBySearch(string search);
	}
}
