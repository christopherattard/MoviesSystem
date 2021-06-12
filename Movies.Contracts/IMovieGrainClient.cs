using Movies.Models;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieGrainClient
	{
		Task<MovieApiData> CreateMovie(MovieApiData movieApiData);
		Task<MovieApiData> UpdateMovie(MovieApiData movieApiData);
	}
}
