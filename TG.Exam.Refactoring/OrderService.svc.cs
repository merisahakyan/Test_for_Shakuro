using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using log4net;
using log4net.Config;

namespace TG.Exam.Refactoring
{
    public class OrderService : IOrderService
    {
        //Better to use dependency injection
        private static readonly ILog logger = LogManager.GetLogger(typeof(OrderService));

        readonly string connectionString = ConfigurationManager.ConnectionStrings["OrdersDBConnectionString"].ConnectionString;

        public IDictionary<string, Order> cache = new Dictionary<string, Order>();

        public OrderService()
        {
            BasicConfigurator.Configure();
        }

        public Order LoadOrder(string orderId)
        {
            try
            {
                // this is wrong approach to check string . Better to use !string.IsNullOrEmpty(orderId)
                Debug.Assert(null != orderId && orderId != "");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                lock (cache)
                {
                    if (cache.ContainsKey(orderId))
                    {
                        stopWatch.Stop();
                        logger.InfoFormat("Elapsed - {0}", stopWatch.Elapsed);
                        return cache[orderId];
                    }
                }

                //here instead of using string format we can use better approach to create query string, using $ feature
                //it is more readable and we will not use new objects to save query in formatted way, and will save memory
                //string queryTemplate = $"SELECT OrderId, OrderCustomerId, OrderDate FROM dbo.Orders where OrderId='{orderId}'";

                string queryTemplate =
                  "SELECT OrderId, OrderCustomerId, OrderDate" +
                  "  FROM dbo.Orders where OrderId='{0}'";
                string query = string.Format(queryTemplate, orderId);

                SqlConnection connection =
                  new SqlConnection(this.connectionString);
                SqlCommand command =
                  new SqlCommand(query, connection);

                //Instead of opening and closing connection is better to use using blocks
                //in this example developer forgot to close connetion and it is not good.
                //Using block will close connection in this situations. And we should not to worry about opening and closing connection

                //using (var conn = new SqlConnection(this.connectionString))
                //{
                //    using (var comm = new SqlCommand(query, connection))
                //    {

                //    }
                //}

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Order order = new Order
                    {
                        //This casting is not prefered to use.Better to use int.Parse and DateTime.Parse methods.
                        //More prefered to use int.TryParse and DateTime.TryParse methods and only in successful case add order to cache
                        OrderId = (int)reader[0],
                        OrderCustomerId = (int)reader[1],
                        OrderDate = (DateTime)reader[2]
                    };

                    lock (cache)
                    {
                        if (!cache.ContainsKey(orderId))
                            cache[orderId] = order;
                    }
                    stopWatch.Stop();
                    logger.InfoFormat("Elapsed - {0}", stopWatch.Elapsed);
                    return order;
                }
                stopWatch.Stop();
                logger.InfoFormat("Elapsed - {0}", stopWatch.Elapsed);
                return null;
            }
            catch (SqlException ex)
            {
                logger.Error(ex.Message);
                throw new ApplicationException("Error");
            }
        }
    }
}
