
namespace Demo.Service
{
    using Contracts;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data;
    using System.Data.SqlClient;
    

    public class PurchaseOrderService
    {
        private readonly string connectionString = "Server=tcp:zj0h1zrsaj.database.windows.net,1433;Database=OrderDB;User ID=daleiyang@zj0h1zrsaj;Password=1qaz@WSX;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

        public PurchaseOrderService()
        {            
        }

        public List<PurchaseOrder> GetPurchaseOrder(long purchaseOrderId){

            List<PurchaseOrder> purchaseOrderList = new List<PurchaseOrder>();            

            string query = "select * from purchaseorder where Id = @Id";

            SqlCommand cmd = new SqlCommand
            {
                Connection = new SqlConnection(connectionString),
                CommandText = query,
                CommandType = CommandType.Text
            };

            var param = new SqlParameter("Id", 2);
            cmd.Parameters.Add(param);

            DataTable dataTable = new DataTable();
            dataTable.TableName = "purchaseorder";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                da.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    purchaseOrderList = (from DataRow row in dataTable.Rows
                        select new PurchaseOrder
                        {
                            PoId = Convert.ToInt64(row["Id"]),
                            OrderStatus = row["OrderStatus"].ToString()
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd.Connection != null && cmd.Connection.State.ToString() != "Close")
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();

                }
            }
                
            return purchaseOrderList;
            
        }

        public List<PurchaseOrderLineItem> GetPurchaseOrderLineItem(long purchaseOrderId, long itemId)
        {
            List<PurchaseOrderLineItem> lineItemList = new List<PurchaseOrderLineItem>();

            string query = "select * from purchaseorderlineitem where PoId = @PoId and ItemId = @ItemId";

            SqlCommand cmd = new SqlCommand
            {
                Connection = new SqlConnection(connectionString),
                CommandText = query,
                CommandType = CommandType.Text
            };

            var param = new SqlParameter("PoId", 2);
            cmd.Parameters.Add(param);
            param = new SqlParameter("ItemId", 4);
            cmd.Parameters.Add(param);

            DataTable dataTable = new DataTable();
            dataTable.TableName = "purchaseorderlineitem";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                da.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    lineItemList = (from DataRow row in dataTable.Rows
                                    select new PurchaseOrderLineItem
                                    {
                                        PoId = Convert.ToInt64(row["PoId"]),
                                        ItemId = Convert.ToInt64(row["ItemId"]),
                                        ItemName = row["ItemName"].ToString()
                                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd.Connection != null && cmd.Connection.State.ToString() != "Close")
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();

                }   
            }
            
            return lineItemList;
        }


    }
}
