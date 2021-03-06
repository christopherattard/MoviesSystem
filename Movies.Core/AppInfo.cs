using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Movies.Core
{
	public interface IAppInfo
	{
		/// <summary>
		/// Get application name. e.g. '@odin/skeleton'.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets the application short name. e.g. 'skeleton'.
		/// </summary>
		string ShortName { get; }

		string ClusterId { get; set; }

		/// <summary>
		/// Get environment. e.g. 'Development'. (based on ASPNET_ENVIRONMENT, which can be mapped).
		/// </summary>
		string Environment { get; }

		/// <summary>
		/// Get git short commit hash. e.g. 'b603d6'
		/// </summary>
		string GitCommit { get; }

		/// <summary>
		/// Get application version. e.g. '1.1.0-staging'
		/// </summary>
		string Version { get; }

		/// <summary>
		/// Get whether the app is dockerized or not.
		/// </summary>
		bool IsDockerized { get; }

		/// <summary>
		/// Gets which service type is this app responsible of e.g. web, silo, etc...
		/// </summary>
		string ServiceType { get; set; }

		/// <summary>
		/// Gets the full path of the movies.json file.
		/// </summary>
		string MoviesPath { get; set; }

		/// <summary>
		/// Gets the movie list grain primary key.
		/// </summary>
		string GrainPrimaryKey { get; set; }
		
		/// <summary>
		/// The origins that are specified to the CORS policy.
		/// </summary>
		string CorsOrigins { get; set; }

		/// <summary>
		/// The symmetric key used for authentication.
		/// </summary>
		string SymmetricKey { get; set; }
		
		/// <summary>
		/// The username for API authentication
		/// </summary>
		string ApiUsername { get; set; }
		
		/// <summary>
		/// The password for API authentication
		/// </summary>
		string ApiPassword { get; set; }


	}

	public class AppInfo : IAppInfo
	{
		public string Name { get; set; }
		public string ShortName { get; }
		public string ClusterId { get; set; }
		public string Environment { get; set; }
		public string GitCommit { get; set; }
		public string Version { get; set; }
		public bool IsDockerized { get; set; }
		public string ServiceType { get; set; }
		public string MoviesPath { get; set; }
		public string GrainPrimaryKey { get; set; }
		public string CorsOrigins { get; set; }
		public string SymmetricKey { get; set; }
		public string ApiUsername { get; set; }
		public string ApiPassword { get; set; }

		private static readonly Dictionary<string, string> EnvironmentMapping = new Dictionary<string, string>
		{
			["Development"] = "dev",
			["Staging"] = "staging",
			["Production"] = "prod",
		};

		public AppInfo()
		{
		}

		/// <summary>
		/// Resolve from <see cref="IConfiguration"/>.
		/// </summary>
		/// <param name="config"></param>
		public AppInfo(IConfiguration config)
		{
			Name = config.GetValue("appName", "app");
			Version = config.GetValue("version", "local");
			GitCommit = config.GetValue("gitCommit", "-");
			Environment = config.GetValue<string>("ASPNETCORE_ENVIRONMENT");
			IsDockerized = config.GetValue<bool>("DOCKER");
			ServiceType = config.GetValue("serviceType", "dotnet");
			ShortName = Name.Split('/').Last();
			MoviesPath = config.GetValue("moviesPath", "");
			GrainPrimaryKey = config.GetValue("grainPrimaryKey", "");
			CorsOrigins = config.GetValue("corsOrigins", "");
			SymmetricKey = config.GetValue("symmetricKey", "");
			ApiUsername = config.GetValue("apiUsername", "");
			ApiPassword = config.GetValue("apiPassword", "");

			if (string.IsNullOrEmpty(Environment))
				throw new InvalidOperationException("Environment is not set. Please specify the environment via 'ASPNETCORE_ENVIRONMENT'");

			ClusterId = $"{Name}-{Version}";

			Environment = MapEnvironmentName(Environment);
		}

		public static string MapEnvironmentName(string environment)
		{
			if (environment == null) throw new ArgumentNullException(nameof(environment));

			EnvironmentMapping.TryGetValue(environment, out var env);
			return env;
		}
	}
}
