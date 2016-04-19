using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.UI.Repositories.DomainModels;
using Web.UI.Repositories.Models;
using Web.Common.Extensions;
using Web.UI.ServiceGateway.OrderServiceProxy;
namespace Web.UI.Repositories.Interfaces
{
    public interface IOrderBridge
    {
        Task<SearchResult<KeyValuePair<OrderHeader, IEnumerable<OrderLineItem>>>> GetOrdersWithStatus(   string pcnFilter,
                                                                                                        string customerNumber,
                                                                                                        long? id,
                                                                                                        string userName,
                                                                                                        int pageNumber,
                                                                                                        int pageSize,
                                                                                                        string[] purchaseOrderStatusCodeTable);
    }
}
