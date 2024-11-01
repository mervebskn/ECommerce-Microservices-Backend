using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName{ get; set; }
        public int Quantity { get; set; }

        // Navigation property for Product (optional)
        //public virtual Product Product { get; set; }
    }
}
