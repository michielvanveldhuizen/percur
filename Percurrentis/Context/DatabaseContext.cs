// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Context class for the creation of the database and Save-actions on the database.</summary>

using Percurrentis.AD_classes;
using Percurrentis.Mapping;
using Percurrentis.Model;
using Percurrentis.NotificationCenter;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace Percurrentis.Context
{
    public class DatabaseContext : DbContext
    {
        #region DbSet Declaration
        public DbSet<Accommodation> Accommodation { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<AirportInformation> AirportInformation { get; set; }
        public DbSet<AddressType> AddressType { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Insurance> Insurance { get; set; }
        public DbSet<CountryInformation> CountryInformation { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<ServiceCompany> ServiceCompany { get; set; }
        public DbSet<ServiceCompanyType> ServiceCompanyType { get; set; }
        public DbSet<ContactPerson> ContactPerson { get; set; }
        public DbSet<FlightRequest> FlightRequest { get; set; }
        public DbSet<FlyerMemberCard> FlyerMemberCard { get; set; }
        public DbSet<NoteCollection> NoteCollection { get; set; }
        public DbSet<Note> Note { get; set; }
        public DbSet<PhoneNumber> PhoneNumber { get; set; }
        public DbSet<RentalCarRequest> RentalCarRequest { get; set; }
        public DbSet<RequestTraveller> RequestTraveller { get; set; }
        public DbSet<FerryRequest> FerryRequest { get; set; }
        public DbSet<TaxiRequest> TaxiRequest { get; set; }
        public DbSet<EuroTunnelRequest> EuroTunnelRequest { get; set; }
        public DbSet<TravelRequestApproval> TravelRequestApproval { get; set; }
        public DbSet<TravelRequest> TravelRequest { get; set; }
        public DbSet<ExchangeRate> ExchangeRate { get; set; }
        public DbSet<TravelRequest_RequestTraveller> TravelRequest_RequestTraveller { get; set; }
        public DbSet<ArchivedTravelRequest> ArchivedTravelRequest { get; set; }
        public DbSet<TravelProposal> TravelProposal { get; set; }
        #endregion
        public DatabaseContext()
            : base("PercurrentisContext")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region modelBuilder conventions and configurations
            //conventions
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //configs
            modelBuilder.Configurations.Add(new AccommodationMap());
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new AddressTypeMap());
            modelBuilder.Configurations.Add(new AirportInformationMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new CountryInformationMap());
            modelBuilder.Configurations.Add(new ContactPersonMap());
            modelBuilder.Configurations.Add(new EuroTunnelRequestMap());
            modelBuilder.Configurations.Add(new FerryRequestMap());
            modelBuilder.Configurations.Add(new FlightRequestMap());
            modelBuilder.Configurations.Add(new FlyerMemberCardMap());
            modelBuilder.Configurations.Add(new NoteCollectionMap());
            modelBuilder.Configurations.Add(new NoteMap());
            modelBuilder.Configurations.Add(new PhoneNumberMap());
            modelBuilder.Configurations.Add(new RentalCarRequestMap());
            modelBuilder.Configurations.Add(new RequestTravellerMap());
            modelBuilder.Configurations.Add(new ServiceCompanyMap());
            modelBuilder.Configurations.Add(new ServiceCompanyTypeMap());
            modelBuilder.Configurations.Add(new TaxiRequestMap());
            modelBuilder.Configurations.Add(new TravelRequestApprovalMap());
            modelBuilder.Configurations.Add(new TravelRequestMap());
            modelBuilder.Configurations.Add(new TravelProposalMap());
            //other declarations such as keys, properties and required attributes are 
            //described in the pocos related mapping class (eg TravelRequest is mapped 
            //in TravelRequestMap.
            #endregion
        }

        public override int SaveChanges()
        {
            //ADservice as Singelton
            ADservices AD = ADservices.InstanceCreation();

            ChangeTracker.DetectChanges();
            var changedEntities = ChangeTracker.Entries();
            Console.WriteLine(changedEntities);
            //space for server-side edits on entites which are about to be saved into the database

            //Oncreate
            foreach (var changedEntity in changedEntities.Where(e => e.State == EntityState.Added))
            {
                //Company
                Console.WriteLine(changedEntity);
                if (changedEntity.Entity is Company)
                {
                    var specEntity = changedEntity.Entity as Company;
                    if (specEntity != null && !GlobalVar.defaultCompanies.Contains(specEntity.Name))
                    {
                        specEntity.DefaultCompany = false;
                    }
                }
                #region nullHacks
                if (changedEntity.Entity is FlightRequest)
                {
                    var specEntity = changedEntity.Entity as FlightRequest;
                    if ((specEntity.TravelProposalID).Equals(0))
                    {
                        specEntity.TravelProposalID = null;
                    }
                    else
                    {
                        if (specEntity.TravelRequestID.Equals(0))
                        {
                            specEntity.TravelRequestID = null;
                        }
                    }
                }

                if (changedEntity.Entity is RentalCarRequest)
                {
                    var specEntity = changedEntity.Entity as RentalCarRequest;
                    if ((specEntity.TravelProposalID).Equals(0))
                    {
                        specEntity.TravelProposalID = null;
                    }
                    if (specEntity.TravelRequestID.Equals(0))
                    {
                        specEntity.TravelRequestID = null;
                    }
                }
                if (changedEntity.Entity is TaxiRequest)
                {
                    var specEntity = changedEntity.Entity as TaxiRequest;
                    if ((specEntity.TravelProposalID).Equals(0))
                    {
                        specEntity.TravelProposalID = null;
                    }
                    else
                    {
                        if (specEntity.TravelRequestID.Equals(0))
                        {
                            specEntity.TravelRequestID = null;
                        }
                    }
                }
                if (changedEntity.Entity is EuroTunnelRequest)
                {
                    var specEntity = changedEntity.Entity as EuroTunnelRequest;
                    if ((specEntity.TravelProposalID).Equals(0))
                    {
                        specEntity.TravelProposalID = null;
                    }
                    else
                    {
                        if (specEntity.TravelRequestID.Equals(0))
                        {
                            specEntity.TravelRequestID = null;
                        }
                    }
                }
                if (changedEntity.Entity is Accommodation)
                {
                    var specEntity = changedEntity.Entity as Accommodation;
                    if ((specEntity.TravelProposalID).Equals(0))
                    {
                        specEntity.TravelProposalID = null;
                    }
                    else
                    {
                        if(specEntity.TravelRequestID.Equals(0))
                        {
                            specEntity.TravelRequestID = null;
                        }
                    }
                }
                if (changedEntity.Entity is FerryRequest)
                {
                    var specEntity = changedEntity.Entity as FerryRequest;
                    if ((specEntity.TravelProposalID).Equals(0))
                    {
                        specEntity.TravelProposalID = null;
                    }
                    else
                    {
                        if (specEntity.TravelRequestID.Equals(0))
                        {
                            specEntity.TravelRequestID = null;
                        }
                    }
                }
                #endregion
                if (changedEntity.Entity is MetaEntity)
                {
                    var specEntity = changedEntity.Entity as MetaEntity;
                    if (specEntity.IsDeleted != true)
                    {
                        specEntity.OnBeforeInsert();
                    }
                }
                //TravelRequest
                if (changedEntity.Entity is TravelProposal)
                {
                    Console.WriteLine("Proposal!");
                }
                //TravelRequest
                if (changedEntity.Entity is TravelRequest)
                {
                    var specEntity = changedEntity.Entity as TravelRequest;

                    //Create TravelRequestApproval to connect it to the TravelRequest
                    TravelRequestApproval TRA = new TravelRequestApproval();
                    TRA.Id = -1;
                    TRA.Archived = false;
                    TRA.Flag = false;
                    TRA.HasApproved = 0;

                    // Notify superior
                    specEntity.Hash = String.Format("{0:X}", DateTime.Now.GetHashCode());
                    Notification.requestManagerApproval(specEntity);
                    
                    

                    //get the country based on the CountryID
                    CountryInformation country = this.CountryInformation.Single(Country => Country.Id == specEntity.CountryID);
                    specEntity.Country = country;

                    string objectGuidTempSave = specEntity.SuperiorID;

                    if (country.CountryCode.Equals("RO "))
                    {
                        // Notify COO of travel request to Romania
                        UserAC kees = AD.GetUserByName("Kees Oosting");
                        specEntity.SuperiorID = kees.objectGuid;
                        Notification.requestManagerApproval(specEntity);
                    }

                    //reset the superior
                    specEntity.SuperiorID = objectGuidTempSave;
                        
                    
                    specEntity.TravelRequestApproval = TRA;
                }
            }
            //OnChange
            foreach (var changedEntity in changedEntities.Where(e => e.State == EntityState.Modified))
            {
                if (changedEntity.Entity is MetaEntity)
                {
                    var specEntity = changedEntity.Entity as MetaEntity;
                    specEntity.OnBeforeUpdate();
                }
                if (changedEntity.Entity is TaxiRequest)
                {
                    var specEntity = changedEntity.Entity as TaxiRequest;
                    if ((specEntity.TravelProposalID).Equals(0))
                    {
                        specEntity.TravelProposalID = null;
                    }
                    else
                    {
                        if (specEntity.TravelRequestID.Equals(0))
                        {
                            specEntity.TravelRequestID = null;
                        }
                    }
                }
                if (changedEntity.Entity is RentalCarRequest)
                {
                    var specEntity = changedEntity.Entity as RentalCarRequest;
                    if ((specEntity.TravelProposalID).Equals(0))
                    {
                        specEntity.TravelProposalID = null;
                    }
                    else
                    {
                        if (specEntity.TravelRequestID.Equals(0))
                        {
                            specEntity.TravelRequestID = null;
                        }
                    }
                }
                if (changedEntity.Entity is Accommodation)
                {
                    var specEntity = changedEntity.Entity as Accommodation;
                    if ((specEntity.TravelProposalID).Equals(0))
                    {
                        specEntity.TravelProposalID = null;
                    }
                    else
                    {
                        if (specEntity.TravelRequestID.Equals(0))
                        {
                            specEntity.TravelRequestID = null;
                        }
                    }
                }
                if (changedEntity.Entity is FerryRequest)
                {
                    var specEntity = changedEntity.Entity as FerryRequest;
                    if ((specEntity.TravelProposalID).Equals(0))
                    {
                        specEntity.TravelProposalID = null;
                    }
                    else
                    {
                        if (specEntity.TravelRequestID.Equals(0))
                        {
                            specEntity.TravelRequestID = null;
                        }
                    }
                }
                //TravelRequest
                if (changedEntity.Entity is TravelRequest)
                {
                    var specEntity = changedEntity.Entity as TravelRequest;
                    if (specEntity.IsDeleted == true)
                    {
                        specEntity.DeletedDate = DateTime.Now;
                        specEntity.DeletedBy = 1;
                    }
                }
                //TravelRequestApproval
                if (changedEntity.Entity is TravelRequestApproval)
                {
                    var specEntity = changedEntity.Entity as TravelRequestApproval;

                    UserAC self = AD.GetSelf();


                    bool isSaveRequest = false;

                    if (specEntity.ApprovedBy.Equals(self.objectGuid))
                    {
                        isSaveRequest = true;
                    }
                    
                    if(GlobalVar.developMode){
                        isSaveRequest = true;
                    }

                    if(isSaveRequest){
                        TravelRequest travelRequest = this.TravelRequest.Single(TravelRequest => TravelRequest.TravelRequestApprovalID == specEntity.Id);
                        CountryInformation country = this.CountryInformation.Single(Country => Country.Id == travelRequest.CountryID);
                        UserAC requester = AD.GetUserByName(travelRequest.ApplicantID);
                        
                        //If user is Kees and country is Romania
                        if (GlobalVar.COOGuid.Equals(self.objectGuid) && country.Name.Equals("Romania"))
                        {
                            //If Kees is also the supervisor
                            if (self.objectGuid.Equals(travelRequest.SuperiorID))
                            {
                                specEntity.ApprovedBy = self.objectGuid;
                                specEntity.ApprovalDate = System.DateTime.Now;
                            }
                        }
                        else
                        {
                            specEntity.ApprovedBy = self.objectGuid;
                            specEntity.ApprovalDate = System.DateTime.Now;
                        }

                        bool isFullyApproved = false;
                        bool isRejected = false;

                        //If Country is Romania and Supervisor is not kees
                        //Need 2 approves in this kees
                        if (country.Name.Equals("Romania") && !travelRequest.SuperiorID.Equals(GlobalVar.COOGuid))
                        {
                            //Check if fully approved now
                            if (specEntity.HasApproved == 2 && specEntity.COOApproved == 2)
                            {
                                travelRequest.IsApproved = 2;
                                isFullyApproved = true;
                            }

                            //Check if one has rejected
                            if (specEntity.HasApproved == 1 || specEntity.COOApproved == 1)
                            {
                                isRejected = true;
                                travelRequest.IsApproved = 1;
                            }
                        }
                        else
                        {
                            //(Country != Romania && Supervisor = Kees) Or (Country != Romania) 
                            //Only need 1 approve in that kees
                            travelRequest.IsApproved = specEntity.HasApproved;
                            if (specEntity.HasApproved == 2)
                            {
                                isFullyApproved = true;
                            }
                            if (specEntity.HasApproved == 1)
                            {
                                isRejected = true;
                            }
                        }

                        if (isFullyApproved)
                        {
                            UserAC travelAgency = AD.GetUserByName("Travel Agency");
                            Notification.notifyOfApproval(travelAgency, travelRequest);
                            // Notify the person who requested the travel that it was approved
                        }
                        if (isRejected)
                        {
                            Notification.notifyOfDenial(requester, travelRequest);
                        }
                    }
                    else
                    {
                        specEntity.COOApproved = 0;
                    }
                }
            }
            try
            {
                return base.SaveChanges();
            }
            catch (Exception e)
            {
                if (e is DbEntityValidationException)
                {
                    var pe = e as DbEntityValidationException;
                    foreach (var eve in pe.EntityValidationErrors)
                    {
                        Trace.WriteLine("Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:");
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Trace.WriteLine("- Property: " + ve.PropertyName + ", Error: " + ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                else
                {
                    Trace.WriteLine("An error other than DbEntityValidationException occured on the database: Message: " + e.Message + ", InnerException: " + e.InnerException);
                    throw;
                }
            }
        }
    }
}