namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientTreeBrandTech_IDeactivatable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientTreeBrandTech", "Disabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.ClientTreeBrandTech", "DeletedDate", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientTreeBrandTech", "DeletedDate");
            DropColumn("dbo.ClientTreeBrandTech", "Disabled");
        }
    }
}
