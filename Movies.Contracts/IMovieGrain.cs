using Movies.Models;
using Orleans;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieGrain : IGrainWithStringKey
	{
		Task<MovieApiData> Update(MovieApiData movieApiData);
		Task<MovieApiData> GetMovieDetails();
	}
}
