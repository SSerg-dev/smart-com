namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IsApolloExport_Promo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "IsApolloExport", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "IsApolloExport");
        }
    }
}
