namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeClientTreeField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientTree", "IsOnInvoice", c => c.Boolean(nullable: true, defaultValue: null));
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClientTree", "ONOFFInvoice", c => c.String());
        }
    }
}
