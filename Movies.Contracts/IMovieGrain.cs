using Movies.Models;
using Orleans;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieGrain : IGrainWithStringKey
	{
		Task<MovieState> Update(MovieApiData movieApiData);
		Task<MovieInfo> GetMovieDetails();
	}
}
