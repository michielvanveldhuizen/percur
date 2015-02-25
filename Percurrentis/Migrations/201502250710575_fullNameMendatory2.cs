namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fullNameMendatory2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RequestTraveller", "FullName", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequestTraveller", "FullName", c => c.String(maxLength: 256));
        }
    }
}
