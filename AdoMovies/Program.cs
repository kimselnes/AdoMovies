using AdoMovies.Data;
using AdoMovies.UI;

namespace AdoMovies
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=localhost;Database=AdoMoviesDb;Trusted_Connection=True;TrustServerCertificate=True;";

            var repository = new MovieRepository(connectionString);
            var menu = new MovieMenu(repository);

            menu.Run();

            Console.ReadLine();
        }
    }
}
