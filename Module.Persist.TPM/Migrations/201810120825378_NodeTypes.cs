namespace Module.Persist.TPM.Migrations {
    using System.Data.Entity.Migrations;

    public partial class NodeTypes : DbMigration {
        public override void Up() {
            CreateTable(
                "dbo.NodeType",
                c => new {
                    Id = c.Guid(nullable: false, identity: true),
                    Disabled = c.Boolean(nullable: false),
                    DeletedDate = c.DateTimeOffset(precision: 7),
                    Type = c.String(nullable: false, maxLength: 50),
                    Name = c.String(nullable: false, maxLength: 255),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Disabled)
                .Index(t => new { t.DeletedDate, t.Type, t.Name }, unique: true, name: "NTX");

        }

        public override void Down() {
            DropTable("dbo.NodeType");
        }
    }
}
