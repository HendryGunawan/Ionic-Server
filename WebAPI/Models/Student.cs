using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace WebAPI.Models
{
    public class Student
    {
        public int id { get; set; }

        public string nama { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RefreshRequest
    {
        public string token { get; set; }
        public string refreshToken { get; set; }
    }

    public class LoginResponse
    {
        public LoginResponse()
        {

            this.Token = "";
            this.refreshToken = "";
            //this.responseMsg = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.Unauthorized };
        }

        public string Token { get; set; }
        public string refreshToken { get; set; }
        //public HttpResponseMessage responseMsg { get; set; }

    }

    public class RefreshResponse
    {
        public RefreshResponse()
        {

            this.token = "";
            this.refreshToken = "";
            //this.responseMsg = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.Unauthorized };
        }

        public string token { get; set; }
        public string refreshToken { get; set; }
        //public HttpResponseMessage responseMsg { get; set; }

    }

    public class ErrorResponse
    {
        public ErrorResponse()
        {

            this.responseMsg = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.Unauthorized };
        }

        public HttpResponseMessage responseMsg { get; set; }

    }

}