using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class UpdateOrderDto
    {
        public int Quantity { get; set; }
        public bool IsCanceled { get; set; }
    }
}
