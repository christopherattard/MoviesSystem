using Movies.Contracts;
using Orleans;
using Orleans.Providers;
using System.Threading.Tasks;

namespace Movies.Grains
{
	[StorageProvider(ProviderName = "movie-store")]
	public class MovieGrain : Grain<MovieState>, IMovieGrain
	{
		
	}
}