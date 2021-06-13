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

Go to the MoviesSystem collection folder and under Variables tab set the variable *hostname* value to the host address (for e.g. http://localhost:6600). Press 
the 'Save' button.

Start from the GetToken request to obtain a security token for the API. Specify the username and password (that were set up in the *app-info.json* config file) 
in the body of the POST request. The response returns a security token. Copy it and go to the MoviesSystem parent folder. Under Authorization tab, ensure that
Type is set to *Bearer Token*, paste the security token in the Token inputbox and press the 'Save' button. All the other API requests are set to inherit this 
authorization token.

You can now run the other requests one by one and observe their responses.

## Features

  1) A MovieController API containing:
  - method for obtaining security token for API authentication;
  - method for creating a movie entry;
  - method for updating a movie entry.

  2) A MovieListController API containing:
  - method for obtaining all movie entries;
  - method for getting a specific movie detail;
  - method for getting the top N movies by rating;
  - method for getting movies that fall under the specified genres;
  - method for obtaining movies that are asoociated with specified search words.

  3) Documentation of the APIs is done with Swagger UI.

  4) GraphQL queries for all the web methods defined in the MovieListController API. These can be executed from Postman.

  5) GraphQL mutations for creating and updating a movie entry. These can be executed from Postman.

  6) PreloadGrainStartupTask.cs class loads the contents of movies.json file in the MovieList grain on startup.

  7) In-memory cache is implemented on the web methods defined in the MovieListController API.

  8) A small Xunit unit test project defines some unit tests on the movies.json file.
