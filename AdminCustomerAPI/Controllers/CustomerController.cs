using Microsoft.AspNetCore.Mvc;

namespace AdminCustomerAPI.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
