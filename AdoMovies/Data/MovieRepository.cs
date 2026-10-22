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
                "ORDER BY m.Id" +
                "";

            var movies = new List<Movie>();

            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = new SqlCommand(sql, connection);


            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                movies.Add(MapMovie(reader));
            }

            return movies;
        }

        public List<Genre> GetAllGenres()
        {
            string sql = "" +
                "SELECT Id, Name " +
                "FROM Genres " +
                "ORDER BY Id;";

            var genres = new List<Genre>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            connection.Open();

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                genres.Add(new Genre
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name"))
                });
            }

            return genres;
        }

        public List<Movie> GetMoviesByGenre(string genreName)
        {
            string sql = "" +
                "SELECT " +
                    "m.Id, " +
                    "m.Title, " +
                    "m. ReleaseYear, " +
                    "m.GenreId, " +
                    "g.Name AS GenreName " +
                "FROM Movies as m " +
                "INNER JOIN Genres AS g ON g.Id = m.GenreId " +
                "WHERE g.Name = @GenreName " +
                "ORDER BY m.Title" +
                "";

            var movies = new List<Movie>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@GenreName", genreName);
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                movies.Add(MapMovie(reader));
            }

            return movies;
        }

        public int AddMovie(Movie movie)
        {
            string sql = "" +
                "INSERT INTO Movies (Title, ReleaseYear, GenreId) " +
                "VALUES (@Title, @ReleaseYear, @GenreId);";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Title", movie.Title);
            command.Parameters.AddWithValue("@ReleaseYear", movie.ReleaseYear);
            command.Parameters.AddWithValue("@GenreId", movie.GenreId);
            connection.Open();

            return command.ExecuteNonQuery();
        }

        public int DeleteMovie(int movieId)
        {
            string sql = "" +
                "DELETE FROM Movies " +
                "WHERE Id = @MovieId;";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@MovieId", movieId);
            connection.Open();

            return command.ExecuteNonQuery();
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
