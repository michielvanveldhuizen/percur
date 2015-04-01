namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccoParentID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accommodation", "ParentID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accommodation", "ParentID");
        }
    }
}
