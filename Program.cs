using System.Data.SqlClient;
using System.Diagnostics;
namespace CrudInjection
{
    public class SportShopDb
    {
        private SqlConnection conn;
        private string connectionString = @"workstation id=hattoriq1.mssql.somee.com;packet size=4096;
                                        user id=hattori_SQLLogin_1;
                                        pwd=9s2l522m2p;
                                        data source=hattoriq1.mssql.somee.com;persist security info=False;
                                        initial catalog=hattoriq1;
                                        TrustServerCertificate=True";
        public SportShopDb()
        {
            conn = new SqlConnection(connectionString);
            conn.Open();
        }
        ~SportShopDb()
        {
            conn.Close();
        }
        public void Create(Product product)
        {
            string cmdText = $@"INSERT INTO Products
                              VALUES ('@Name', 
                                      '@Type', 
                                       @Quantity, 
                                       @Price, 
                                      '@Producer', 
                                       @CostPrice)";

            SqlCommand command = new SqlCommand(cmdText, conn);
            command.Parameters.AddWithValue("Name", product.Name);
            command.Parameters.AddWithValue("Type", product.Type);
            command.Parameters.AddWithValue("Quantity", product.Quantity);
            command.Parameters.AddWithValue("Price", product.Price);
            command.Parameters.AddWithValue("Producer", product.Producer);
            command.Parameters.AddWithValue("CostPrice", product.CostPrice);
            command.CommandTimeout = 5;
            int rows = command.ExecuteNonQuery();
            Console.WriteLine(rows + " rows affected!");
        }
        public void CreateSale(Sales sales)
        {
            string cmdText = $@"INSERT INTO Sales
                              VALUES (@ProductId, 
                                      @Price, 
                                      @Quantity, 
                                      @EmployeeId, 
                                      @ClientId,
                                      @SaleDate)";

            SqlCommand command = new SqlCommand(cmdText, conn);
            command.Parameters.AddWithValue("ProductId", sales.ProductId);
            command.Parameters.AddWithValue("Price", sales.Price);
            command.Parameters.AddWithValue("Quantity", sales.Quantity);
            command.Parameters.AddWithValue("EmployeeId", sales.EmployeeId);
            command.Parameters.AddWithValue("ClientId", sales.ClientId);
            command.Parameters.AddWithValue("SaleDate", sales.SaleDate);
            command.CommandTimeout = 5;
            int rows = command.ExecuteNonQuery();
            Console.WriteLine(rows + " rows affected!");
        }
        public List<Product> GetAll()
        {
            string cmdText = @"select * from Products";
            SqlCommand command = new SqlCommand(cmdText, conn);
            SqlDataReader reader = command.ExecuteReader();
            List<Product> products = new List<Product>();
            while (reader.Read())
            {
                products.Add(new Product()
                {
                    Id = (int)reader[0],
                    Name = (string)reader[1],
                    Type = (string)reader[2],
                    Quantity = (int)reader[3],
                    CostPrice = (int)(decimal)reader[4],
                    Producer = (string)reader[5],
                    Price = (int)(decimal)reader[6]
                });
            }
            reader.Close();
            return products;
        }
        public List<Sales> GetAllSales()
        {
            string cmdText = @"select * from Sales";
            SqlCommand command = new SqlCommand(cmdText, conn);
            SqlDataReader reader = command.ExecuteReader();
            List<Sales> sales = new List<Sales>();
            while (reader.Read())
            {
                sales.Add(new Sales()
                {
                    Id = (int)reader[0],
                    ProductId = (int)reader[1],
                    Price = (int)(decimal)reader[2],
                    Quantity = (int)reader[3],
                    EmployeeId = (int)reader[4],
                    ClientId = (int)reader[5]

                });
            }
            reader.Close();
            return sales;
        }
        public void ShowSellers()
        {
            string cmdText = @"select * from Employees";
            SqlDataReader reader = null;
            SqlCommand command = new SqlCommand(cmdText, conn);
            reader = command.ExecuteReader();
            if (reader.HasRows == true)
            {
                while (reader.Read())
                {
                    Console.WriteLine(String.Format("{0} - {1} - {2} - {3}",
                        reader["FullName"].ToString(),
                        reader["HireDate"].ToString(),
                        reader["Gender"].ToString(),
                        reader["Salary"].ToString()));
                }
            }
            reader.Close();
        }
        public void ShowClients()
        {
            string cmdText = @"select * from Clients";
            SqlDataReader reader = null;
            SqlCommand command = new SqlCommand(cmdText, conn);
            reader = command.ExecuteReader();
            if (reader.HasRows == true)
            {
                while (reader.Read())
                {
                    Console.WriteLine(String.Format("{0} - {1} - {2} - {3} - {4} - {5}",
                        reader["FullName"].ToString(),
                        reader["Email"].ToString(),
                        reader["Phone"].ToString(),
                        reader["Gender"].ToString(),
                        reader["PercentSale"].ToString(),
                        reader["Subscribe"].ToString()));
                }
            }
            reader.Close();
        }
        public List<Sales> GetSalesByPeriod(DateTime startDate, DateTime endDate)
        {
            string cmdText = $@"select * from Sales where SaleDate >= {startDate} and SaleDate <= {endDate} ";
            SqlCommand command = new SqlCommand(cmdText, conn);
            SqlDataReader reader = command.ExecuteReader();
            List<Sales> sales = new List<Sales>();
            while (reader.Read())
            {
                sales.Add(new Sales()
                {
                    Id = (int)reader[0],
                    ProductId = (int)reader[1],
                    Price = (int)reader[2],
                    Quantity = (int)reader[3],
                    EmployeeId = (int)reader[4],
                    ClientId = (int)reader[5],
                });
            }
            reader.Close();
            return sales;
        }
        public Product GetById(int id)
        {
            #region Execute Reader
            string cmdText = $@"select * from Products where Id = {id}";
            SqlCommand command = new SqlCommand(cmdText, conn);
            SqlDataReader reader = command.ExecuteReader();
            Product product = new Product();
            while (reader.Read())
            {

                product.Id = (int)reader[0];
                product.Name = (string)reader[1];
                product.Type = (string)reader[2];
                product.Quantity = (int)reader[3];
                product.CostPrice = (int)reader[4];
                product.Producer = (string)reader[5];
                product.Price = (int)reader[6];

            }
            reader.Close();
            return product;
            #endregion
        }
        public void Update(Product product)
        {
            string cmdText = $@"UPDATE Products
                              SET Name ='@Name', 
                                  TypeProduct ='@Type', 
                                  Quantity =@Quantity, 
                                  CostPrice =@CostPrice, 
                                  Producer ='@Producer', 
                                  Price =@Price
                                  where Id = {product.Id}";

            SqlCommand command = new SqlCommand(cmdText, conn);
            command.Parameters.AddWithValue("Name", product.Name);
            command.Parameters.AddWithValue("Type", product.Type);
            command.Parameters.AddWithValue("Quantity", product.Quantity);
            command.Parameters.AddWithValue("CostPrice", product.CostPrice);
            command.Parameters.AddWithValue("Producer", product.Producer);
            command.Parameters.AddWithValue("Price", product.Price);
            command.CommandTimeout = 5;
            command.ExecuteNonQuery();
        }
        public void Delete(int id)
        {
            string cmdText = $@"delete Products where Id = {id}";
            SqlCommand command = new SqlCommand(cmdText, conn);
            command.ExecuteNonQuery();
        }
        private int FindIdBySellerName(string fullName)
        {
            string employeeCmdText = @"select * from Employees";
            SqlDataReader employeeReader = null;
            SqlCommand employeeCommand = new SqlCommand(employeeCmdText, conn);
            int wantedId = 0;
            employeeReader = employeeCommand.ExecuteReader();
            if (employeeReader.HasRows == true)
            {
                while (employeeReader.Read())
                {
                    if (employeeReader["FullName"].ToString() == fullName)
                    {
                        wantedId = int.Parse(employeeReader["Id"].ToString());
                    }
                }
            }
            employeeReader.Close();
            return wantedId;
        }
        public void ShowSellBySellerName(string fullName)
        {
            string salesCmdText = @"select * from Sales";
            SqlDataReader salesReader = null;
            SqlCommand salesCommand = new SqlCommand(salesCmdText, conn);
            salesReader = salesCommand.ExecuteReader();
            if (salesReader.HasRows == true)
            {
                while (salesReader.Read())
                {
                    if (salesReader["EmployeeId"].ToString() == FindIdBySellerName(fullName).ToString())
                    {
                        Console.WriteLine(String.Format("{0} - {1} - {2} - {3} - {4}",
                        salesReader["ProductId"].ToString(),
                        salesReader["Price"].ToString(),
                        salesReader["Quantity"].ToString(),
                        salesReader["ClientId"].ToString(),
                        salesReader["SaleDate"].ToString()));
                    }
                }
            }
            salesReader.Close();
        }
        public void ShowMoreExpensiveSales(float targetCost)
        {
            string salesCmdText = $@"select * from Sales where Price > {targetCost}  ";
            SqlDataReader salesReader = null;
            SqlCommand salesCommand = new SqlCommand(salesCmdText, conn);
            salesReader = salesCommand.ExecuteReader();
            if (salesReader.HasRows == true)
            {
                while (salesReader.Read())
                {
                    Console.WriteLine(String.Format("{0} - {1} - {2} - {3} - {4}",
                    salesReader["ProductId"].ToString(),
                    salesReader["Price"].ToString(),
                    salesReader["Quantity"].ToString(),
                    salesReader["ClientId"].ToString(),
                    salesReader["SaleDate"].ToString()));
                }
            }
            salesReader.Close();
        }
        public void ShowMaxMinSalesBySellerName(string fullName)
        {
            int maxPrice = 0;
            List<Sales> salesList = GetAllSales();
            List<Sales> salesListBySellerName = new List<Sales>();
            foreach (Sales sales in salesList)
            {
                if (sales.EmployeeId == FindIdBySellerName(fullName))
                {
                    salesListBySellerName.Add(sales);
                }
            }
            foreach (Sales sales in salesListBySellerName)
            {
                if (sales.Price > maxPrice)
                {
                    maxPrice = sales.Price;
                }
            }
            int minPrice = maxPrice;
            foreach (Sales sales in salesListBySellerName)
            {
                if (sales.Price < minPrice)
                {
                    minPrice = sales.Price;
                }
            }
            foreach (Sales sales in salesListBySellerName)
            {
                if (sales.Price == minPrice)
                {
                    Console.WriteLine(sales);
                }
                if (sales.Price == maxPrice)
                {
                    Console.WriteLine(sales);
                }
            }
        }
        public void ShowFirstSaleBySellerName(string fullName)
        {
            List<Sales> salesList = GetAllSales();
            List<Sales> salesListBySellerName = new List<Sales>();
            foreach (Sales sales in salesList)
            {
                if (sales.EmployeeId == FindIdBySellerName(fullName))
                {
                    salesListBySellerName.Add(sales);
                }
            }
            DateTime minDate = salesListBySellerName[0].SaleDate;
            foreach (Sales sales in salesListBySellerName)
            {
                if (sales.SaleDate < minDate)
                {
                    minDate = sales.SaleDate;
                }
            }
            foreach (Sales sales in salesListBySellerName)
            {
                if (sales.SaleDate == minDate)
                {
                    Console.WriteLine(sales);
                }
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {

            SportShopDb db = new SportShopDb(); 
        }
    }
}