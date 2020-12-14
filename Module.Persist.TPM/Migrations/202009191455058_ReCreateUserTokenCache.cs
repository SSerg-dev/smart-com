namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReCreateUserTokenCache : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            string tableName = defaultSchema + ".UserTokenCache";
            Sql(@"
                IF OBJECT_ID('dbo.UserTokenCaches', 'U') IS NOT NULL 
                    DROP TABLE dbo.UserTokenCaches;
            ");
            Sql(@"
                IF OBJECT_ID('Jupiter.UserTokenCaches', 'U') IS NOT NULL 
                    DROP TABLE Jupiter.UserTokenCaches;
            ");

            CreateTable(
                tableName,
                c => new
                    {
                        UserTokenCacheId = c.Int(nullable: false, identity: true),
                        webUserUniqueId = c.String(),
                        cacheBits = c.Binary(),
                        LastWrite = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserTokenCacheId);
            
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            string tableName = defaultSchema + ".UserTokenCache";
            DropTable(tableName);
            
            CreateTable(
                "dbo.UserTokenCaches",
                c => new
                {
                    UserTokenCacheId = c.Int(nullable: false, identity: true),
                    webUserUniqueId = c.String(),
                    cacheBits = c.Binary(),
                    LastWrite = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.UserTokenCacheId);
        }
    }
}
