namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System.Data.Entity.Migrations;
    
    public partial class AddScenarioCopyTask : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            var sqlString = $@"
            CREATE TABLE {defaultSchema}.[ScenarioCopyTask](
	            [Id] [uniqueidentifier] NOT NULL,
	            [Disabled] [bit] NOT NULL,
	            [DeletedDate] [datetimeoffset](7) NULL,
	            [ClientPrefix] [nvarchar](255) NOT NULL,
	            [ClientObjectId] [int] NOT NULL,
	            [Schema] [nvarchar](25) NOT NULL,
	            [CreateDate] [datetimeoffset](7) NULL,
	            [ProcessDate] [datetimeoffset](7) NULL,
	            [ScenarioType] [nvarchar](25) NOT NULL,
	            [ScenarioName] [nvarchar](128) NOT NULL,
	            [Status] [nvarchar](25) NOT NULL,
	            [Email] [nvarchar](255) NOT NULL
            ) ON [PRIMARY]
            GO

            ALTER TABLE {defaultSchema}.[ScenarioCopyTask] ADD  DEFAULT (newsequentialid()) FOR [Id]
            GO
            ";
            Sql(sqlString);
        }
        
        public override void Down()
        {
        }
    }
}
