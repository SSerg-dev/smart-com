namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_Add_Property_RegularExcludedProductIds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "RegularExcludedProductIds", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "RegularExcludedProductIds");
        }
    }
}
