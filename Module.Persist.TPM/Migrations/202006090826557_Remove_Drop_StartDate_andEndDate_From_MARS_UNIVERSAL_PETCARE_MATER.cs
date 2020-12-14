namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_Drop_StartDate_andEndDate_From_MARS_UNIVERSAL_PETCARE_MATER : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE [MARS_UNIVERSAL_PETCARE_MATERIALS] DROP COLUMN [StartDate] " +
                "ALTER TABLE [MARS_UNIVERSAL_PETCARE_MATERIALS] DROP COLUMN [EndDate] ");
        }

        public override void Down()
        {
            Sql("alter table [MARS_UNIVERSAL_PETCARE_MATERIALS] add [StartDate] [datetime] NULL " +
                "alter table [MARS_UNIVERSAL_PETCARE_MATERIALS] add [EndDate] [datetime] NULL ");
        }
    }
}
