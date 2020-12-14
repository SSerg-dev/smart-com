namespace Module.Persist.TPM.Migrations {
    using System.Data.Entity.Migrations;

    public partial class PromoView : DbMigration {
        public override void Up() {
            Sql("CREATE VIEW [PromoView] AS SELECT pr.[Id]" +
                    ", pr.[Name]" +
                    ", mmc.[Name] as MarsMechanicName" +
                    ", mmt.[Name] as MarsMechanicTypeName" +
                    ", pr.[MarsMechanicDiscount]" +
                    ", cl.[SystemName] as ColorSystemName" +
                    ", ps.[Color] as PromoStatusColor" +
                    ", ps.[SystemName] as PromoStatusSystemName" +
                    ", pr.[CreatorId]" +
                    ", pr.[ClientTreeId]" +
                    ", pr.[BaseClientTreeIds]" +
                    ", pr.[StartDate]" +
                    ", pr.[EndDate]" +
                    ", pr.[CalendarPriority]" +
                "FROM [Promo] pr " +
                "LEFT JOIN PromoStatus ps ON pr.PromoStatusId = ps.Id " +
                "LEFT JOIN Color cl ON pr.ColorId = cl.Id " +
                "LEFT JOIN Mechanic mmc ON pr.MarsMechanicId = mmc.Id " +
                "LEFT JOIN MechanicType mmt ON pr.MarsMechanicTypeId = mmt.Id " +
                "WHERE pr.[Disabled] = 0");
        }

        public override void Down() {
            Sql("DROP VIEW [PromoView]");
        }
    }
}
