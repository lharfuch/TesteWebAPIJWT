using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIJWT.Domain.Business.User;
using WebAPIJWT.Domain.Business.User.Entities;
using WebAPIJWT.Models;

namespace WebAPIJWT.Controllers
{
    public class UsuarioController : ApiController
    {
        IAccount _account = null;
        public UsuarioController(IAccount account)
        {
            _account = account;
        }

        [AllowAnonymous]
        [Route("api/v1/login")]
        [HttpPost]
        public HttpResponseMessage Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Valida Usuário
                UserAccount _validar = new UserAccount()
                {
                    UserName = model.Email,
                    Password = model.Password
                };
                Account _conta = new Account(new Domain.Db.User.UserDb());
                _validar = _conta.Login(_validar);

                if (_validar != null)
                {
                    UsuarioViewModel user = new UsuarioViewModel()
                    {
                        Id = _validar.Id,
                        user = _validar.UserName
                    };

                    var token = CreateToken(user);
                    return Request.CreateResponse(HttpStatusCode.OK, new { user, token });
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Usuario ou senha nao encontrato");
        }

        [AllowAnonymous]
        [Route("api/v1/criar")]
        [HttpPost]
        public HttpResponseMessage Criar(NovoUsuarioViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserAccount _novo = new UserAccount()
                    {
                        UserName = model.Email,
                        Password = model.Senha
                    };

                    Account _conta = new Account(new Domain.Db.User.UserDb());
                    _conta.New(_novo);
                    _novo = _conta.Login(_novo);
                    return Request.CreateResponse(HttpStatusCode.Created, _novo);
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Parametros incorretor");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        private static string CreateToken(UsuarioViewModel user)//, out object dbUser)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expiry = Math.Round((DateTime.UtcNow.AddMinutes(2) - unixEpoch).TotalSeconds);
            var issuedAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
            var notBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);

            var payload = new Dictionary<string, object>
            {
                {"user", user.user},
                {"name", user.user},
                {"userId", user.Id},
                {"sub", user.Id},
                {"iat", ToUnixTime(DateTime.Now).ToString()},
                {"exp", expiry}
            };

            var secret = ConfigurationManager.AppSettings.Get("secretKey");

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            //var token = JsonWebToken.Encode(payload, apikey, JwtHashAlgorithm.HS256);

            var token = encoder.Encode(payload, secret);

            //dbUser = new { user.user, user.Id };
            return token;
        }

        private static long ToUnixTime(DateTime dateTime)
        {
            return (int)(dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
