namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixAutoMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestTraveller", "TravelDocument", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestTraveller", "TravelDocument");
        }
    }
}
