namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CountryAndSuperiorIDrequired : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TravelRequest", new[] { "CountryID" });
            AlterColumn("dbo.TravelRequest", "SuperiorID", c => c.String(nullable: false));
            AlterColumn("dbo.TravelRequest", "CountryID", c => c.Int(nullable: false));
            CreateIndex("dbo.TravelRequest", "CountryID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TravelRequest", new[] { "CountryID" });
            AlterColumn("dbo.TravelRequest", "CountryID", c => c.Int());
            AlterColumn("dbo.TravelRequest", "SuperiorID", c => c.String());
            CreateIndex("dbo.TravelRequest", "CountryID");
        }
    }
}
