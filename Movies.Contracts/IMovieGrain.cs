using Movies.Models;
using Orleans;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieGrain : IGrainWithStringKey
	{
		Task<MovieState> Update(MovieState movieState);
		Task Delete(string movieId);
	}
}
