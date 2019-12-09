namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ClientTreeNeedUpdateIncident_Fields : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ClientTreeNeedUpdateIncident", new[] { "CreateDate" });
            DropIndex("dbo.ClientTreeNeedUpdateIncident", new[] { "ProcessDate" });
            AddColumn("dbo.ClientTreeNeedUpdateIncident", "Disabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.ClientTreeNeedUpdateIncident", "DeletedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.ClientTreeNeedUpdateIncident", "PropertyName", c => c.String());
            AddColumn("dbo.ClientTreeNeedUpdateIncident", "PropertyValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientTreeNeedUpdateIncident", "PropertyValue");
            DropColumn("dbo.ClientTreeNeedUpdateIncident", "PropertyName");
            DropColumn("dbo.ClientTreeNeedUpdateIncident", "DeletedDate");
            DropColumn("dbo.ClientTreeNeedUpdateIncident", "Disabled");
            CreateIndex("dbo.ClientTreeNeedUpdateIncident", "ProcessDate");
            CreateIndex("dbo.ClientTreeNeedUpdateIncident", "CreateDate");
        }
    }
}
