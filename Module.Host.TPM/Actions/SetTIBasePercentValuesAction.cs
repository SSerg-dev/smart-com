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
    /// Класс для расчета значения TIBasePersent в промо
    /// </summary>
    public class SetTIBasePercentValuesAction : BaseAction
    {
        private readonly string updatePromoScriptTemplate = "UPDATE [Promo] SET [PlanTIBasePercent] = {0}, [ActualTIBasePercent] = {1} WHERE Id = '{2}' \n";

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
                    IQueryable<ActualTradeInvestment> actualTIQuery = context.Set<ActualTradeInvestment>().Where(x => !x.Disabled);
                    IQueryable<TradeInvestment> TIQuery = context.Set<TradeInvestment>().Where(x => !x.Disabled);
                    double? PlanTIBasePercent = null;
                    double? ActualTIBasePercent = null;
                    string message = null;
                    bool error;

                    foreach (var promo in promoes)
                    {
                        message = null;
                        PlanTIBasePercent = null;
                        ActualTIBasePercent = null;
                        SimplePromoTradeInvestment simplePromoTradeInvestment = new SimplePromoTradeInvestment(promo);
                        if (previousYearsPromoIds.Contains(promo.Id))
                        {
                            ActualTIBasePercent = PromoUtils.GetTIBasePercent(simplePromoTradeInvestment, context, actualTIQuery, out message, out error);
                            if (ActualTIBasePercent == null)
                            {
                                ActualTIBasePercent = PromoUtils.GetTIBasePercent(simplePromoTradeInvestment, context, TIQuery, out message, out error);
                            }
                        }
                        else
                        {
                            PlanTIBasePercent = PromoUtils.GetTIBasePercent(simplePromoTradeInvestment, context, TIQuery, out message, out error);
                        }

                        if (message != null)
                        {
                            Warnings.Add(message);
                        }

                        updatePromoScript.Add(string.Format(updatePromoScriptTemplate,
                                                            PlanTIBasePercent.HasValue ? PlanTIBasePercent.Value.ToString() : "NULL",
                                                            ActualTIBasePercent.HasValue ? ActualTIBasePercent.Value.ToString() : "NULL",
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
