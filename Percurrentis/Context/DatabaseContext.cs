// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Context class for the creation of the database and Save-actions on the database.</summary>

using Percurrentis.AD_classes;
using Percurrentis.Mapping;
using Percurrentis.Model;
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
        public DbSet<ApprovalRole> ApprovalRole { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Insurance> Insurance { get; set; }
        public DbSet<CountryInformation> CountryInformation { get; set; }
        public DbSet<ServiceCompany> Servicecompany { get; set; }
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
            modelBuilder.Configurations.Add(new ApprovalRoleMap());
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
            //other declarations such as keys, properties and required attributes are 
            //described in the pocos related mapping class (eg TravelRequest is mapped 
            //in TravelRequestMap.
            #endregion
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            var changedEntities = ChangeTracker.Entries();
            //space for server-side edits on entites which are about to be saved into the database
            foreach (var changedEntity in changedEntities.Where(e => e.State == EntityState.Added))
            {
                if (changedEntity.Entity is Company)
                {
                    var specEntity = changedEntity.Entity as Company;
                    if (specEntity != null && !GlobalVar.defaultCompanies.Contains(specEntity.Name))
                    {
                        specEntity.DefaultCompany = false;
                    }
                }
                if (changedEntity.Entity is MetaEntity)
                {
                    var specEntity = changedEntity.Entity as MetaEntity;
                    if (specEntity.IsDeleted != true)
                    {
                        specEntity.OnBeforeInsert();
                    }
                }
                if (changedEntity.Entity is TravelRequest)
                {
                    var specEntity = changedEntity.Entity as TravelRequest;
                    
                    TravelRequestApproval TRA = new TravelRequestApproval();
                    TRA.ApprovalRoleID = 1;
                    TRA.Hash = "dingen";
                    TRA.Id = -1;
                    TRA.Archived = false;
                    TRA.Flag = false;
                    TRA.HasApproved = 0;

                    //PETER DO NOTIFICATION TO PROJECT MANAGER

                    TRA.NotificationSent = false;
                    
                    this.TravelRequestApproval.Add(TRA);

                    specEntity.TravelRequestApproval = TRA;
                    
                    
                }
            }
            foreach (var changedEntity in changedEntities.Where(e => e.State == EntityState.Modified))
            {
                if (changedEntity.Entity is MetaEntity)
                {
                    var specEntity = changedEntity.Entity as MetaEntity;
                    specEntity.OnBeforeUpdate();
                }
                if (changedEntity.Entity is TravelRequest)
                {
                    var specEntity = changedEntity.Entity as TravelRequest;
                    if (specEntity.IsDeleted == true)
                    {
                        specEntity.DeletedDate = DateTime.Now;
                        specEntity.DeletedBy = 1;
                    }
                }
                if (changedEntity.Entity is TravelRequestApproval)
                {
                    var specEntity = changedEntity.Entity as TravelRequestApproval;

                    TravelRequest travelRequest = this.TravelRequest.Single(TravelRequest => TravelRequest.TravelRequestApprovalID == specEntity.Id);
                    travelRequest.IsApproved = specEntity.HasApproved;

                    if (specEntity.HasApproved == 2) { 
                        //PETER GA JIJ DIT EENS AAN DE TRAVELAGENTS NOTIFIBANANEN
                        //PETER GA JIJ DIT EENS AAN DE EMPLOYBAANAAN NOTIFIBANANEN
                    }
                    
                    ADservices AD = ADservices.InstanceCreation();
                    UserAC self = AD.GetSelf();
                    
                    specEntity.ApprovedBy = self.objectGuid;
                    specEntity.ApprovalDate = System.DateTime.Now;

                    Trace.WriteLine(specEntity.Flag+" - specEntity flag");
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
                        Trace.WriteLine("Entity of type " + eve.Entry.Entity.GetType().Name + " in state "+eve.Entry.State+" has the following validation errors:");
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