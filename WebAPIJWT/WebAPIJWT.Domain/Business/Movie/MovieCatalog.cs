using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIJWT.Domain.Business.Movie.Entities;
using WebAPIJWT.Domain.Db.Movie;

namespace WebAPIJWT.Domain.Business.Movie
{
    public class MovieCatalog : IMovieCatalog
    {
        MovieDb _db = null;
        public MovieCatalog(MovieDb db)
        {
            _db = db;
        }

        public MovieDetail GetMovie(int Id)
        {
            return _db.MoviesDetails.Where(m => m.Id == Id).FirstOrDefault<MovieDetail>();
        }

        public IEnumerable<MovieDetail> ListMovies()
        {
           return _db.MoviesDetails.ToList();
        }
    }
}
