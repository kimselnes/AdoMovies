using AdoMovies.Data;
using System;
using System.Collections.Generic;
using System.Text;

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

                //if (int.TryParse(Console.ReadLine(), out int choice))
                //{
                //    //switch (choice)
                //}
                //else
                //{
                //    Console.WriteLine("Ogiltigt val. Tyck ENTER för att försöka igen.");
                //    Console.ReadLine();
                //}

                string? choice2 = Console.ReadLine();

                switch (choice2)
                {
                    case "1":
                        ShowAllMovies();
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

        private static void ShowAllMovies()
        {

        }
    }
}
