using GraphQL.Types;
using Movies.Models;

namespace Movies.Server.Gql.Types
{
	public class MovieInputGraphType : InputObjectGraphType
	{
		public MovieInputGraphType()
		{
			Name = "MovieInput";

			Field<NonNullGraphType<StringGraphType>>("key");
			Field<NonNullGraphType<StringGraphType>>("name");
			Field<StringGraphType>("description");
			Field<NonNullGraphType<ListGraphType<StringGraphType>>>("Genres");			
			Field<DecimalGraphType>("rate");
			Field<StringGraphType>("length");
			Field<StringGraphType>("img");
		}
	}
}
