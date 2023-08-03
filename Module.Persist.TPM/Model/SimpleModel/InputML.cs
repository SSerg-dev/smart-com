using System;
using System.ComponentModel;

namespace Module.Persist.TPM.Model.SimpleModel
{
    public abstract class InputML
    {
        [DisplayName("Promo ID")]
        public int PromoId { get; set; }
        [DisplayName("PPG")]
        public string PPG { get; set; }
        [DisplayName("Format")]
        public string Format { get; set; }
        [DisplayName("ZREP")]
        public int ZREP { get; set; }
        [DisplayName("StartDate")]
        public DateTimeOffset StartDate { get; set; }
        [DisplayName("EndDate")]
        public DateTimeOffset EndDate { get; set; }
        [DisplayName("Mechanic(Mars)")]
        public string MechanicMars { get; set; }
        [DisplayName("Discount(Mars)")]
        public double DiscountMars { get; set; }
        [DisplayName("Mech(Instore)")]
        public string MechInstore { get; set; }
        [DisplayName("Instore Discount")]
        public double InstoreDiscount { get; set; }
        [DisplayName("Planned Uplift")]
        public double PlannedUplift { get; set; }
        [DisplayName("PlanInStore Shelf Price")]
        public double PlanInStoreShelfPrice { get; set; }
        [DisplayName("FormatCode")]
        public int FormatCode { get; set; }
        [DisplayName("Source")]
        public string Source { get; set; }

    }
    public class InputMLRS : InputML
    {
        [DisplayName("BaseLSV")]
        public double BaseLSV { get; set; }
        [DisplayName("TotalLSV")]
        public double TotalLSV { get; set; }
    }
    public class InputMLRA : InputML
    {
        [DisplayName("Year")]
        public int Year { get; set; }
    }
}
