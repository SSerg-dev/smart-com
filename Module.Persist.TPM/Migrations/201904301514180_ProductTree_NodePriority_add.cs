namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductTree_NodePriority_add : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductTree", "NodePriority", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductTree", "NodePriority");
        }
    }
}
