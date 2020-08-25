namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_FilterQuery_ProductTree : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductTree", "FilterQuery", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductTree", "FilterQuery");
        }
    }
}
