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
		Task<List<MovieApiData>> GetAllMovies();
		Task<MovieApiData> GetMovieDetails(string movieKey);		
		Task<List<MovieApiData>> GetTopMovies(int topCount);
		Task<List<MovieApiData>> GetMoviesByGenre(string genre);
		Task<List<MovieApiData>> GetMoviesBySearch(string search);
	}
}
