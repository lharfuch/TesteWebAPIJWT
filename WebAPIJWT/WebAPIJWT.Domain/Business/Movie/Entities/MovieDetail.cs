using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIJWT.Domain.Business.Movie.Entities
{
    public class MovieDetail
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Runtime { get; set; }
    }
}
