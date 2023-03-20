using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class SelectedItemsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
