namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reAddFerry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FerryRequest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LicensePlate = c.String(maxLength: 256),
                        CarHeight = c.String(),
                        CarLength = c.String(),
                        Cost = c.Double(),
                        CostSecondary = c.Double(),
                        SecondaryCurrency = c.String(),
                        Note = c.String(),
                        DepartureDate = c.DateTime(nullable: false),
                        DepartureAddressID = c.Int(nullable: false),
                        DestinationAddressID = c.Int(nullable: false),
                        TravelRequestID = c.Int(),
                        TravelProposalID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Address", t => t.DepartureAddressID)
                .ForeignKey("dbo.Address", t => t.DestinationAddressID)
                .ForeignKey("dbo.TravelRequest", t => t.TravelRequestID)
                .ForeignKey("dbo.TravelProposal", t => t.TravelProposalID)
                .Index(t => t.DepartureAddressID)
                .Index(t => t.DestinationAddressID)
                .Index(t => t.TravelRequestID)
                .Index(t => t.TravelProposalID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FerryRequest", "TravelProposalID", "dbo.TravelProposal");
            DropForeignKey("dbo.FerryRequest", "TravelRequestID", "dbo.TravelRequest");
            DropForeignKey("dbo.FerryRequest", "DestinationAddressID", "dbo.Address");
            DropForeignKey("dbo.FerryRequest", "DepartureAddressID", "dbo.Address");
            DropIndex("dbo.FerryRequest", new[] { "TravelProposalID" });
            DropIndex("dbo.FerryRequest", new[] { "TravelRequestID" });
            DropIndex("dbo.FerryRequest", new[] { "DestinationAddressID" });
            DropIndex("dbo.FerryRequest", new[] { "DepartureAddressID" });
            DropTable("dbo.FerryRequest");
        }
    }
}
