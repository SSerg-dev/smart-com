namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RejectReason : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RejectReason",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                        SystemName = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.RejectReason", new[] { "Name" });
            DropTable("dbo.RejectReason");
        }
    }
}
