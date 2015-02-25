// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Model classes for the database</summary>

using Percurrentis.Model.Validation.Attributes;
using System.Collections.Generic;

namespace Percurrentis.Model
{
    public class TravelRequest : MetaEntity
    {
        public int Id { get; set; }
        [_256]
        public string QuoteOrOrderNumber { get; set; }
        [_1024]
        public string Purpose { get; set; }
        public int IsApproved { get; set; }
        [TrueOrFalse]
        public bool IsArchived { get; set; }
        public int CustomerOrProspectID { get; set; }
        public string SuperiorID { get; set; }
        public string ApplicantID { get; set; }
        public string NoteCollectionID { get; set; }
        public int? TravelRequestApprovalID { get; set; }
        public int? CountryID { get; set; }
        public string Hash { get; set; }

        public virtual CountryInformation Country { get; set; }
        public virtual TravelRequestApproval TravelRequestApproval { get; set; }
        public virtual Company CustomerOrProspect { get; set; }
        //[ValidateObject]
        //public ContactPerson Superior { get; set; }
        //[ValidateObject]
        //public ContactPerson Applicant { get; set; }
        //[ValidateObject]
        //public NoteCollection NoteCollection { get; set; }

        public virtual ICollection<Accommodation> Accommodations { get; set; }
        public virtual ICollection<RentalCarRequest> RentalCarRequests { get; set; }
        public virtual ICollection<FerryRequest> FerryRequests { get; set; }
        public virtual ICollection<EuroTunnelRequest> EuroTunnelRequests { get; set; }
        public virtual ICollection<TaxiRequest> TaxiRequests { get; set; }
        public virtual ICollection<FlightRequest> FlightRequests { get; set; }
        public virtual ICollection<RequestTraveller> RequestTravellers { get; set; }
    }
}
