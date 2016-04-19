using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.UI.Repositories.DomainModels;

namespace Web.UI.Repositories.Interfaces
{
      public interface IProductBridge
    {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="productRequest"></param>
      /// <returns></returns>
          Task<SearchResult<OrderLineItem>> SearchProducts(ProductRequest productRequest);

          /// <summary>
          /// Get Purchase Orders in Detail 
          /// </summary>
          /// <param name="agreementId"></param>
          /// <param name="endCustomerNumber"></param>
          /// <returns></returns>
          Task<SearchResult<OrderLineItem>> GetOpportunitiesByOrderHistory(int agreementId, string endCustomerNumber);
    }
}
