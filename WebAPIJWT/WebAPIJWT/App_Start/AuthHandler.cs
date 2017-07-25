using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using JWT;
using JWT.Serializers;
using System.Configuration;

namespace WebAPIJWT
{
    internal class AuthHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                    CancellationToken cancellationToken)
        {
            HttpResponseMessage errorResponse = null;
            try
            {
                IEnumerable<string> authHeaderValues;
                request.Headers.TryGetValues("Authorization", out authHeaderValues);

                if (authHeaderValues == null)
                    return base.SendAsync(request, cancellationToken);

                var bearerToken = authHeaderValues.ElementAt(0);
                var token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;

                var secret = ConfigurationManager.AppSettings.Get("secretKey");

                Thread.CurrentPrincipal = ValidateToken(
                    token,
                    secret,
                    true
                    );

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = Thread.CurrentPrincipal;
                }
            }
            catch (TokenExpiredException expirado)
            {
                errorResponse = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Expirado em " + expirado.Expiration.ToString());
            }
            catch (SignatureVerificationException ex)
            {
                errorResponse = request.CreateErrorResponse(HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (Exception ex)
            {
                errorResponse = request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return errorResponse != null
                ? Task.FromResult(errorResponse)
                : base.SendAsync(request, cancellationToken);
        }

        private static ClaimsPrincipal ValidateToken(string token, string secret, bool checkExpiration)
        {
            var jsonSerializer = new JavaScriptSerializer();
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

            var payloadJson = decoder.Decode(token, secret, verify: true);
            var payloadData = jsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson);
            var subject = new ClaimsIdentity("Federation", ClaimTypes.Name, ClaimTypes.Role);
            var claims = new List<Claim>();

            if (payloadData != null)
                foreach (var pair in payloadData)
                {
                    var claimType = pair.Key;
                    var source = pair.Value as ArrayList;
                    if (source != null)
                    {
                        claims.AddRange(from object item in source
                                        select new Claim(claimType, item.ToString(), ClaimValueTypes.String));
                        continue;
                    }

                    switch (pair.Key)
                    {
                        case "name":
                            claims.Add(new Claim(ClaimTypes.Name, pair.Value.ToString(), ClaimValueTypes.String));
                            break;
                        case "surname":
                            claims.Add(new Claim(ClaimTypes.Surname, pair.Value.ToString(), ClaimValueTypes.String));
                            break;
                        case "email":
                            claims.Add(new Claim(ClaimTypes.Email, pair.Value.ToString(), ClaimValueTypes.Email));
                            break;
                        case "role":
                            claims.Add(new Claim(ClaimTypes.Role, pair.Value.ToString(), ClaimValueTypes.String));
                            break;
                        case "userId":
                            claims.Add(new Claim(ClaimTypes.UserData, pair.Value.ToString(), ClaimValueTypes.Integer));
                            break;
                        default:
                            claims.Add(new Claim(claimType, pair.Value.ToString(), ClaimValueTypes.String));
                            break;
                    }
                }

            subject.AddClaims(claims);
            return new ClaimsPrincipal(subject);
        }

    }
}