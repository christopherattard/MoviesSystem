using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Contracts
{
	public interface IMovieGrainClient
	{		
		Task<MovieState> GetTopMovies(int topCount);
		Task<MovieState> GetAllMovies();
	}
}
