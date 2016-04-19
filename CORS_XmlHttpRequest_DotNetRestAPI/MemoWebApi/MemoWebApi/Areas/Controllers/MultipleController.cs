
using System.Web.Http;
using System.Web.Http.Cors;
using MemoWebApi.Areas.Models;

namespace MemoWebApi.Areas.Controllers
{
    [EnableCors(origins:"*", headers: "*", methods: "*")]
    [RoutePrefix("POC")]
    public class MultipleController : ApiController
    {
        // GET: Multiple
        [Route("Multiple")]
        public double Post([FromBody]MultipleViewModel multipleViewModel)
        {
            return multipleViewModel.Num1*multipleViewModel.Num2;
        }
    }
}