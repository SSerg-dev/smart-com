namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_ClientTreeNeedUpdateIncident_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientTreeNeedUpdateIncident",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                        ProcessDate = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CreateDate)
                .Index(t => t.ProcessDate);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClientTreeNeedUpdateIncident", new[] { "ProcessDate" });
            DropIndex("dbo.ClientTreeNeedUpdateIncident", new[] { "CreateDate" });
            DropTable("dbo.ClientTreeNeedUpdateIncident");
        }
    }
}
