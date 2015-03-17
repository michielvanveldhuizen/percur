using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Percurrentis.Model.Validation.Attributes;

namespace Percurrentis.Model
{
    public class TravelRequest_RequestTraveller
    {
        public int Id { get; set; }
        public int RequestTravellerID { get; set; }
        public int TravelRequestID { get; set; }
        public TravelRequest TravelRequest { get; set; }
        public RequestTraveller RequestTraveller { get; set; }
    }
}