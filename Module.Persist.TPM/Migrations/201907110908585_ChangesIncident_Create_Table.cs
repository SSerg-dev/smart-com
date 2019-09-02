namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesIncident_Create_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChangesIncident",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        DirectoryName = c.String(),
                        ItemId = c.String(),
                        CreateDate = c.DateTimeOffset(nullable: false, precision: 7),
                        ProcessDate = c.DateTimeOffset(precision: 7),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Disabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ChangesIncident");
        }
    }
}
