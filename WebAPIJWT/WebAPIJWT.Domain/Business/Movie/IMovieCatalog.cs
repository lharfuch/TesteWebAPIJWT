using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIJWT.Domain.Business.Movie.Entities;

namespace WebAPIJWT.Domain.Business.Movie
{
    public interface IMovieCatalog
    {
        IEnumerable<MovieDetail> ListMovies();
        MovieDetail GetMovie(int Id);
    }
}
