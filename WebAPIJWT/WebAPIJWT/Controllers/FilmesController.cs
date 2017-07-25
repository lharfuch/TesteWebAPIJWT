using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIJWT.Domain.Business.Movie;
using WebAPIJWT.Domain.Business.Movie.Entities;

namespace WebAPIJWT.Controllers
{
    [Authorize]
    public class FilmesController : ApiController
    {
        private IMovieCatalog _movie;
        public FilmesController(IMovieCatalog movie)
        {
            _movie = movie;
        }

        [Route("api/v1/listafilmes")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _movie.ListMovies());
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Erro ao selecionar lista");
            }
        }

        [Route("api/v1/listafilmes/{Id}")]
        [HttpGet]
        public HttpResponseMessage Get(int Id)
        {
            try
            {
                MovieDetail _mov = _movie.GetMovie(Id);
                if (_mov != null)
                    return Request.CreateResponse(HttpStatusCode.OK, _mov);
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Filme nao encontrado");
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Erro ao selecionar lista");
            }
        }
    }
}
