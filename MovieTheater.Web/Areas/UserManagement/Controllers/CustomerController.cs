using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieTheater.Web.Areas.UserManagement.Controllers
{
    [Area("UserManagement")]
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        public IActionResult Homepage()
        {
            return View();
        }
    }
}