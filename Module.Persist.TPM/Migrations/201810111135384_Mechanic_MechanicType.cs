namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mechanic_MechanicType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MechanicType",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Mechanic", "Name", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Mechanic", "SystemName", c => c.String(maxLength: 255));
            //DropColumn("dbo.Mechanic", "MechanicType");
            DropColumn("dbo.Mechanic", "MechanicName");
            DropColumn("dbo.Mechanic", "Discount");
            DropColumn("dbo.Mechanic", "Comment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Mechanic", "Comment", c => c.String(maxLength: 255));
            AddColumn("dbo.Mechanic", "Discount", c => c.Int());
            AddColumn("dbo.Mechanic", "MechanicName", c => c.String(maxLength: 255));
            //AddColumn("dbo.Mechanic", "MechanicType", c => c.String(maxLength: 255));
            DropColumn("dbo.Mechanic", "SystemName");
            DropColumn("dbo.Mechanic", "Name");
            DropTable("dbo.MechanicType");
        }
    }
}
