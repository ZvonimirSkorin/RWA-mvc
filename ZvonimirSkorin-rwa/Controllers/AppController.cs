using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZvonimirSkorin_rwa.DAL;
using ZvonimirSkorin_rwa.Models;

namespace RWA_javni_dio.Controllers
{
    public class AppController : Controller
{
        private bool isCookieEmpty(string? cookie)
        {
            if(cookie==null)
                return true;
            return cookie.Length <= 0;
        }

    public IActionResult Index()
    {
            string? RoomsCookie = (Request.Cookies["Rooms"]);
            string? AdultsCookie = (Request.Cookies["Adults"]);
            string? ChildrenCookie = (Request.Cookies["Children"]);
            string? SortCookie = (Request.Cookies["Sort"]);
            int? Rooms =!isCookieEmpty(RoomsCookie)? int.Parse(Request.Cookies["Rooms"]):null;
            int? Adults = !isCookieEmpty(AdultsCookie) ? int.Parse(Request.Cookies["Adults"]):null;
            int? Children = !isCookieEmpty(ChildrenCookie) ? int.Parse(Request.Cookies["Children"]):null;
            string Sort = !isCookieEmpty(SortCookie) ? SortCookie:"";
            

          IList<Apartment> apartments =  ApartmentRepo.loadApartments(Rooms, Adults, Children, Sort);
            return View(apartments);
    }

        public IActionResult Apartman()
        {
            string? id = Request.Query["Id"][0];
            if (id == null) return RedirectToAction("/App");
            Apartment apartment = ApartmentRepo.getApartmentById(int.Parse(id));
            return View(apartment);
        }
}
}
