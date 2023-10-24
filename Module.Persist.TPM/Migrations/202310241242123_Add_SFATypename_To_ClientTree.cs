namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Add_SFATypename_To_ClientTree : DbMigration
    {
        public override void Up()
        {
            AddColumn("Jupiter.ClientTree", "SFATypeName", c => c.String());
        }

        public override void Down()
        {
            DropColumn("Jupiter.ClientTree", "SFATypeName");
        }
    }
}
