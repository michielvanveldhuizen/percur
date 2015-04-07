namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makeTRIDnullableOnAcco : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FerryRequest", "DepartureAddressID", "dbo.Address");
            DropForeignKey("dbo.FerryRequest", "DestinationAddressID", "dbo.Address");
            DropForeignKey("dbo.FerryRequest", "TravelRequestID", "dbo.TravelRequest");
            DropForeignKey("dbo.FerryRequest", "TravelProposalID", "dbo.TravelProposal");
            DropIndex("dbo.FerryRequest", new[] { "DepartureAddressID" });
            DropIndex("dbo.FerryRequest", new[] { "DestinationAddressID" });
            DropIndex("dbo.FerryRequest", new[] { "TravelRequestID" });
            DropIndex("dbo.FerryRequest", new[] { "TravelProposalID" });
            DropTable("dbo.FerryRequest");
        }
        
        public override void Down()
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
                        TravelRequestID = c.Int(nullable: false),
                        TravelProposalID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.FerryRequest", "TravelProposalID");
            CreateIndex("dbo.FerryRequest", "TravelRequestID");
            CreateIndex("dbo.FerryRequest", "DestinationAddressID");
            CreateIndex("dbo.FerryRequest", "DepartureAddressID");
            AddForeignKey("dbo.FerryRequest", "TravelProposalID", "dbo.TravelProposal", "Id");
            AddForeignKey("dbo.FerryRequest", "TravelRequestID", "dbo.TravelRequest", "Id");
            AddForeignKey("dbo.FerryRequest", "DestinationAddressID", "dbo.Address", "Id");
            AddForeignKey("dbo.FerryRequest", "DepartureAddressID", "dbo.Address", "Id");
        }
    }
}
