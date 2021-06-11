namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ManualInputSumInvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("Jupiter.Promo", "ManualInputSumInvoice", c => c.Boolean(defaultValue: false));
            Sql(@"
                UPDATE [Jupiter].[Promo]
                   SET [ManualInputSumInvoice] = 0
                 WHERE [ManualInputSumInvoice] is NULL
                GO
            ");
        }
        
        public override void Down()
        {
            DropColumn("Jupiter.Promo", "ManualInputSumInvoice");
        }
    }
}
