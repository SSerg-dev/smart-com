namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AssortmentMatrix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssortmentMatrix",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        ClientTreeId = c.Int(nullable: false),
                        ZREP_AssortMatrix = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientTree", t => t.ClientTreeId)
                .Index(t => t.ClientTreeId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssortmentMatrix", "ClientTreeId", "dbo.ClientTree");
            DropIndex("dbo.AssortmentMatrix", new[] { "ClientTreeId" });
            DropTable("dbo.AssortmentMatrix");
        }
    }
}
