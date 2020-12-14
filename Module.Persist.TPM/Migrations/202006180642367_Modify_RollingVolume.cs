namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modify_RollingVolume : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RollingVolume", "ActualOO", c => c.Double());
            AddColumn("dbo.RollingVolume", "PreliminaryRollingVolumes", c => c.Double());
            AddColumn("dbo.RollingVolume", "RollingVolumesCorrection", c => c.Double());
            AddColumn("dbo.RollingVolume", "RollingVolumesTotal", c => c.Double());
            AddColumn("dbo.RollingVolume", "ManualRollingTotalVolumes", c => c.Double());
            DropColumn("dbo.RollingVolume", "RollingVolumes");
            Sql("ALTER TABLE [RollingVolume] DROP COLUMN [RollingVolumesCorrection];" +

                "ALTER TABLE[RollingVolume]  ADD RollingVolumesCorrection AS(ManualRollingTotalVolumes - RollingVolumesTotal)");
        }
        
        public override void Down()
        {
            Sql("ALTER TABLE [RollingVolume] DROP COLUMN [RollingVolumesCorrection];" +

             "ALTER TABLE[RollingVolume]  ADD [RollingVolumesCorrection ] [float] NULL");

            AddColumn("dbo.RollingVolume", "RollingVolumes", c => c.Double());
            DropColumn("dbo.RollingVolume", "ManualRollingTotalVolumes");
            DropColumn("dbo.RollingVolume", "RollingVolumesTotal");
            DropColumn("dbo.RollingVolume", "RollingVolumesCorrection");
            DropColumn("dbo.RollingVolume", "PreliminaryRollingVolumes");
            DropColumn("dbo.RollingVolume", "ActualOO");
          
        }
    }
}
