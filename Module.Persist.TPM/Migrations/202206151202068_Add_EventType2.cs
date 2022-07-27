namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_EventType2 : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.Event", new[] { "EventType_Id" });
            DropColumn($"{defaultSchema}.Event", "EventTypeId");
            RenameColumn(table: $"{defaultSchema}.Event", name: "EventType_Id", newName: "EventTypeId");
            AlterColumn($"{defaultSchema}.Event", "EventTypeId", c => c.Guid());
            CreateIndex($"{defaultSchema}.Event", "EventTypeId");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.Event", new[] { "EventTypeId" });
            AlterColumn($"{defaultSchema}.Event", "EventTypeId", c => c.Int());
            RenameColumn(table: $"{defaultSchema}.Event", name: "EventTypeId", newName: "EventType_Id");
            AddColumn($"{defaultSchema}.Event", "EventTypeId", c => c.Int());
            CreateIndex($"{defaultSchema}.Event", "EventType_Id");
        }
    }
}
