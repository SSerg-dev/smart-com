namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDMDGroup_RollingVolume : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RollingVolume", "DMDGroup", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RollingVolume", "DMDGroup");
        }
    }
}
