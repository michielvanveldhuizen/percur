using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Percurrentis.Model
{
    public class ExchangeRate
    {
        public int Id { get; set; }
        public double EUR { get; set; }
        public double RON { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
