# Movies Microservice
**Version: 1.0**
## Scenario

You are tasked with creating an API for an application that the company is building for a client. The application is a movies indexing application that will have high volumes of traffic and thus it needs to be fast, efficient and robust while being secure at the same time.

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

### Technologies required

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