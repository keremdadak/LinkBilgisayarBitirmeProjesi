using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterDirectory.Data.Models
{
    public class CommercialActivity
    {
        public int Id { get; set; }
        public string Service { get; set; }
        public decimal Price { get; set; }
        public int CustomerId { get; set; }
        public DateTime ServiceDate { get; set; }
    }
}
