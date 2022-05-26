using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace ApiMinimalPersonajesSeries.Authentication
{
    public class BasicAuthenticationHandler:
        AuthenticationHandler<AuthenticationSchemeOptions>
    {
        //CONTENDRA UN CONTRUSCTOR QUE RECIBIRA 
        //UNOS PARAMETROS (INTERNOS) DENTRO DE LA CLASE BASE
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
            ) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            //RECIBE UN PARAMETRO QUE ES AUTHORIZATION CON UN CONTENIDO
            //DE basic Y USERNAME:PASSWORD ENCRIPTADO COMO TOKEN
            if (authHeader != null && 
                authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                //RECUPERAMOS EL TOKEN CIFRADO
                var token = authHeader.Substring("Basic ".Length).Trim();
                //DECODIFICAMOS EL TOKEN PARA QUEDARNOS CON USERNAME:PASSWORD
                var credentialsString = 
                    Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var credential = credentialsString.Split(':');
                if (credential[0] == "admin" && credential[1] == "admin")
                {
                    //CREAMOS LOS CLAIMS PARA LA VALIDACION DE USUARIOS
                    var claims = new[]
                    {
                        new Claim("name", credential[0]),
                        new Claim(ClaimTypes.NameIdentifier, credential[0]),
                        new Claim(ClaimTypes.Role, "Basic")
                    };
                    var identity = new ClaimsIdentity(claims, "Basic");
                    var claimsPrincipal = new ClaimsPrincipal(identity);
                    return Task.FromResult(AuthenticateResult.Success
                        (new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
                }
                //NO TIENE LAS CREDENCIALES CORRECTAS
                Response.StatusCode = 401;
                Response.Headers.Add("WWW-Authenticate", "Basic realm=\"dotnetthoughts.net\"");
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }
            else
            {
                Response.StatusCode = 401;
                Response.Headers.Add("WWW-Authenticate", "Basic realm=\"dotnetthoughts.net\"");
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }
        }
    }
}
