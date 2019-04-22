namespace Module.Persist.TPM.Migrations {
    using System;
    using System.Data.Entity.Migrations;

    public partial class NodeTypesPriority : DbMigration {
        public override void Up() {
            AddColumn("dbo.NodeType", "Priority", c => c.Int(nullable: false, defaultValue: 0));
        }

        public override void Down() {
            DropColumn("dbo.NodeType", "Priority");
        }
    }
}
