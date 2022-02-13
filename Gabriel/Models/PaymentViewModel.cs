using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gabriel.Models
{
    public class PaymentViewModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Email { get; set; }
        public int Price { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
