namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RejectReason_AddField_Custom : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RejectReason", "Custom", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RejectReason", "Custom");
        }
    }
}
