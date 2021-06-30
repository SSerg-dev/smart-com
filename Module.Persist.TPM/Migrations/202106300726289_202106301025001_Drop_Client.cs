namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202106301025001_Drop_Client : DbMigration
    {
        public override void Up()
        {
            Sql("DROP TABLE Jupiter.Client");
        }
        
        public override void Down()
        {
        }
    }
}
