namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteTempSub_Code : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BrandTech", "TempSub_code");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BrandTech", "TempSub_code", c => c.String());
        }
    }
}
