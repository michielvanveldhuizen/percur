namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FerryGodParentsId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FerryRequest", "ParentID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FerryRequest", "ParentID");
        }
    }
}
