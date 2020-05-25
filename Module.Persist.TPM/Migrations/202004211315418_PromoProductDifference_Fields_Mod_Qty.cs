namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoProductDifference_Fields_Mod_Qty : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PromoProductDifference", "QTY", c => c.Decimal(precision:30, scale:6));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PromoProductDifference", "QTY", c => c.Double());
        }
    }
}
