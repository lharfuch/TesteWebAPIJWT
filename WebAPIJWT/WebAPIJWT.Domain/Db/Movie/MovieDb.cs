using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIJWT.Domain.Business.Movie.Entities;

namespace WebAPIJWT.Domain.Db.Movie
{
    public class MovieDb : DbContext
    {
        public MovieDb() : base("DefaultConnection")
        {

        }
        public DbSet<MovieDetail> MoviesDetails { get; set; }
    }
}
