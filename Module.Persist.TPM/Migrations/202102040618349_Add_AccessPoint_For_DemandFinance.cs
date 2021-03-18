namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AccessPoint_For_DemandFinance : DbMigration
    {
		public override void Up()
		{
			var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
			SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
			Sql(SqlString);
		}

		public override void Down()
		{
		}

		private string SqlString = @"  
				insert into [DefaultSchemaSetting].[AccessPointRole] (RoleId, AccessPointId) values (
				(SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='DemandFinance'), 
				(SELECT [Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='Patch')
				)";
	}
}
