namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoProduct_ActualProductLSVByCompensation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoProduct", "ActualProductLSVByCompensation", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoProduct", "ActualProductLSVByCompensation");
        }
    }
}
