using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;

namespace Module.Persist.TPM.Utils {
    /// <summary>
    /// Класс для сортировки промо с учётом дат и приоритета для выгрузки в EXCEL
    /// </summary>
    public class PromoSortComparer : IComparer<Promo> {
        /// <summary>
        /// Сортировка промо с учётом дат и приоритета для выгрузки в EXCEL
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(Promo x, Promo y) {
            DateTimeOffset? startdDate1 = x.StartDate;
            DateTimeOffset? endDate1 = x.EndDate;
            int? priority1 = x.CalendarPriority;

            DateTimeOffset? startdDate2 = y.StartDate;
            DateTimeOffset? endDate2 = y.EndDate;
            int? priority2 = y.CalendarPriority;

            bool fullEqual = (startdDate1.Value == startdDate2.Value) && (priority1.Value == priority2.Value);
            bool equalStart = (startdDate1.Value == startdDate2.Value);
            bool equalPriority = priority1.Value == priority2.Value;
            if (fullEqual) {
                return 0;
            } else if (equalStart) {
                if (equalPriority) {
                    return endDate1.Value > endDate2.Value ? -1 : 1;
                } else {
                    return priority1 < priority2 ? -1 : 1;
                }
            } else {
                if (equalPriority) {
                    return startdDate1.Value < startdDate2.Value ? -1 : 1;
                } else {
                    return priority1 < priority2 ? -1 : 1;
                }
            }
        }
    }
}