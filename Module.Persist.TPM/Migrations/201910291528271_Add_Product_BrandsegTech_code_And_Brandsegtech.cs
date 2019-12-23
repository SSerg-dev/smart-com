namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Product_BrandsegTech_code_And_Brandsegtech : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "BrandsegTech_code", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "Brandsegtech", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "Brandsegtech");
            DropColumn("dbo.Product", "BrandsegTech_code");
        }
    }
}
