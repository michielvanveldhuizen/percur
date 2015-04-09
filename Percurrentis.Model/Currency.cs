using Percurrentis.Model.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Percurrentis.Model
{
    public class Currency
    {
        [Forbidden]
        public int Id { get; set; }
        [Forbidden]
        public string Name { get; set; }
        [Forbidden]
        public string CurrencyCode { get; set; }
    }
}
