using AdoMovies.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdoMovies.Data
{
    public class MovieRepository
    {
        private readonly string _connectionString;

        public MovieRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Movie> GetAllMovies()
        {
            string sql = "" +
                "SELECT " +
                    "m.Id, " +
                    "m.Title, " +
                    "m.ReleaseYear, " +
                    "m.GenreId, " +
                    "g.Name AS GenreName " +
                "FROM Movies as m " +
                "INNER JOIN Genres AS g ON g.Id = m.GenreId " +
                "ORDER BY m.Title" +
                "";

            var movies = new List<Movie>();

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = new SqlCommand(sql, connection);


            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                movies.Add(MapMovie(reader));
                //string name = reader.GetString(1);
                //string year = reader.GetString(2);
                //int genreId = reader.GetInt32(3);

                //Console.WriteLine($"- {name} ({year})");
            }

            return movies;
        }

        public static Movie MapMovie(SqlDataReader reader)
        {
            return new Movie
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                ReleaseYear = reader.GetInt32(reader.GetOrdinal("ReleaseYear")),
                GenreId = reader.GetInt32(reader.GetOrdinal("GenreId")),
                Genre = new Genre
                {
                    Id = reader.GetInt32(reader.GetOrdinal("GenreId")),
                    Name = reader.GetString(reader.GetOrdinal("GenreName"))
                }
            };
        }
    }
}
