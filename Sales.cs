using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudInjection
{
    public class Sales
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public DateTime SaleDate { get; set; }


        public override string ToString()
        {
            return $"Product :  {ProductId,-16}, Client:  {ClientId,-14}, Price {Price,14},Sale date : {SaleDate}";
        }
    }
}
