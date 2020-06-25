namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PromoDifference_RollingVolume : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RollingVolume", "PromoDifference", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RollingVolume", "PromoDifference");
        }
    }
}
