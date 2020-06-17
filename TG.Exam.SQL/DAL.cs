using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TG.Exam.SQL
{
    public class DAL
    {
        private SqlConnection GetConnection()
        {
            var connectionString = ConfigurationManager.AppSettings["ConnectionString"];

            var con = new SqlConnection(connectionString);

            con.Open();

            return con;
        }

        private DataSet GetData(string sql)
        {
            var ds = new DataSet();

            using (var con = GetConnection())
            {
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(ds);
                    }
                }
            }

            return ds;
        }

        private void Execute(string sql)
        {
            using (var con = GetConnection())
            {
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetAllOrders()
        {
            var sql = "select * from Orders";

            var ds = GetData(sql);

            var result = ds.Tables.OfType<DataTable>().FirstOrDefault();

            return result;
        }
        public DataTable GetAllOrdersWithCustomers()
        {
            var sql = "select * from Orders join Customers on Orders.OrderCustomerId = Customers.CustomerId";

            var ds = GetData(sql);

            var result = ds.Tables.OfType<DataTable>().FirstOrDefault();

            return result;
        }

        public DataTable GetAllOrdersWithPriceUnder(int price)
        {
            var sql = $@"select distinct Orders.OrderId, Orders.OrderCustomerId, Orders.OrderDate from Orders 
	                     join OrdersItems on Orders.OrderId = OrdersItems.OrderId
                         join Items on OrdersItems.ItemId = Items.ItemId
                         where Items.ItemPrice > {price}";

            var ds = GetData(sql);

            var result = ds.Tables.OfType<DataTable>().FirstOrDefault();

            return result;
        }

        public void DeleteCustomer(int orderId)
        {
            var sql = $@"delete from Customers
                         where CustomerId = (select top (1) OrderCustomerId from Orders where OrderId={orderId})";

            Execute(sql);
        }

        internal DataTable GetAllItemsAndTheirOrdersCountIncludingTheItemsWithoutOrders()
        {
            var sql = $@"select Items.ItemId, Items.ItemName, Items.ItemPrice, Count(Orders.OrderId) as Count from Items
                        join OrdersItems on Items.ItemId = OrdersItems.ItemId
                        join Orders on OrdersItems.OrderId = Orders.OrderId
                        group by Items.ItemId, Items.ItemName, Items.ItemPrice";

            var ds = GetData(sql);

            var result = ds.Tables.OfType<DataTable>().FirstOrDefault();

            return result;
        }
    }
}
