namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductsTree_Add_LogoFileName_Field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductTree", "LogoFileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductTree", "LogoFileName");
        }
    }
}
