namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoProduct_ProductEN : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoProduct", "ProductEN", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoProduct", "ProductEN");
        }
    }
}
