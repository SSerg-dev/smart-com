namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ClientTrees_MassPublish_AP : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            string SqlString = $@"  
				INSERT INTO [{defaultSchema}].[AccessPoint] ([Disabled],[Resource],[Action]) VALUES
				(0, 'ClientTrees',	'MassPublish')

                DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='Administrator');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ClientTrees' and [Action]='MassPublish'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='CustomerMarketing');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ClientTrees' and [Action]='MassPublish'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='KeyAccountManager');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ClientTrees' and [Action]='MassPublish'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='FunctionalExpert');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ClientTrees' and [Action]='MassPublish'))
				";
            Sql(SqlString);
        }
        
        public override void Down()
        {
        }
    }
}
