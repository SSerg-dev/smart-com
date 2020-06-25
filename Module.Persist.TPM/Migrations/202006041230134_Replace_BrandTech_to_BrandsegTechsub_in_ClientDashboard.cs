namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Replace_BrandTech_to_BrandsegTechsub_in_ClientDashboard : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.ClientDashboard", "BrandTechName", "BrandsegTechsubName");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.ClientDashboard", "BrandsegTechsubName", "BrandTechName");
        }
    }
}
