﻿// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Controller classes for the connection between the server and the client.</summary>

using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;
using Breeze.WebApi2;
using Newtonsoft.Json.Linq;
using Percurrentis.AD_classes;
using Percurrentis.Context;
using Percurrentis.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace Percurrentis.Controllers
{
    [BreezeController]
    public class TravelRequestController : ApiController
    {
        private 
        static readonly DatabaseContext db = new DatabaseContext();
        readonly EFContextProvider<DatabaseContext> _contextProvider = new EFContextProvider<DatabaseContext>();

        //retrieve metadata from database
        [HttpGet]
        public string Metadata()
        {
            try
            {
                return _contextProvider.Metadata();
            }
            catch (DataException e)
            {
                Trace.WriteLine("Unable to retrieve metadata:" + "\r" + " -Errormessage: " + e.Message + "\r" + " -InnerException: " + e.InnerException);
                throw new HttpResponseException(HttpStatusCode.NoContent);
            }
        }

        //save results in the database and call SaveChanges method
        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
           //Trace.WriteLine("TEST123");
            try
            {
                return _contextProvider.SaveChanges(saveBundle);
            }
            catch (DbUpdateException e)
            {
                Trace.WriteLine("Unable to save changes:" + "\r" + " -Errormessage: " + e.Message + "\r" + " -InnerException: " + e.InnerException);
                throw new HttpResponseException(HttpStatusCode.ServiceUnavailable);
            }
        }

        //retrieve Insurances
        [HttpGet]
        public IQueryable<Insurance> Insurances()
        {
            return _contextProvider.Context.Insurance;
        }


        //retrieve ArchivedTravelRequests
        [HttpGet]
        public IQueryable<ArchivedTravelRequest> ArchivedTravelRequests()
        {
            return _contextProvider.Context.ArchivedTravelRequest;
        }

        //retrieve TravelRequests with all its relevant related entities
        [HttpGet]
        public IQueryable<TravelRequest> TravelRequests()
        {
            return _contextProvider.Context.TravelRequest
                .Include("TravelRequest_RequestTravellers")
                .Include("TravelRequest_RequestTravellers.RequestTraveller")
                .Include("CustomerOrProspect")
                .Include("CustomerOrProspect.Address")
                .Include("FlightRequests")
                .Include("FlightRequests.FlyerMemberCard")
                .Include("FlightRequests.DepartureAddress")
                .Include("FlightRequests.DestinationAddress")
                .Include("FerryRequests")
                .Include("FerryRequests.DepartureAddress")
                .Include("FerryRequests.DestinationAddress")
                .Include("EuroTunnelRequests")
                .Include("EuroTunnelRequests.DepartureAddress")
                .Include("EuroTunnelRequests.DestinationAddress")
                .Include("RentalCarRequests")
                .Include("RentalCarRequests.Driver")
                .Include("RentalCarRequests.SecondaryDriver")
                .Include("RentalCarRequests.ServiceCompany")
                .Include("RentalCarRequests")
                .Include("TaxiRequests")
                .Include("TaxiRequests.DepartureAddress")
                .Include("TaxiRequests.DestinationAddress")
                .Include("Accommodations")
                .Include("TravelRequestApproval");
        }

        //retrieve travelRequestApprovals including the TravelRequest
        [HttpGet]
        public IQueryable<TravelRequestApproval> TravelRequestApprovals()
        {
            return _contextProvider.Context.TravelRequestApproval
                .Include("TravelRequest");
        }

        
        [HttpGet]
        public IQueryable<TravelRequest_RequestTraveller> TravelRequest_RequestTravellers()
        {
            return _contextProvider.Context.TravelRequest_RequestTraveller.AsNoTracking();
        }

        //retrieve AddressTypes
        [HttpGet]
        public IQueryable<AddressType> AddressTypes()
        {
            return _contextProvider.Context.AddressType.AsNoTracking();
        }

        //retrieve Addresses
        [HttpGet]
        public IQueryable<Address> Address()
        {
            return _contextProvider.Context.Address.AsNoTracking();
        }

        //retrieve Companies including the address
        [HttpGet]
        public ExchangeRate ExchangeRate()
        {
            return _contextProvider.Context.ExchangeRate.AsNoTracking().First();
        }

        //
        [HttpGet]
        public IQueryable<Company> Company()
        {
            return _contextProvider.Context.Company
                .Include("Address")
                .Include("Company.Address");
        }

        //retrieve FerryRequests
        [HttpGet]
        public IQueryable<FerryRequest> FerryRequest()
        {
            return _contextProvider.Context.FerryRequest;
        }

        //retrieve TaxiRequests
        [HttpGet]
        public IQueryable<TaxiRequest> TaxiRequest()
        {
            return _contextProvider.Context.TaxiRequest;
        }

        //retrieve AccommodationRequests
        [HttpGet]
        public IQueryable<Accommodation> AccommodationRequest()
        {
            return _contextProvider.Context.Accommodation;
        }

        //retrieve RequestTraveller
        [HttpGet]
        public IQueryable<RequestTraveller> RequestTraveller()
        {
            return _contextProvider.Context.RequestTraveller
                .Include("Company")
                .Include("Company.Address")
                .Include("TravelRequest_RequestTravellers")
                .Include("TravelRequest_RequestTravellers.TravelRequest");
        }

        //retrieve RentalCarRequest
        [HttpGet]
        public IQueryable<RentalCarRequest> RentalCarRequest()
        {
            return _contextProvider.Context.RentalCarRequest;
        }

        //retrieve AirportInformation
        [HttpGet]
        public IQueryable<AirportInformation> AirportInformation()
        {
            return _contextProvider.Context.AirportInformation.AsNoTracking();
        }

        //retrieve ServiceCompanies
        [HttpGet]
        public IQueryable<ServiceCompany> ServiceCompanies()
        {
            return _contextProvider.Context.ServiceCompany;
        }

        //retrieve CountryInformation
        [HttpGet]
        public IQueryable<CountryInformation> CountryInformation()
        {
            return _contextProvider.Context.CountryInformation.AsNoTracking();
        }

        //retrieve ActiveDirectory Users
        [HttpGet]
        public List<UserAC> ADusers()
        {            
            //Using Adservice as Singleton
            ADservices ADservices = ADservices.InstanceCreation();
            List<UserAC> List = ADservices.GetUsers();
            return List;
        }  
    }
}
