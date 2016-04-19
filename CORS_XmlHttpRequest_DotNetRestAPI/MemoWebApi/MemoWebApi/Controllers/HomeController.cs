using System.Web.Mvc;

namespace MemoWebApi.Controller
{
    public class HomeController : System.Web.Mvc.Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}