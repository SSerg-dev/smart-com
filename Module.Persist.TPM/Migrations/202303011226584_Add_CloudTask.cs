namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CloudTask : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.CloudTask",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                        ProcessDate = c.DateTimeOffset(precision: 7),
                        PipeLine = c.String(maxLength: 255),
                        Status = c.String(maxLength: 255),
                        Model = c.String(maxLength: 255),
                        ModelJson = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropTable($"{defaultSchema}.CloudTask");
        }
    }
}
