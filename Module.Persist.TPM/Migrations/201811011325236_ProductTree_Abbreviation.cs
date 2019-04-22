namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductTree_Abbreviation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductTree", "Abbreviation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductTree", "Abbreviation");
        }
    }
}
