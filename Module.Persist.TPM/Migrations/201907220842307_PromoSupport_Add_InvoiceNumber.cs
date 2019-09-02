namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoSupport_Add_InvoiceNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoSupport", "InvoiceNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoSupport", "InvoiceNumber");
        }
    }
}
