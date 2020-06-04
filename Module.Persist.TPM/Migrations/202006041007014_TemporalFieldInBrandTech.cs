namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TemporalFieldInBrandTech : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BrandTech", "TempSub_code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BrandTech", "TempSub_code");
        }
    }
}
