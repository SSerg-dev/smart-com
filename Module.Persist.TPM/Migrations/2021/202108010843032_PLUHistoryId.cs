namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PLUHistoryId : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Jupiter.Plu", "Id", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Jupiter.Plu", "Id", c => c.Guid(nullable: false, identity: true));
        }
    }
}
