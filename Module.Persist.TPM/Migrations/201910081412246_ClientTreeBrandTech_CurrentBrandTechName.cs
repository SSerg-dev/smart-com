namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientTreeBrandTech_CurrentBrandTechName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientTreeBrandTech", "CurrentBrandTechName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientTreeBrandTech", "CurrentBrandTechName");
        }
    }
}
