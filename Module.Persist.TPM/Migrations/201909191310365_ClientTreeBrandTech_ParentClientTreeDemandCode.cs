namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientTreeBrandTech_ParentClientTreeDemandCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientTreeBrandTech", "ParentClientTreeDemandCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientTreeBrandTech", "ParentClientTreeDemandCode");
        }
    }
}
