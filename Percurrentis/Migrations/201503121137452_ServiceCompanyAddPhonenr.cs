namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceCompanyAddPhonenr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceCompany", "PhoneNumber", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceCompany", "PhoneNumber");
        }
    }
}
