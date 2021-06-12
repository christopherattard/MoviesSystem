using GraphiQl;
using GraphQL.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Movies.Core;
using Movies.GrainClients;
using Movies.Server.Gql;
using Movies.Server.Gql.App;
using Movies.Server.Infrastructure;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Server
{
	public class ApiStartup
	{
		private readonly IConfiguration _configuration;
		private readonly IAppInfo _appInfo;

		public ApiStartup(
			IConfiguration configuration,
			IAppInfo appInfo
		)
		{
			_configuration = configuration;
			_appInfo = appInfo;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//services.AddCustomAuthentication();
			var apiKey = Encoding.ASCII.GetBytes(_appInfo.ApiKey);
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(x =>
				{
					x.Events = new JwtBearerEvents
					{
						OnTokenValidated = context =>
						{
							return Task.CompletedTask;
						}
					};
					x.RequireHttpsMetadata = false;
					x.SaveToken = false;
					x.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(apiKey),
						ValidateIssuer = false,
						ValidateAudience = false
					};
				});
			
			services.AddCors(o => o.AddPolicy("TempCorsPolicy", builder =>
			{
				builder
					//.SetIsOriginAllowed((host) => true)
					.WithOrigins(_appInfo.CorsOrigins)
					.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowCredentials()
					;
			}));			

			// note: to fix graphql for .net core 3
			services.Configure<KestrelServerOptions>(options =>
			{
				options.AllowSynchronousIO = true;
			});

			services.AddAppClients();
			services.AddAppGraphQL();
			services.AddResponseCaching();
			services.AddControllers()
			.AddNewtonsoftJson();
			services.AddSwaggerGen();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(
			IApplicationBuilder app,
			IWebHostEnvironment env
		)
		{ 
			// add http for Schema at default url /graphql
			app.UseGraphQL<ISchema>();

			// use graphql-playground at default url /ui/playground
			app.UseGraphQLPlayground();

			//app.UseGraphQLEndPoint<AppSchema>("/graphql");

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseGraphiQl();
			}

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieSystem API Version 1.0");
				c.RoutePrefix = string.Empty;
			});

			app.UseRouting();

			app.UseCors("TempCorsPolicy");

			app.UseResponseCaching();

			app.Use(async (context, next) =>
			{
				context.Response.GetTypedHeaders().CacheControl =
					new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
					{
						Public = true,
						MaxAge = TimeSpan.FromSeconds(30)
					};
				context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
					new string[] { "Accept-Encoding" };

				await next();
			});

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}