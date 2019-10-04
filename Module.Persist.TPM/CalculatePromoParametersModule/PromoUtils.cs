using Core.Dependency;
using Core.Settings;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.Utils;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.CalculatePromoParametersModule
{
    public static class PromoUtils
    {
        private static ISettingsManager SettingsManager
        {
            get
            {
                return (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            }
        }

        public static bool NeedBackToOnApproval(Promo promo)
        {
            var backToOnApprovalDispatchDays = SettingsManager.GetSetting<int>("BACK_TO_ON_APPROVAL_DISPATCH_DAYS_COUNT", 7 * 8);

            bool isCorrectDispatchDifference = promo.DispatchesStart < promo.StartDate ?
                                               (promo.DispatchesStart - ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)).Value.Days >= backToOnApprovalDispatchDays :
                                               true;

            return !isCorrectDispatchDifference;
        }
    }
}
