namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_SFAType_Field_In_ClientTree : DbMigration
    {
        public override void Up()
        {
            AddColumn("Jupiter.ClientTree", "SFATypeId", c => c.Guid());
            CreateIndex("Jupiter.ClientTree", "SFATypeId");
            AddForeignKey("Jupiter.ClientTree", "SFATypeId", "Jupiter.SFATypes", "Id");

            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);

            DropColumn("Jupiter.ClientTree", "SFATypeName");
        }
        
        public override void Down()
        {
            AddColumn("Jupiter.ClientTree", "SFATypeName", c => c.String());
            DropForeignKey("Jupiter.ClientTree", "SFATypeId", "Jupiter.SFATypes");
            DropIndex("Jupiter.ClientTree", new[] { "SFATypeId" });
            DropColumn("Jupiter.ClientTree", "SFATypeId");
        }

        private string SqlString = $@"
            UPDATE [DefaultSchemaSetting].[ClientTree] 
            SET
                SFATypeId = SELECT [Id] FROM [DefaultSchemaSetting].[SFATypes] WHERE [Name] = [SFATypeName]
            WHERE 
                [SFATypeName] IS NOT NULL
        ";
    }
}
