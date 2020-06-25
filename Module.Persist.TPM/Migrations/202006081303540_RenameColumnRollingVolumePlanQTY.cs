namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameColumnRollingVolumePlanQTY : DbMigration
    {
        //Невидимые пробелы
        public override void Up()
        {
            RenameColumn("dbo.RollingVolume", "PlanProductInсrementalQTY", "PlanProductIncrementalQTY"); 
        }

        public override void Down()
        {
            RenameColumn("dbo.RollingVolume", "PlanProductIncrementalQTY", "PlanProductInсrementalQTY");
        }
     
    }
}
