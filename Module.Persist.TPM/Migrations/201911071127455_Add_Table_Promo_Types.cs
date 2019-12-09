namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Table_Promo_Types : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromoTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.Name, t.Disabled, t.DeletedDate }, unique: true, name: "Unique_Name");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PromoTypes", "Unique_Name");
            DropTable("dbo.PromoTypes");
        }
    }
}
