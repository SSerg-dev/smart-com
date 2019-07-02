namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_NumberField_PromoSupport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoSupport", "Number", c => c.Int(nullable: false, defaultValue: 0));

            // Триггер для обновления Number
            Sql("CREATE TRIGGER PromoSupport_Increment_Number ON PromoSupport AFTER INSERT AS "
            + "BEGIN " 
                + "UPDATE PromoSupport SET Number = (SELECT ISNULL((SELECT MAX(Number) FROM PromoSupport), 0) + 1) FROM Inserted WHERE PromoSupport.Id = Inserted.Id; "
            + "END");

            // Пронумеровываем старые записи
            Sql("UPDATE PromoSupport SET Number = PS2.R_Number "
                + "FROM PromoSupport AS PS1, (SELECT Row_Number() OVER(ORDER BY PS3.StartDate) AS R_Number, Id FROM PromoSupport AS PS3) AS PS2 "
                + "WHERE PS1.Id = PS2.Id;");
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoSupport", "Number");
            Sql("DROP TRIGGER PromoSupport_Increment_Number");
        }
    }
}
