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
		Task<List<MovieInfo>> GetAllMovies();
		Task<List<MovieInfo>> GetTopMovies(int topCount);
		Task<List<MovieInfo>> GetMoviesByGenre(List<string> genres);
	}
}
