namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_JobFlag_Table : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'JobFlag' AND XTYPE = 'U')
                    CREATE TABLE [JobFlag](
	                    [Prefix] [nvarchar](50) NOT NULL,
	                    [Value] [tinyint] NOT NULL,
	                    [Description] [nvarchar](50) NULL
                    ) ON [PRIMARY]"
            );
        }
        
        public override void Down()
        {
            Sql(@"IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'JobFlag' AND XTYPE = 'U')
                  DROP TABLE [JobFlag]"
            );
        }
    }
}
