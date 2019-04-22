namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product_AddEAN : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "EAN", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "EAN");
        }
    }
}
