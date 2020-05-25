namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePromoField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "IsOnInvoice", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            AddColumn("dbo.Promo", "ONOFFInvoice", c => c.String());
        }
    }
}
