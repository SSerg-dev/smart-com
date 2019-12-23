namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_BrandTech_BrandTech_code : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BrandTech", "BrandTech_code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BrandTech", "BrandTech_code");
        }
    }
}
