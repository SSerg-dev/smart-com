﻿using Looper.Core;
using Looper.Parameters;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model;
using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.LogWriter;

namespace Module.Host.TPM.Handlers
{
    public class CalculateActualParamatersHandler : BaseHandler
    {
        public string logLine = "";
        public override void Action(HandlerInfo info, ExecuteData data)
        {
            LogWriter handlerLogger = null;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            handlerLogger = new LogWriter(info.HandlerId.ToString());
            logLine = String.Format("The calculation of the actual parameters began at {0:yyyy-MM-dd HH:mm:ss}", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow));
            handlerLogger.Write(true, logLine, "Message");
            handlerLogger.Write(true, "");

            Guid promoId = HandlerDataHelper.GetIncomingArgument<Guid>("PromoId", info.Data, false);
            Guid RoleId = HandlerDataHelper.GetIncomingArgument<Guid>("RoleId", info.Data, false);
            bool? needRedistributeLSV = HandlerDataHelper.GetIncomingArgument<bool?>("needRedistributeLSV", info.Data, false);
            bool needToSaveChanges = false;

            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    Promo promo = context.Set<Promo>().FirstOrDefault(n => n.Id == promoId && !n.Disabled);
                    Role role = context.Set<Role>().FirstOrDefault(x => x.Id == RoleId);
                    bool isSupportAdmin = false;
                    if (role.SystemName == "SupportAdministrator")
                    {
                        isSupportAdmin = true;
                    }
                    if (promo != null && (promo.PromoStatus.SystemName == "Finished" || (promo.PromoStatus.SystemName == "Closed" && role != null && role.SystemName == "SupportAdministrator")))
                    {
                        string errorString = null;
                        // Продуктовые параметры считаем только, если были загружены Actuals
                        var promoProductList = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled).ToList();
                        if (!promo.LoadFromTLC && promoProductList.Any(x => x.ActualProductPCQty.HasValue))
                        {
                            if (needRedistributeLSV == true)
                            {
                                //Хак, что бы пересчет не переписывать
                                ActualProductParametersCalculation.CalculatePromoProductParameters(promo, context, isSupportAdmin: isSupportAdmin, needToSaveChanges: needToSaveChanges);
                                ActualLSVChangeHandler.CalculateAllActualLSV(promo, context);
                            }
                            // если есть ошибки, они перечисленны через ;
                            errorString = ActualProductParametersCalculation.CalculatePromoProductParameters(promo, context, isSupportAdmin: isSupportAdmin, needToSaveChanges: needToSaveChanges);
                            // записываем ошибки если они есть
                            if (errorString != null)
                                WriteErrorsInLog(handlerLogger.CurrentLogWriter, errorString);
                        }

                        // пересчет фактических бюджетов (из-за LSV)
                        BudgetsPromoCalculation.CalculateBudgets(promo, false, true, handlerLogger.CurrentLogWriter, info.HandlerId, context);

                        BTL btl = context.Set<BTLPromo>().Where(x => x.PromoId == promo.Id && !x.Disabled && x.DeletedDate == null).FirstOrDefault()?.BTL;
                        if (btl != null)
                        {
                            BudgetsPromoCalculation.CalculateBTLBudgets(btl, false, true, handlerLogger.CurrentLogWriter, context);
                        }

                        // Параметры промо считаем только, если промо из TLC или если были загружены Actuals
                        if (promo.LoadFromTLC || promoProductList.Any(x => x.ActualProductPCQty.HasValue))
                        {
                            errorString = ActualPromoParametersCalculation.CalculatePromoParameters(promo, context, needToSaveChanges: needToSaveChanges);
                            PromoHelper.CalculateSumInvoiceProduct(context, promo);
                        }

                        // записываем ошибки если они есть
                        if (errorString != null)
                            WriteErrorsInLog(handlerLogger.CurrentLogWriter, errorString);
                    }

                    //CalculationTaskManager.UnLockPromo(promo.Id);
                    CalculationTaskManager.UnLockPromoForHandler(info.HandlerId);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                handlerLogger.Write(true, e.ToString(), "Error");
                CalculationTaskManager.UnLockPromoForHandler(info.HandlerId);
            }

            sw.Stop();
            handlerLogger.Write(true, "");
            logLine = String.Format("The calculation of the actual parameters was completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow), sw.Elapsed.TotalSeconds);
            handlerLogger.Write(true, logLine, "Message");
            handlerLogger.UploadToBlob();
        }

        /// <summary>
        /// Записать ошибки в лог
        /// </summary>
        /// <param name="handlerLogger">Лог</param>
        /// <param name="errorString">Список ошибок, записанных через ';'</param>
        private void WriteErrorsInLog(ILogWriter handlerLogger, string errorString)
        {
            string[] errors = errorString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string message = "";
            foreach (string e in errors)
                message += e + "\n";

            handlerLogger.Write(true, message);
        }
    }
}
