using Core.Dependency;
using Core.Extensions;
using Core.Settings;
using Interfaces.Implementation.Action;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Actions
{
    /// <summary>
    /// Класс для расчета значения COGSPercent в промо
    /// </summary>
    public class SetCOGSPercentValuesAction : BaseAction
    {
        private readonly string updatePromoScriptTemplate = "UPDATE [Promo] SET [PlanCOGSPercent] = {0}, [ActualCOGSPercent] = {1} WHERE Id = '{2}' \n";

        public override void Execute()
        {
            try
            {
                IList<string> updatePromoScript = new List<string>();
                using (DatabaseContext context = new DatabaseContext())
                {
                    var currentYear = DateTimeOffset.Now.Year;
                    var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                    var statusesSetting = settingsManager.GetSetting<string>("ACTUAL_COGSTI_CHECK_PROMO_STATUS_LIST", "Finished,Closed");
                    var checkPromoStatuses = statusesSetting.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    var previousYearsPromoIds = context.Set<Promo>()
                        .Where(x => !x.Disabled && x.StartDate.HasValue && x.StartDate.Value.Year != currentYear)
                        .Where(x => checkPromoStatuses.Contains(x.PromoStatus.Name))
                        .Select(x => x.Id).ToList();

                    var promoes = context.Set<Promo>().Where(x => !x.Disabled);
                    IQueryable<ActualCOGS> actualCOGSQuery = context.Set<ActualCOGS>().Where(x => !x.Disabled);
                    IQueryable<COGS> COGSQuery = context.Set<COGS>().Where(x => !x.Disabled);
                    double? PlanCOGSPercent = null;
                    double? ActualCOGSPercent = null;
                    string message = null;

                    foreach (var promo in promoes)
                    {
                        message = null;
                        PlanCOGSPercent = null;
                        ActualCOGSPercent = null;
                        SimplePromoCOGS simplePromoCOGS = new SimplePromoCOGS(promo);
                        if (previousYearsPromoIds.Contains(promo.Id))
                        {
                            ActualCOGSPercent = PromoUtils.GetCOGSPercent(simplePromoCOGS, context, actualCOGSQuery, out message);
                            if (ActualCOGSPercent == null)
                            {
                                ActualCOGSPercent = PromoUtils.GetCOGSPercent(simplePromoCOGS, context, COGSQuery, out message);
                            }
                        }
                        else
                        {
                            PlanCOGSPercent = PromoUtils.GetCOGSPercent(simplePromoCOGS, context, COGSQuery, out message);
                        }

                        if (message != null)
                        {
                            Warnings.Add(message);
                        }

                        updatePromoScript.Add(string.Format(updatePromoScriptTemplate,
                                                            PlanCOGSPercent.HasValue ? PlanCOGSPercent.Value.ToString() : "NULL",
                                                            ActualCOGSPercent.HasValue ? ActualCOGSPercent.Value.ToString() : "NULL",
                                                            promo.Id));
                    }
                }

                using (DatabaseContext context = new DatabaseContext())
                {
                    foreach (var s in updatePromoScript.Partition(10000))
                    {
                        string updateScript = String.Join("", s);
                        context.Database.ExecuteSqlCommand(updateScript);
                    }
                }
            }
            catch (Exception e)
            {
                string msg = String.Format("An error occurred while calculation: {0}", e.ToString());
                Errors.Add(msg);
            }
        }
    }
}
