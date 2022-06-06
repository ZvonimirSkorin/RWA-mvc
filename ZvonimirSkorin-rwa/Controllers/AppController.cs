using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RWA_javni_dio.Controllers
{
    public class AppController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
}
