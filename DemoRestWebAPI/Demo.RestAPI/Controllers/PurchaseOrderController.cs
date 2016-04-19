
namespace Demo.RestAPI.Controllers
{
    using Attributes;

    using Service;

    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Cors;

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/purchaseorder")]
    [RequireHttps]
    [RequireAuthorization]
    public class PurchaseOrderController : ApiController
    {
        #region Fields

        private PurchaseOrderService purchaseOrderService = new PurchaseOrderService();

        #endregion

        [HttpGet]
        [Route("{purchaseOrderId:long}")]
        public HttpResponseMessage GetPurchaseOrder(long purchaseOrderId)
        {
            var ret = this.purchaseOrderService.GetPurchaseOrder(purchaseOrderId);            
            return this.Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        [HttpGet]
        [Route("{purchaseOrderId:long}/lineitem/{lineItemId:long}")]
        public HttpResponseMessage GetPurchaseOrderLineItem(long purchaseOrderId, long lineItemId)
        {
            var ret = this.purchaseOrderService.GetPurchaseOrderLineItem(purchaseOrderId, lineItemId);            
            return this.Request.CreateResponse(HttpStatusCode.OK, ret);
        }
    }
}
