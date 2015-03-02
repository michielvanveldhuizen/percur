namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RebuiltAccommodationRequest : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Accommodation", newName: "AccommodationRequest");
            AddColumn("dbo.AccommodationRequest", "CheckInDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AccommodationRequest", "CheckOutDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AccommodationRequest", "DailyRate", c => c.Double());
            AddColumn("dbo.AccommodationRequest", "AdditionalFees", c => c.Double());
            AddColumn("dbo.AccommodationRequest", "FeeSpecification_Id", c => c.Int());
            CreateIndex("dbo.AccommodationRequest", "FeeSpecification_Id");
            AddForeignKey("dbo.AccommodationRequest", "FeeSpecification_Id", "dbo.Note", "Id");
            DropColumn("dbo.AccommodationRequest", "Cost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccommodationRequest", "Cost", c => c.Double());
            DropForeignKey("dbo.AccommodationRequest", "FeeSpecification_Id", "dbo.Note");
            DropIndex("dbo.AccommodationRequest", new[] { "FeeSpecification_Id" });
            DropColumn("dbo.AccommodationRequest", "FeeSpecification_Id");
            DropColumn("dbo.AccommodationRequest", "AdditionalFees");
            DropColumn("dbo.AccommodationRequest", "DailyRate");
            DropColumn("dbo.AccommodationRequest", "CheckOutDate");
            DropColumn("dbo.AccommodationRequest", "CheckInDate");
            RenameTable(name: "dbo.AccommodationRequest", newName: "Accommodation");
        }
    }
}
