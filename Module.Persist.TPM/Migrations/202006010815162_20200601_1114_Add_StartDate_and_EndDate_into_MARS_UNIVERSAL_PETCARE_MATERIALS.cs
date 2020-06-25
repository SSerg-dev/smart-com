namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200601_1114_Add_StartDate_and_EndDate_into_MARS_UNIVERSAL_PETCARE_MATERIALS : DbMigration
    {
        public override void Up()
        {
            Sql("alter table [dbo].[MARS_UNIVERSAL_PETCARE_MATERIALS] add [StartDate] [datetime] NULL " +
                "alter table [dbo].[MARS_UNIVERSAL_PETCARE_MATERIALS] add [EndDate] [datetime] NULL ");
        }
        
        public override void Down()
        {
            Sql("ALTER TABLE [dbo].[MARS_UNIVERSAL_PETCARE_MATERIALS] DROP COLUMN [StartDate] " +
                "ALTER TABLE [dbo].[MARS_UNIVERSAL_PETCARE_MATERIALS] DROP COLUMN [EndDate] ");
        }
    }
}
