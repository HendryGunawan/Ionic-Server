using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using WebAPI.Models;


namespace WebAPI.Controllers
{
    public class LoginController : ApiController
    {
        const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";

        [HttpPost]
        public IHttpActionResult Authenticate([FromBody] LoginRequest login)
        {
            IHttpActionResult response;
            LoginResponse loginResponse = new LoginResponse { };
            ErrorResponse errorResponse = new ErrorResponse { };
            LoginRequest loginrequest = new LoginRequest { };

            loginrequest.Email = login.Email.ToLower();
            loginrequest.Password = login.Password;
          
            bool isUsernamePasswordValid = false;

            if (login != null)
                isUsernamePasswordValid = loginrequest.Password == "admin" ? true : false;
            // if credentials are valid
            if (isUsernamePasswordValid)
            {
                LoginResponse LogRes = new LoginResponse();
                LogRes.Token = createToken(loginrequest.Email);
                LogRes.refreshToken = GenerateRefreshToken();
                //return the token
                return Ok(LogRes);
            }
            else
            {
                // if credentials are not valid send unauthorized status code in response
                errorResponse.responseMsg.StatusCode = HttpStatusCode.Unauthorized;
                response = ResponseMessage(errorResponse.responseMsg);
                return response;
            }
        }

        private string createToken(string username)
        {
            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddMinutes(2);

            //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            });

            
            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);


            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(
                        issuer: "http://localhost:54454", 
                        audience: "http://localhost:54454",
                        subject: claimsIdentity, 
                        notBefore: issuedAt, 
                        expires: expires, 
                        signingCredentials: signingCredentials
                    );
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                //return Convert.ToBase64String(randomNumber);
                return "12345";
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        [HttpPost]
        public IHttpActionResult Refresh([FromBody] RefreshRequest RefreshRequest)
        {
            var RefreshResponse = new RefreshResponse { };
            ErrorResponse ErrorResponse = new ErrorResponse();
            IHttpActionResult response;

            var principal = GetPrincipalFromExpiredToken(RefreshRequest.token);
            var username = principal.Identity.Name;
            //var savedRefreshToken = GetRefreshToken(username); //retrieve the refresh token from a data store
            var savedRefreshToken = "12345"; //retrieve the refresh token from a data store
            if (savedRefreshToken != RefreshRequest.refreshToken)
            {
                ErrorResponse.responseMsg.StatusCode = HttpStatusCode.Unauthorized;
                response = ResponseMessage(ErrorResponse.responseMsg);
                return response;
            }


            var newJwtToken = createToken(username);
            var newRefreshToken = GenerateRefreshToken();
            //DeleteRefreshToken(username, refreshToken);
            //SaveRefreshToken(username, newRefreshToken);

            return Ok(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }
    }
}