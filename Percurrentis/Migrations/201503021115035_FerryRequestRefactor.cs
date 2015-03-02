namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FerryRequestRefactor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FerryRequest", "Cost", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FerryRequest", "Cost");
        }
    }
}
