using AdoMovies.Data;
using AdoMovies.UI;
using Microsoft.Data.SqlClient;

namespace AdoMovies
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // ÄNDRA NAMNEN PÅ TABLES I SQL SERVER!

            string connectionString = "Server=localhost;Database=AdoMoviesDb;Trusted_Connection=True;TrustServerCertificate=True;";

            //using var connection = new SqlConnection(connectionString);
            //connection.Open();

            var repository = new MovieRepository(connectionString);
            var menu = new MovieMenu(repository);

            menu.Run();

            Console.ReadLine();
        }
    }
}
