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


    }
}
