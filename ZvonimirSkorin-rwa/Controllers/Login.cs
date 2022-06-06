using Microsoft.AspNetCore.Mvc;
using ZvonimirSkorin_rwa.DAL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZvonimirSkorin_rwa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Login : ControllerBase
    {
        // GET: api/<Login>
        [HttpGet("/Login")]
        public bool Get()
        {
            string userName = Request.Query["username"][0];
            string passWord = Request.Query["password"][0];
            string token = AuthRepo.Authenticate(userName, passWord);
            if (token != null)
            {

                Response.Cookies.Append("token", token, new CookieOptions() { Expires = DateTime.Now.AddMinutes(10) });
                return true;
            }
            return false;
        }

        
    }
}
