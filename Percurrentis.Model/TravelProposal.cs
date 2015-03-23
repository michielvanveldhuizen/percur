﻿using Percurrentis.Model.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Percurrentis.Model
{
    public class TravelProposal : MetaEntity
    {
        public int Id { get; set; }
        [TrueOrFalse]
        public bool IsApproved { get; set; }
        public int TravelRequestID { get; set; }

        public virtual ICollection<Accommodation> Accommodations { get; set; }
        public virtual ICollection<FlightRequest> FlightRequests { get; set; }
        /*public virtual ICollection<RentalCarRequest> RentalCarRequests { get; set; }
        public virtual ICollection<FerryRequest> FerryRequests { get; set; }
        public virtual ICollection<EuroTunnelRequest> EuroTunnelRequests { get; set; }
        public virtual ICollection<TaxiRequest> TaxiRequests { get; set; }
        */
    }
}