namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncrementalPromo_PromoProduct_Update_Fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoProduct", "Price", c => c.Double());
            DropColumn("dbo.PromoProduct", "ProductBaselinePrice");
            DropColumn("dbo.IncrementalPromo", "CasePrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IncrementalPromo", "CasePrice", c => c.Double());
            AddColumn("dbo.PromoProduct", "ProductBaselinePrice", c => c.Double());
            DropColumn("dbo.PromoProduct", "Price");
        }
    }
}
