using Movies.Models;
using Orleans;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieGrain : IGrainWithStringKey
	{
		Task<MovieState> CreateOrUpdate(MovieApiData movieApiData);
		Task<MovieInfo> GetMovieDetails();
	}
}
