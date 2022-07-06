namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_EventType : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.EventType",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                        National = c.Boolean(nullable: false),
                        MarketSegment = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.Disabled, t.DeletedDate, t.Name }, unique: true, name: "EventType_index");
            
            AddColumn($"{defaultSchema}.Event", "EventTypeId", c => c.Int());
            AddColumn($"{defaultSchema}.Event", "EventType_Id", c => c.Guid());
            CreateIndex($"{defaultSchema}.Event", "EventType_Id");
            AddForeignKey($"{defaultSchema}.Event", "EventType_Id", $"{defaultSchema}.EventType", "Id");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.Event", "EventType_Id", $"{defaultSchema}.EventType");
            DropIndex($"{defaultSchema}.EventType", "EventType_index");
            DropIndex($"{defaultSchema}.Event", new[] { "EventType_Id" });
            DropColumn($"{defaultSchema}.Event", "EventType_Id");
            DropColumn($"{defaultSchema}.Event", "EventTypeId");
            DropTable($"{defaultSchema}.EventType");
        }
    }
}
