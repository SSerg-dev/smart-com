namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_InvoiceTotal_and_InvoiceTotalProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "InvoiceTotal", c => c.Double());
            AddColumn("dbo.PromoProduct", "InvoiceTotalProduct", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoProduct", "InvoiceTotalProduct");
            DropColumn("dbo.Promo", "InvoiceTotal");
        }
    }
}
