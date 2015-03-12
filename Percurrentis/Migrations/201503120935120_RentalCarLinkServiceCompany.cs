namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RentalCarLinkServiceCompany : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RentalCarRequest", "ServiceCompanyID", c => c.Int());
            CreateIndex("dbo.RentalCarRequest", "ServiceCompanyID");
            AddForeignKey("dbo.RentalCarRequest", "ServiceCompanyID", "dbo.ServiceCompany", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RentalCarRequest", "ServiceCompanyID", "dbo.ServiceCompany");
            DropIndex("dbo.RentalCarRequest", new[] { "ServiceCompanyID" });
            DropColumn("dbo.RentalCarRequest", "ServiceCompanyID");
        }
    }
}
