namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExecutionCode_ChangeName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientTree", "GHierarchyCode", c => c.String(maxLength: 255));
            DropColumn("dbo.ClientTree", "ExecutionCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClientTree", "ExecutionCode", c => c.String(maxLength: 255));
            DropColumn("dbo.ClientTree", "GHierarchyCode");
        }
    }
}
