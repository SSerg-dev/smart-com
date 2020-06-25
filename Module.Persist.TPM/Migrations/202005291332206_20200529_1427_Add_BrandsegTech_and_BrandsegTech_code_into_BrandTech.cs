namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200529_1427_Add_BrandsegTech_and_BrandsegTech_code_into_BrandTech : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BrandTech", "BrandsegTechsub_code", c => c.String());
            AddColumn("dbo.BrandTech", "BrandsegTechsub", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BrandTech", "BrandsegTechsub");
            DropColumn("dbo.BrandTech", "BrandsegTechsub_code");
        }
    }
}
