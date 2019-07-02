namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoProduct_ActualProductBaselineLSV : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoProduct", "ActualProductBaselineLSV", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoProduct", "ActualProductBaselineLSV");
        }
    }
}
