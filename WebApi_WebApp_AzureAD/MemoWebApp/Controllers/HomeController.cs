using MemoApiClient;
using MemoContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MemoWebApp.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var memoClient = new MemoClient();
            var memoList = await memoClient.GetAsync();
            ViewBag.MemoList = memoList;

            return View();
        }
    }
}