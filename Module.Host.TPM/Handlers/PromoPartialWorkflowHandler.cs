using Core.Dependency;
using Core.Settings;
using Looper.Core;
using Module.Frontend.TPM.Controllers;
using Module.Frontend.TPM.Util;
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
    class PromoPartialWorkflowHandler : BaseHandler
    {
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                using (var context = new DatabaseContext())
                {
                    handlerLogger = new LogWriter(info.HandlerId.ToString());
                    handlerLogger.Write(true, String.Format("Partial workflow processing started at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");

                    var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                    var promoNumbersRecalculatingString = settingsManager.GetSetting<string>("PROMO_PARTIAL_WORKFLOW_LIST");
                    var promoNumbers = promoNumbersRecalculatingString.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    var promoes = new List<Promo>();

                    List<Mechanic> mechanics = context.Set<Mechanic>().Where(g => !g.Disabled).ToList();
                    List<MechanicType> mechanicTypes = context.Set<MechanicType>().Where(g => !g.Disabled).ToList();
                    List<ClientTree> clientTrees = context.Set<ClientTree>().Where(g => g.EndDate == null).ToList();
                    List<ProductTree> productTrees = context.Set<ProductTree>().Where(g => g.EndDate == null).ToList();
                    List<Brand> brands = context.Set<Brand>().Where(g => !g.Disabled).ToList();
                    List<Technology> technologies = context.Set<Technology>().Where(g => !g.Disabled).ToList();
                    List<BrandTech> brandTeches = context.Set<BrandTech>().Where(g => !g.Disabled).ToList();
                    List<Color> colors = context.Set<Color>().Where(g => !g.Disabled).ToList();
                    foreach (var promoNumber in promoNumbers)
                    {
                        int number;
                        if (int.TryParse(promoNumber, out number))
                        {
                            var promo = context.Set<Promo>().FirstOrDefault(x => x.Number == number);
                            if (promo != null)
                            {
                                promoes.Add(promo);
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

                    if (promoes.Count > 0)
                    {
                        var swPlanParameters = new Stopwatch();
                        var draftStatus = context.Set<PromoStatus>().FirstOrDefault(x => x.SystemName.ToLower() == "draft");
                        var startedStatus = context.Set<PromoStatus>().FirstOrDefault(x => x.SystemName.ToLower() == "started");

                        foreach (var promo in promoes)
                        {
                            if (promo.PromoStatusId == draftStatus.Id)
                            {
                                handlerLogger.Write(true, String.Format("Calculation of promo number {0}", promo.Number), "Message");

                                bool isSubrangeChanged = false;
                                List<PromoProductTree> promoProductTrees = PromoHelper.AddProductTrees(promo.ProductTreeObjectIds, promo, out isSubrangeChanged, context);
                                PromoHelper.SetPromoByProductTree(promo, promoProductTrees, productTrees, brands, technologies, brandTeches, colors);
                                PromoHelper.SetPromoMarsDates(promo);
                                PromoHelper.SetPromoByClientTree(promo, clientTrees);
                                PromoHelper.SetMechanic(promo, mechanics, mechanicTypes);
                                PromoHelper.SetMechanicIA(promo, mechanics, mechanicTypes);

                                try
                                {
                                    List<Product> filteredProducts;
                                    PromoHelper.CheckSupportInfo(promo, promoProductTrees, out filteredProducts, context);
                                }
                                catch (Exception e)
                                {
                                    handlerLogger.Write(true, e.Message, "Error");
                                    continue;
                                }

                                promo.PromoStatusId = startedStatus.Id;
                                promo.NeedRecountUplift = false;
                                promo.IsCMManagerApproved = true;
                                promo.IsDemandFinanceApproved = true;
                                promo.IsDemandPlanningApproved = true;
                                promo.IsGAManagerApproved = true;

                                var promoProductTree = context.Set<PromoProductTree>().FirstOrDefault(x => x.PromoId == promo.Id);
                                var promoNameProductTreeAbbreviations = "";
                                if (promoProductTree != null)
                                {
                                    var productTree = context.Set<ProductTree>().FirstOrDefault(x => x.ObjectId == promoProductTree.ProductTreeObjectId);
                                    if (productTree != null)
                                    {
                                        if (productTree.Type != "Brand")
                                        {
                                            var currentTreeNode = productTree;
                                            while (currentTreeNode != null && currentTreeNode.Type != "Brand")
                                            {
                                                currentTreeNode = context.Set<ProductTree>().FirstOrDefault(x => x.ObjectId == currentTreeNode.parentId);
                                            }
                                            promoNameProductTreeAbbreviations = currentTreeNode.Abbreviation;
                                        }
                                        promoNameProductTreeAbbreviations = promoNameProductTreeAbbreviations + " " + productTree.Abbreviation;
                                    }
                                }

                                var mechanic = context.Set<Mechanic>().FirstOrDefault(x => x.Id == promo.MarsMechanicId);
                                var promoNameMechanic = "";
                                if (mechanic != null)
                                {
                                    promoNameMechanic = mechanic.Name;
                                    if (mechanic.SystemName == "TPR" || mechanic.SystemName == "Other")
                                    {
                                        promoNameMechanic += " " + promo.MarsMechanicDiscount + "%";
                                    }
                                    else
                                    {
                                        promoNameMechanic += " " + promo.MarsMechanicType.Name;
                                    }
                                }

                                promo.Name = promoNameProductTreeAbbreviations + " " + promoNameMechanic;

                                swPlanParameters.Restart();

                                handlerLogger.Write(true, String.Format("Calculation of planned parameters began at {0:yyyy-MM-dd HH:mm:ss}. It may take some time.",
                                    ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)), "Message");

                                string setPromoProductError;
                                PlanProductParametersCalculation.SetPromoProduct(promo.Id, context, out setPromoProductError);
                                if (setPromoProductError != null)
                                {
                                    handlerLogger.Write(true, String.Format("Error filling Product: {0}", setPromoProductError), "Error");
                                }

                                string calculateBaselineError = PlanProductParametersCalculation.CalculateBaseline(context, promo.Id);
                                string calculateError = PlanProductParametersCalculation.CalculatePromoProductParameters(promo.Id, context);

                                if (calculateBaselineError != null && calculateError != null)
                                {
                                    calculateError += calculateBaselineError;
                                }
                                else if (calculateBaselineError != null && calculateError == null)
                                {
                                    calculateError = calculateBaselineError;
                                }

                                if (calculateError != null)
                                {
                                    handlerLogger.Write(true, String.Format("Error when calculating the planned parameters of the Product: {0}", calculateError), "Error");
                                }

                                calculateError = PlanPromoParametersCalculation.CalculatePromoParameters(promo.Id, context);
                                if (calculateError != null)
                                {
                                    handlerLogger.Write(true, String.Format("Error when calculating the planned parameters Promo: {0}", calculateError), "Error");
                                }

                                swPlanParameters.Stop();
                                handlerLogger.Write(true, String.Format("Calculation of planned parameters was completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds",
                                    ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), swPlanParameters.Elapsed.TotalSeconds), "Message");
                            }
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
                    handlerLogger.Write(true, String.Format("Partial workflow processing ended at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds), "Message");
                    handlerLogger.UploadToBlob();
                }
            }
        }
    }
}
