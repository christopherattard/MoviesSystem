# Movies System
**Version: 1.0**

## Setup

1) Open Visual Studio and download the repository from: https://github.com/christopherattard/MoviesSystem

2) Locate the *app-info.json* configuration file and add the following keys:
	- "moviesPath" -> specify the full path of the movies.json file (for e.g. "*C:\\Projects\\MoviesSystem\\movies.json*");
	- "grainPrimaryKey" -> specify a string primary key for the MoviesListGrain (for e.g. "CA");
	- "corsOrigins" -> specify the host for the CORS policy (for e.g. "http://localhost:6600");
	- "symmetricKey" -> specify a symmetric key to be used for encrypting/decrypting the security token for the API. You can specify a GUID.
	- "apiUsername" -> specify the username for API authentication.
	- "apiPassword" -> specify the password for API authentication.

3) Update the following NuGet packages in Movies.Server project to the latest version:
	- GraphQL
	- GraphQL.Server.Transports.AspNetCore.NewtonsoftJson
	
4) Set Movies.Server project as the Startup project.

5) Go to the Movies.Tests project and open the PreloadGrainStartupTaskTests.cs file. Update the MOVIES_PATH constant string to the path of the movies.json file.
Run the Xunit unit tests found in this class. 	

6) Build and run the solution.

## Usage

Load the Swagger UI at the web application's root (for e.g. http://localhost:6600). From here one can find the definition of each API controller and method.

Open Postman and import the MoviesSystem collection file *MoviesSystem.postman_collection.json* found in the MoviesSystem solution folder. 
The format of this collection is Postman version 2.1. 

Start from the GetToken request to obtain a security token for the API. Specify the username and password (that were set up in the *app-info.json* config file) 
in the body of the POST request. The response returns a security token. Copy it and go to the MoviesSystem parent folder. Under Authorization tab, ensure that
Type is set to *Bearer Token*, paste the security token in the Token inputbox and press 'Save'. All the other API requests are set to inherit this authorization 
token.

You can now run the other requests one by one and observe their responses.


The application has the following functionality:

## Features

- **Home**
  - List top 5 highest rated movies
- **Movies List**
  - List Movies
  - Search
  - Filter by Genre
- **Movie detail**
  - Display selected movie detail information
- **Create Movie**
  - Create a new movie that can be retrieved in the movies list
- **Update Movie**
  - Update movies data.  

### Technologies user

- [ASP.NET (AspNetCore)](https://dotnet.microsoft.com/apps/aspnet) (3.1 or higher)
- [Microsoft Orleans](https://dotnet.github.io/orleans/) (3 or higher)
- [GraphQL](https://github.com/graphql-dotnet/graphql-dotnet) (3 or higher)

*You may use any 3rd party libraries which can facilitates your development.*

### Content

- A complete working solution with GraphQL and Orleans pre-configured. You do not need to create the boilerplate code yourself
- A `movies.json` with some mock data that can be used as your database (Although you might opt to use some other datasource)

### Running the sample application

- Make sure the startup project is set to `Movies.Server`
- The project has one controller `SampleDataController` that has to requests:
  - [GET] http://localhost:6600/api/sampledata/{id}
  - [POST] http://localhost:6600/api/sampledata/{id}
- There is also a Graph Query for the Application `AppGraphQuery` and one GraphType `SampleDataGraphType`
  - Accessible through: `http://localhost:6600/api/graphql`
  - Sample query:
      ```
      query sampleData($id: String!) {
          sample(id: $id) {
              id,
              name
          }
      }
      ```
- All the endpoints call one simple Grain called `SampleGrain` that holds the data on the Orleans server

### Helpful links
- [Orleans](https://dotnet.github.io/orleans/docs/grains/index.html)
- [GraphQL](https://graphql.org/learn/)
- [Docker](https://www.docker.com/)

### Extra Credit

- Pre-loading data in memory on App Start-up so it can be retrieved faster (using the required technologies)
- Use of good design patterns that avoid bottle necks
- Add Unit tests
- Rudimentary UI
- Dockerized application

If you get the demo in good shape and have extra time, add your own flair and features.

### Deliverable

- Provide a working application
- Provide source code in a public git such as github or Bitbucket repository
- Provide markdown readme file
  - General information about the app
  - Provide steps how to build/launch your application

Good luck!