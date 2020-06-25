namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_Drop_StartDate_andEndDate_From_MARS_UNIVERSAL_PETCARE_MATER : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE [dbo].[MARS_UNIVERSAL_PETCARE_MATERIALS] DROP COLUMN [StartDate] " +
                "ALTER TABLE [dbo].[MARS_UNIVERSAL_PETCARE_MATERIALS] DROP COLUMN [EndDate] ");
        }

        public override void Down()
        {
            Sql("alter table [dbo].[MARS_UNIVERSAL_PETCARE_MATERIALS] add [StartDate] [datetime] NULL " +
                "alter table [dbo].[MARS_UNIVERSAL_PETCARE_MATERIALS] add [EndDate] [datetime] NULL ");
        }
    }
}
