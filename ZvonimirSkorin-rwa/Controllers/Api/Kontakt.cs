using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZvonimirSkorin_rwa.DAL;
using ZvonimirSkorin_rwa.Models;

namespace ZvonimirSkorin_rwa.Controllers.Api
{
    public class Kontakt : Controller
    {

        [HttpPost("/Api/Reserve")]
        public bool Reserve(string firstName, string lastName, string email, string subject, int apartmentId, string address, 
            string phone)
        {
            ApartmentRepo.reserve(apartmentId, subject, firstName + " " + lastName, email, address, phone);
            List <Apartment> list = new List<Apartment>();
            return true;
        }
        [HttpPost("/Api/Register")]
        public bool Post(string? username, string? email, string? phonenumber, string? address, string? password)
        {
            UserRepo.createUser(username, password, email, phonenumber, address);
            return true;
        }
        [HttpGet("/Api/Me")]
        public User me()
        {
            
            string token = Request.Cookies["Token"];
            if (token == null) return null;
            string username = AuthRepo.getUsernameFromToken(token);
            User user = UserRepo.GetUser(username);

                return user;
        }
    }
}
