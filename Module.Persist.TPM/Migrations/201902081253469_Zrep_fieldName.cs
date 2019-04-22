namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Zrep_fieldName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "ZREP", c => c.String(maxLength: 255));
            AddColumn("dbo.Actual", "ZREP", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.BaseLine", "ZREP", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.Product", "GRD");
            DropColumn("dbo.Actual", "GRD");
            DropColumn("dbo.BaseLine", "GRD");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BaseLine", "GRD", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Actual", "GRD", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Product", "GRD", c => c.String(maxLength: 255));
            DropColumn("dbo.BaseLine", "ZREP");
            DropColumn("dbo.Actual", "ZREP");
            DropColumn("dbo.Product", "ZREP");
        }
    }
}
