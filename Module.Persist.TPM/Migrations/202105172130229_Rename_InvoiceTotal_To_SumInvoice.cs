namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rename_InvoiceTotal_To_SumInvoice : DbMigration
    {
        public override void Up()
        {
            RenameColumn("Jupiter.Promo", "InvoiceTotal", "SumInvoice");
            RenameColumn("Jupiter.PromoProduct", "InvoiceTotalProduct", "SumInvoiceProduct");
            RenameColumn("Jupiter.PromoROIReportView", "InvoiceTotal", "SumInvoice");
        }
        
        public override void Down()
        {
            RenameColumn("Jupiter.Promo", "SumInvoice", "InvoiceTotal");
            RenameColumn("Jupiter.PromoProduct", "SumInvoiceProduct", "InvoiceTotalProduct");
            RenameColumn("Jupiter.PromoROIReportView", "SumInvoice", "InvoiceTotal");
        }
    }
}
