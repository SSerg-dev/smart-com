namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PT_GetPromoBasicProducts_AP : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            string SqlString = $@"  
				INSERT INTO [{defaultSchema}].[AccessPoint] ([Disabled],[Resource],[Action]) VALUES
				(0, 'ProductTrees',	'GetPromoBasicProducts')

                DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='Administrator');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ProductTrees' and [Action]='GetPromoBasicProducts'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='CMManager');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ProductTrees' and [Action]='GetPromoBasicProducts'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='CustomerMarketing');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ProductTrees' and [Action]='GetPromoBasicProducts'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='DemandFinance');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ProductTrees' and [Action]='GetPromoBasicProducts'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='DemandPlanning');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ProductTrees' and [Action]='GetPromoBasicProducts'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='FunctionalExpert');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ProductTrees' and [Action]='GetPromoBasicProducts'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='GAManager');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ProductTrees' and [Action]='GetPromoBasicProducts'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='KeyAccountManager');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ProductTrees' and [Action]='GetPromoBasicProducts'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='SuperReader');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ProductTrees' and [Action]='GetPromoBasicProducts'))

                SET @RoleId = (SELECT [Id] FROM [{defaultSchema}].[Role] where SystemName='SupportAdministrator');
				INSERT INTO [{defaultSchema}].[AccessPointRole] (RoleId, AccessPointId) values
				(@RoleId, (SELECT [Id] FROM [{defaultSchema}].[AccessPoint] where [Resource]='ProductTrees' and [Action]='GetPromoBasicProducts'))
				";
            Sql(SqlString);
        }
        
        public override void Down()
        {
        }
    }
}
