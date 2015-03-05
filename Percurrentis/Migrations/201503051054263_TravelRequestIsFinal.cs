namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TravelRequestIsFinal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TravelRequest", "IsFinal", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TravelRequest", "IsFinal");
        }
    }
}
