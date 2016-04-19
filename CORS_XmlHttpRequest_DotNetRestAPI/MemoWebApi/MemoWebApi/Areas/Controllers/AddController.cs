
using System.Web.Http;
using System.Web.Http.Cors;

namespace MemoWebApi.Areas.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("POC")]
    public class AddController : ApiController
    {
        [Route("Add/{num1:int}/{num2:int}")]
        public int Get(int num1, int num2)
        {
            return num1+num2;
        }
    }
}
