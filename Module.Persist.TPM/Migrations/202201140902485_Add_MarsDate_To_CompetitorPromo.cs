namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_MarsDate_To_CompetitorPromo : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.CompetitorPromo", "MarsStartDate", c => c.String(nullable:true, maxLength: 15));
            AddColumn($"{defaultSchema}.CompetitorPromo", "MarsEndDate", c => c.String(nullable: true, maxLength: 15));
            AddColumn($"{defaultSchema}.CompetitorPromo", "MarsDispatchesStart", c => c.String(nullable: true, maxLength: 15));
            AddColumn($"{defaultSchema}.CompetitorPromo", "MarsDispatchesEnd", c => c.String(nullable: true, maxLength: 15));
            AddColumn($"{defaultSchema}.CompetitorPromo", "DispatchesStart", c => c.DateTimeOffset(nullable: true, precision: 7));
            AddColumn($"{defaultSchema}.CompetitorPromo", "DispatchesEnd", c => c.DateTimeOffset(nullable: true, precision: 7));
            AddColumn($"{defaultSchema}.CompetitorPromo", "PromoDuration", c => c.Int(nullable: true));
            AddColumn($"{defaultSchema}.CompetitorPromo", "DispatchDuration", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.PromoProduct", "MarsStartDate");
            DropColumn($"{defaultSchema}.PromoProduct", "MarsEndDate");
            DropColumn($"{defaultSchema}.PromoProduct", "MarsDispatchesStart");
            DropColumn($"{defaultSchema}.PromoProduct", "MarsDispatchesEnd");
            DropColumn($"{defaultSchema}.PromoProduct", "DispatchesStart");
            DropColumn($"{defaultSchema}.PromoProduct", "DispatchesEnd");
            DropColumn($"{defaultSchema}.PromoProduct", "PromoDuration");
            DropColumn($"{defaultSchema}.PromoProduct", "DispatchDuration");
        }
    }
}
