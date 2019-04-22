namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BrandTech_Add_DynamicNameField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BrandTech", "Name", c => c.String());
            DropColumn("dbo.BrandTech", "BrandTechName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BrandTech", "BrandTechName", c => c.String());
            DropColumn("dbo.BrandTech", "Name");
        }
    }
}
