namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoneNego_RemovedClientHierarchyAndProductHierarchyFields : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.NoneNego", "ClientHierarchy");
            DropColumn("dbo.NoneNego", "ProductHierarchy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NoneNego", "ProductHierarchy", c => c.String());
            AddColumn("dbo.NoneNego", "ClientHierarchy", c => c.String());
        }
    }
}
