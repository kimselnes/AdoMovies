using AdoMovies.Data;
using AdoMovies.Models;
using Microsoft.Data.SqlClient;

namespace AdoMovies.UI
{
    public class MovieMenu
    {
        private readonly MovieRepository _movieRepository;

        public MovieMenu(MovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public void Run()
        {
            var isRunning = true;
            while (isRunning == true)
            {
                ShowMenu();

                string? choice2 = Console.ReadLine();

                switch (choice2)
                {
                    case "1":
                        ShowAllMovies(_movieRepository);
                        break;

                    case "2":
                        ShowMoviesByGenre(_movieRepository);
                        break;

                    case "3":
                        AddMovie(_movieRepository);
                        break;

                    case "4":
                        DeleteMovie(_movieRepository);
                        break;

                    case "0":
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Ogiltigt val. Tyck ENTER för att försöka igen.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("-- Filmer --");
            Console.WriteLine("1. Visa alla filmer");
            Console.WriteLine("2. Sök filmer efter genre");
            Console.WriteLine("3. Lägg till film");
            Console.WriteLine("4. Ta bort film");
            Console.WriteLine("0. Avsluta");
            Console.WriteLine();
            Console.Write("Välj: ");
        }

        private static void ShowAllMovies(MovieRepository movieRepository)
        {
            Console.Clear();
            Console.WriteLine("-- Alla filmer --");
            Console.WriteLine();
            Console.WriteLine("0. Gå tillbaka");
            Console.WriteLine();
            Console.WriteLine("- - - - - - - - - - - - - - - - - -");
            Console.WriteLine();

            // Implementera logik för att hämta och visa alla filmer från databasen här.
            //var movies = _movieRepository.GetAllMovies();
            //foreach (var movie in movies)
            //{
            //    Console.WriteLine($"- {movie.Title} ({movie.Year})");
            //}

            List<Movie> movies = movieRepository.GetAllMovies();

            if (movies.Count == 0)
            {
                Console.WriteLine("Det finns inga filmer");
                Console.ReadKey();
                return;
            }

            foreach (var movie in movies)
            {
                Console.WriteLine($"{movie.Title} ({movie.ReleaseYear}) - {movie.Genre?.Name}");
            }

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "0":
                    ShowMenu();
                    break;

                default:
                    Console.WriteLine("Ogiltigt val. Tyck ENTER för att försöka igen.");
                    Console.ReadLine();
                    break;
            }
        }

        private static void ShowMoviesByGenre(MovieRepository movieRepository)
        {
            Console.Clear();
            Console.WriteLine("-- Sök filmer efter genre --");
            Console.WriteLine();
            Console.WriteLine("0. Gå tillbaka");
            Console.WriteLine();
            Console.Write("Ange Genre: ");
            string genreName = Console.ReadLine();
            Console.WriteLine();

            if (string.IsNullOrWhiteSpace(genreName))
            {
                Console.WriteLine("\nDu angav fel genre.");
                Console.ReadKey();
                return;
            }

            var movies = movieRepository.GetMoviesByGenre(genreName);

            if (movies.Count == 0)
            {
                Console.WriteLine("Inga filmer hittades");
                Console.ReadKey();
                ShowMoviesByGenre(movieRepository);
                return;
            }

            Console.WriteLine("- - - - - - - - - - - - - - - - - -\n");

            foreach (var movie in movies)
            {
                Console.WriteLine($"{movie.Title} ({movie.ReleaseYear}) - {movie.Genre?.Name}");
            }

            Console.ReadKey();
        }

        private static void AddMovie(MovieRepository movieRepository)
        {
            Console.Clear();
            Console.WriteLine("-- Lägg till film --");
            Console.WriteLine();
            Console.Write("Titel: ");
            string title = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("\nTiteln får inte vara tom.");
                Console.ReadKey();
                return;
            }

            Console.Write("Utgivningsår: ");

            if (!int.TryParse(Console.ReadLine(), out int releaseYear))
            {
                Console.WriteLine("Ogiltigt årtal.");
                Console.ReadKey();
                return;
            }

            var genres = movieRepository.GetAllGenres();

            Console.WriteLine("\nTillgängliga genres:");

            foreach (var genre in genres)
            {
                Console.WriteLine($"{genre.Id}. {genre.Name}");
            }

            Console.Write("\nVälj genre-id: ");

            if (!int.TryParse(Console.ReadLine(), out int genreId) || 
                !genres.Any(g => g.Id == genreId))
            {
                Console.WriteLine("Ogiltigt genre-id.");
                Console.ReadKey();
                return;
            }

            var movie = new Movie
            {
                Title = title,
                ReleaseYear = releaseYear,
                GenreId = genreId
            };

            int affectedRows = movieRepository.AddMovie(movie);

            Console.WriteLine();
            Console.WriteLine(affectedRows == 1 ? "Filmen lades till." : "Filmen kunde inte läggas till.");
            Console.ReadKey();
        }

        private static void DeleteMovie(MovieRepository movieRepository)
        {
            Console.Clear();
            Console.WriteLine("-- Ta bort film --");
            Console.WriteLine();

            var movies = movieRepository.GetAllMovies();

            if (movies.Count == 0)
            {
                Console.WriteLine("Det finns inga filmer");
                Console.ReadKey();
                return;
            }

            foreach (var movie in movies)
            {
                Console.WriteLine($"{movie.Id}. {movie.Title} ({movie.ReleaseYear}) - {movie.Genre?.Name}");
            }
            Console.WriteLine("0. Gå tillbaka\n");

            Console.Write("Ange id för filmen som ska tas bort: ");

            if (!int.TryParse(Console.ReadLine(), out int movieId))
            {
                Console.WriteLine("Ogiltigt id.");
                Console.ReadKey();
                return;
            }

            if (movieId == 0)
            {
                var movieMenu = new MovieMenu(movieRepository);
                movieMenu.Run();
            }

            int affectedRows = movieRepository.DeleteMovie(movieId);

            Console.WriteLine();
            Console.WriteLine(affectedRows == 1 ? "Filmen togs bort." : "Ingen film med detta id hittades.");
            Console.ReadKey();
        }
    }
}
