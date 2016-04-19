using MemoContracts;
using MemoRestfulAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MemoRestfulAPI.Controllers
{
    [Authorize]
    public class MemoController : ApiController
    {
        // GET api/values
        public IEnumerable<MemoItem> Get()
        {
            AadHelper.CheckClaims();

            MemoItem item1 = new MemoItem("test1");
            MemoItem item2 = new MemoItem("test2");
            var itemList = new MemoItem[2] {item1, item2}; 
            
            return itemList;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }
    }
}
