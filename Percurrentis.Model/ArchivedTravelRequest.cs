using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Percurrentis.Model
{
    public class ArchivedTravelRequest
    {
        public int Id { get; set; }
        public int TravelRequestID { get; set; }
        [MaxLength]
        public string Content { get; set; }
    }
}
