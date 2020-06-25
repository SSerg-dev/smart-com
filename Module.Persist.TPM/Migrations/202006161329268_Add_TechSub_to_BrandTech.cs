namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_TechSub_to_BrandTech : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BrandTech", "TechSubName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BrandTech", "TechSubName");
        }
    }
}
