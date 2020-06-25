namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200529_1427_Add_BrandsegTech_and_BrandsegTech_code_into_products : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "BrandsegTechsub", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "BrandsegTechsub_code", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "BrandsegTechsub_code");
            DropColumn("dbo.Product", "BrandsegTechsub");
        }
    }
}
