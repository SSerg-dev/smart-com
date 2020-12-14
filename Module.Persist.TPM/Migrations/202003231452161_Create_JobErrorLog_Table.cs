namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_JobErrorLog_Table : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'JobErrorLog' AND XTYPE = 'U')
                    CREATE TABLE [JobErrorLog](
	                    [Id] [uniqueidentifier] NOT NULL,
	                    [LogDate] [datetime] NOT NULL,
	                    [StepName] [nvarchar](400) NULL,
	                    [ErrorMessage] [nvarchar](max) NULL,
	                    [AdditionalInfo] [nvarchar](max) NULL,
	                    [ProcessDate] [datetime] NULL,
                     CONSTRAINT [PK_JobErrorLog] PRIMARY KEY CLUSTERED 
                    (
	                    [Id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                    GO

                    ALTER TABLE [JobErrorLog] ADD  DEFAULT (newsequentialid()) FOR [Id]
                    GO

                    ALTER TABLE [JobErrorLog] ADD  DEFAULT (sysdatetime()) FOR [LogDate]
                    GO"
            );
        }
        
        public override void Down()
        {
            Sql(@"IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'JobErrorLog' AND XTYPE = 'U')
                  DROP TABLE [JobErrorLog]
                  GO"
            );
        }
    }
}
