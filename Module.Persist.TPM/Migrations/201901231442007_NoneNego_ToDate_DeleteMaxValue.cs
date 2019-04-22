namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoneNego_ToDate_DeleteMaxValue : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NoneNego", "ToDate", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NoneNego", "ToDate", c =>
                c.DateTimeOffset(precision: 7, defaultValue: DateTimeOffset.MaxValue));
        }
    }
}
