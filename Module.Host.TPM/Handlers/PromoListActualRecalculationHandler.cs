using Core.Dependency;
using Core.Settings;
using Looper.Core;
using Module.Frontend.TPM.Controllers;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.Utils;
using Persist;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers
{
    class PromoListActualRecalculationHandler : BaseHandler
    {
        public string logLine = "";
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                var promoesToRecalculate = new List<Promo>();
                var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));

                using (var prev_context = new DatabaseContext())
                {
                    handlerLogger = new LogWriter(info.HandlerId.ToString());
                    handlerLogger.Write(true, String.Format("Plan parameters recalculation started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");

                    var promoNumbersRecalculatingString = settingsManager.GetSetting<string>("PROMO_LIST_FOR_RECALCULATION");
                    var promoNumbers = promoNumbersRecalculatingString.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    var activePromo = prev_context.Set<Promo>().Where(x => !x.Disabled);

                    String formatStrPromo = "UPDATE [DefaultSchemaSetting].[Promo] SET PromoStatusId = (SELECT Id FROM PromoStatus WHERE SystemName = 'Finished' AND Disabled = 0) WHERE Number = {0} \n";
                    string updateScript = "";

                    foreach (var promoNumber in promoNumbers)
                    {
                        int number;
                        if (int.TryParse(promoNumber, out number))
                        {
                            var promo = activePromo.FirstOrDefault(x => x.Number == number);
                            if (promo != null)
                            {
                                promoesToRecalculate.Add(promo);
                                updateScript += String.Format(formatStrPromo, promo.Number);
                            }
                            else
                            {
                                handlerLogger.Write(true, $"The promo with { promo.Number } number not found.", "Error");
                            }
                        }
                        else
                        {
                            handlerLogger.Write(true, $"The { number } is not a number.", "Error");
                        }
                    }

                    if (!String.IsNullOrEmpty(updateScript))
                    {
                        prev_context.ExecuteSqlCommand(updateScript);
                    }
                }

                using (var context = new DatabaseContext())
                {
                    var activePromo = context.Set<Promo>().Where(x => !x.Disabled);
                    var promoIdsToRecalculate = promoesToRecalculate.Select(y => y.Id);
                    var query = activePromo.Where(x => promoIdsToRecalculate.Contains(x.Id));
                    var promoesList = query.ToList();

                    if (promoesList.Count > 0)
                    {
                        foreach(var promo in promoesList)
                        {
                            handlerLogger.Write(true, String.Format("Calculating promo: №{0}", promo.Number), "Message");
                            handlerLogger.Write(true, "");

                            if (promo.PromoStatus.SystemName == "Finished")
                            {
                                Stopwatch swActual = new Stopwatch();
                                swActual.Start();
                                logLine = String.Format("The calculation of the actual parameters began at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow));
                                handlerLogger.Write(true, logLine, "Message");
                                handlerLogger.Write(true, "");

                                var previousYear = DateTimeOffset.Now.AddYears(-1).Year;
                                var useActualCOGS = false;
                                var useActualTI = false;
                                if (promo.StartDate.HasValue && promo.StartDate.Value.Year == previousYear)
                                {
                                    useActualCOGS = true;
                                    useActualTI = true;
                                }

                                CalculatePromoParametersHandler.CalulateActual(promo, context, handlerLogger.CurrentLogWriter, info.HandlerId, 
                                    calculateBudgets: false, useActualCOGS: useActualCOGS, useActualTI: useActualTI);

                                swActual.Stop();
                                handlerLogger.Write(true, "");
                                logLine = String.Format("The calculation of the actual parameters was completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), swActual.Elapsed.TotalSeconds);
                                handlerLogger.Write(true, logLine, "Message");
                            }

                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        handlerLogger.Write(true, "No promo found.", "Error");
                    }
                }
            }
            catch (Exception e)
            {
                data.SetValue<bool>("HasErrors", true);
                logger.Error(e);

                if (handlerLogger != null)
                {
                    handlerLogger.Write(true, e.ToString(), "Error");
                }
            }
            finally
            {
                logger.Debug("Finish '{0}'", info.HandlerId);
                sw.Stop();

                if (handlerLogger != null)
                {
                    handlerLogger.Write(true, String.Format("Plan parameters recalculation ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }
    }
}
