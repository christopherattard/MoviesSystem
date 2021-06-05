using Movies.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieListGrain
	{
		Task AddMovie(MovieInfo movieInfo);

		Task DeleteMovie(string movieId);

		Task<List<MovieInfo>> ListMovies();
	}
}
