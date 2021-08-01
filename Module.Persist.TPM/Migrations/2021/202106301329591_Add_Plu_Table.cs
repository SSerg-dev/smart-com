namespace Module.Persist.TPM.Migrations
{
	using Core.Settings;
	using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Plu_Table : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.Plu",
                c => new
                {
                    ClientTreeId = c.Int(nullable: false),
                    ProductId = c.Guid(nullable: false),
                    PluCode = c.String(maxLength: 20),
                })
                .PrimaryKey(t => new { t.ClientTreeId, t.ProductId })
                .ForeignKey($"{defaultSchema}.ClientTree", t => t.ClientTreeId)
                .ForeignKey($"{defaultSchema}.Product", t => t.ProductId);
            
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.Plu", "ProductId", "Jupiter.Product");
            DropForeignKey($"{defaultSchema}.Plu", "ClientTreeId", "Jupiter.ClientTree");
            DropTable($"{defaultSchema}.Plu");
        }
    }
}
