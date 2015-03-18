namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArchiveTravelRequests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArchivedTravelRequest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TravelRequestID = c.Int(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ArchivedTravelRequest");
        }
    }
}
