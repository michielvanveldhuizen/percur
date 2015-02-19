using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Percurrentis.Model
{
    public class AccessLevels
    {
        public bool Employee { get; set; }
        public bool TravelAgency { get; set; }
        public bool COO { get; set; }
        public bool ProjectManager { get; set; }
        public bool Accountant { get; set; }
        public bool supervisor { get; set; }
    }
}
