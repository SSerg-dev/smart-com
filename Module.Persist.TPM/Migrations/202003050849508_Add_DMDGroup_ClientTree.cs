namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_DMDGroup_ClientTree : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientTree", "DMDGroup", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientTree", "DMDGroup");
        }
    }
}
