using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Data.Entities
{
    public class AddProductResponse
    {
        public string Message { get; set; }
        public string ProductId { get; set; }
        public bool IsSuccess { get; set; }
    }
}
