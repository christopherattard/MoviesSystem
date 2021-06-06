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

		Task<List<MovieInfo>> ListMovies();
	}
}
