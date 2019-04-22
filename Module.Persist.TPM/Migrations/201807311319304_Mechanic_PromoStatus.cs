namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Mechanic_PromoStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Mechanic",
                c => new
                {
                    Id = c.Guid(nullable: false, identity: true),
                    Disabled = c.Boolean(nullable: false),
                    DeletedDate = c.DateTimeOffset(precision: 7),
                    MechanicName = c.String(maxLength: 255),
                    Discount = c.Int(),
                    Comment = c.String(maxLength: 255),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.PromoStatus",
                c => new
                {
                    Id = c.Guid(nullable: false, identity: true),
                    Disabled = c.Boolean(nullable: false),
                    DeletedDate = c.DateTimeOffset(precision: 7),
                    Name = c.String(nullable: false, maxLength: 255),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);

        }

        public override void Down()
        {
            DropIndex("dbo.PromoStatus", new[] { "Name" });
            DropTable("dbo.PromoStatus");
            DropTable("dbo.Mechanic");
        }
    }
}
