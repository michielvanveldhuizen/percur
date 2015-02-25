namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class returnOfTheHash2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TravelRequest", "Hash", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TravelRequest", "Hash", c => c.String(nullable: false));
        }
    }
}
