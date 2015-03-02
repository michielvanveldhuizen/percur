namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RepairAccommodation : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AccommodationRequest", newName: "Accommodation");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Accommodation", newName: "AccommodationRequest");
        }
    }
}
