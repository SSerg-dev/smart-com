namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_NonPromoEquipment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NonPromoEquipment",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        EquipmentType = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.EquipmentType, t.Disabled, t.DeletedDate }, unique: true, name: "Unique_NonPromoEquipment");
        }
        
        public override void Down()
        {
            DropTable("dbo.NonPromoEquipment");
        }
    }
}
