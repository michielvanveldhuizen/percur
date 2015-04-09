// <copyright company=CSi Romania SRL>
// Copyright (c) 2014 All Rights Reserved
// </copyright>
// <author>Tim Lagerburg</author>
// <summary>Migration configuration for the database + some seed data (do be put in SQL-seedfile)</summary>

namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<Percurrentis.Context.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Percurrentis.Context.DatabaseContext context)
        {         
            //todo: create one SQL-file for data building
            if (!context.CountryInformation.Any(c => c.Name == "Netherlands"))
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin", string.Empty) + "\\Migrations";
                context.Database.ExecuteSqlCommand(File.ReadAllText(baseDir + "\\CountryInformation.sql"));
            }
            if (!context.Currency.Any(c => c.Name == "Euro"))
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin", string.Empty) + "\\Migrations";
                context.Database.ExecuteSqlCommand(File.ReadAllText(baseDir + "\\Currency.sql"));
            }
            if (!context.AirportInformation.Any(a => a.Name == "Eindhoven"))
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin", string.Empty) + "\\Migrations";
                context.Database.ExecuteSqlCommand(File.ReadAllText(baseDir + "\\AirportInformation.sql"));
            }
            if (!context.Address.Any(a => a.AddressName == "CSi Industries"))
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin", string.Empty) + "\\Migrations";
                context.Database.ExecuteSqlCommand(File.ReadAllText(baseDir + "\\dbo_Address.sql"));
            }
            if (!context.AddressType.Any(a => a.Name == "Company"))
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin", string.Empty) + "\\Migrations";
                context.Database.ExecuteSqlCommand(File.ReadAllText(baseDir + "\\dbo_AddressType.sql"));
            }
            if (!context.Company.Any(c => c.Name == "CSi Industries"))
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin", string.Empty) + "\\Migrations";
                context.Database.ExecuteSqlCommand(File.ReadAllText(baseDir + "\\dbo_Company.sql"));
            }
            if (!context.ServiceCompanyType.Any(c => c.Name == "Accommodation"))
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin", string.Empty) + "\\Migrations";
                context.Database.ExecuteSqlCommand(File.ReadAllText(baseDir + "\\dbo_ServiceCompanyType.sql"));
            }
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                StringBuilder sb = new StringBuilder();
            //  This method will be called after migrating to the latest version.
 
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        sb.AppendFormat("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw new Exception(sb.ToString());
            }
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
         }
     }
}