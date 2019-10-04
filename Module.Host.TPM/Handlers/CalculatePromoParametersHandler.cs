using ProcessingHost.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Looper.Core;
using Module.Persist.TPM.Model.TPM;
using Looper.Parameters;
using Persist;
using System.Data.Entity;
using Utility.LogWriter;
using System.Diagnostics;
using System.Data;
using Module.Persist.TPM.CalculatePromoParametersModule;
using System.Threading;
using Core.Security.Models;
using Persist.Model;
using Module.Persist.TPM;
using Core.Settings;
using Core.Dependency;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.Utils;

namespace Module.Host.TPM.Handlers
{
	/// <summary>
	/// 
	/// </summary>
	public class CalculatePromoParametersHandler : BaseHandler
	{
		public string logLine = "";
		public override void Action(HandlerInfo info, ExecuteData data)
		{
			using (DatabaseContext context = new DatabaseContext())
			{
				ILogWriter handlerLogger = null;
				Stopwatch sw = new Stopwatch();
				sw.Start();

				handlerLogger = new FileLogWriter(info.HandlerId.ToString());
				logLine = String.Format("The calculation of the parameters started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now);
				handlerLogger.Write(true, logLine, "Message");
				handlerLogger.Write(true, "");

				Guid nullGuid = new Guid();
				Guid promoId = HandlerDataHelper.GetIncomingArgument<Guid>("PromoId", info.Data, false);
				bool needCalculatePlanMarketingTI = HandlerDataHelper.GetIncomingArgument<bool>("NeedCalculatePlanMarketingTI", info.Data, false);

				try
				{
					if (promoId != nullGuid)
					{
						Promo promo = context.Set<Promo>().Where(x => x.Id == promoId).FirstOrDefault();

						//добавление номера рассчитываемого промо в лог
						handlerLogger.Write(true, String.Format("Calculating promo: №{0}", promo.Number), "Message");
						handlerLogger.Write(true, "");

						//Подбор исторических промо и расчет PlanPromoUpliftPercent
						if (NeedUpliftFinding(promo))
						{
							// Uplift не расчитывается для промо в статусе Starte, Finished, Closed
							if (promo.PromoStatus.SystemName != "Started" && promo.PromoStatus.SystemName != "Finished" && promo.PromoStatus.SystemName != "Closed")
							{
								Stopwatch swUplift = new Stopwatch();
								swUplift.Start();
								logLine = String.Format("Pick plan promo uplift started at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now);
								handlerLogger.Write(true, logLine, "Message");

								string upliftMessage;
								double? planPromoUpliftPercent = PlanPromoUpliftCalculation.FindPlanPromoUplift(promoId, context, out upliftMessage);

								if (planPromoUpliftPercent != -1)
								{
									logLine = String.Format("{0}: {1}", upliftMessage, planPromoUpliftPercent);
									handlerLogger.Write(true, logLine, "Message");
								}
								else
								{
									logLine = String.Format("{0}", upliftMessage);
									handlerLogger.Write(true, logLine, "Message");
								}

								swUplift.Stop();
								logLine = String.Format("Pick plan promo uplift completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, swUplift.Elapsed.TotalSeconds);
								handlerLogger.Write(true, logLine, "Message");
								handlerLogger.Write(true, "");
							}
							else
							{
								logLine = String.Format("{0}", "Plan promo uplift is not picking for started, finished and closed promo.");
								handlerLogger.Write(true, logLine, "Message");
							}
						}

                        // возможно, придется это вернуть, НЕ УДАЛЯТЬ КОММЕНТАРИЙ
                        // если уже произошла отгрузка продукта, то перезаполнение таблицы PromoProduct не осуществляется
                        //if (promo.DispatchesStart.HasValue && promo.DispatchesStart.Value > DateTimeOffset.Now)
                        //{
                        //    string setPromoProductError;
                        //    PlanProductParametersCalculation.SetPromoProduct(promoId, context, out setPromoProductError);
                        //    if (setPromoProductError != null)
                        //    {
                        //        logLine = String.Format("Error filling Product: {0}", setPromoProductError);
                        //        handlerLogger.Write(true, logLine, "Error");
                        //    }
                        //}

                        //статусы, в которых не должен производиться пересчет плановых параметров промо
                        ISettingsManager settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                        string promoStatusesNotPlanRecalculateSetting = settingsManager.GetSetting<string>("NOT_PLAN_RECALCULATE_PROMO_STATUS_LIST", "Started,Finished,Closed");
                        string[] statuses = promoStatusesNotPlanRecalculateSetting.Split(',');

                        // Плановые параметры не расчитывается для промо в статусах из настроек
                        if (!statuses.Contains(promo.PromoStatus.SystemName))
                        {
                            //Pасчет плановых параметров Product и Promo
                            Stopwatch swPromoProduct = new Stopwatch();
                            swPromoProduct.Start();
                            logLine = String.Format("Calculation of plan parameters began at {0:yyyy-MM-dd HH:mm:ss}. It may take some time.", DateTimeOffset.Now);
                            handlerLogger.Write(true, logLine, "Message");

                            string calculateError = null;
                            if (!promo.LoadFromTLC)
                            {
                                string setPromoProductError;
                                bool needReturnToOnApprovalStatus = false;

                                // в день старта промо продукты не переподбираются
                                bool isStartToday = promo.StartDate != null &&
                                                    (promo.StartDate - ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)).Value.Days == 0;
                                if (!isStartToday)
                                {
                                    needReturnToOnApprovalStatus = PlanProductParametersCalculation.SetPromoProduct(promoId, context, out setPromoProductError);
                                    if (setPromoProductError != null)
                                    {
                                        logLine = String.Format("Error filling Product: {0}", setPromoProductError);
                                        handlerLogger.Write(true, logLine, "Error");
                                    }
                                }

                                // пересчет baseline должен происходить до попытки согласовать промо, т.к. к зависимости от результата пересчета
                                // резльтат согласования может быть разный (после пересчета baseline может оказаться равен 0, тогда автосогласования не будет)
                                calculateError = PlanProductParametersCalculation.CalculatePromoProductParameters(promoId, context);

                                string[] canBeReturnedToOnApproval = { "OnApproval", "Approved", "Planned" };
								if (needReturnToOnApprovalStatus && canBeReturnedToOnApproval.Contains(promo.PromoStatus.SystemName))
								{
									PromoStatus draftPublished = context.Set<PromoStatus>().First(x => x.SystemName.ToLower() == "draftpublished" && !x.Disabled);
									promo.PromoStatus = draftPublished;
									promo.PromoStatusId = draftPublished.Id;
									promo.IsAutomaticallyApproved = false;
									promo.IsCMManagerApproved = false;
									promo.IsDemandFinanceApproved = false;
									promo.IsDemandPlanningApproved = false;
									PromoStateContext promoStateContext = new PromoStateContext(context, promo);

									PromoStatus onApproval = context.Set<PromoStatus>().First(x => x.SystemName.ToLower() == "onapproval" && !x.Disabled);
									promo.PromoStatus = onApproval;
									promo.PromoStatusId = onApproval.Id;

                                    string statusChangeError;
                                    var status = promoStateContext.ChangeState(promo, "System", out statusChangeError);
                                    if (status)
                                    {
                                        //Сохранение изменения статуса
                                        var promoStatusChange = context.Set<PromoStatusChange>().Create<PromoStatusChange>();
                                        promoStatusChange.PromoId = promo.Id;
                                        promoStatusChange.StatusId = promo.PromoStatusId;
                                        promoStatusChange.Date = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).Value;

                                        context.Set<PromoStatusChange>().Add(promoStatusChange);
                                    }

                                    if (statusChangeError != null && statusChangeError != string.Empty)
									{
										logLine = String.Format("Error while changing status of promo: {0}", statusChangeError);
										handlerLogger.Write(true, logLine, "Error");
									}
								}
                            }

                            if (calculateError != null)
                            {
                                logLine = String.Format("Error when calculating the planned parameters of the Product: {0}", calculateError);
                                handlerLogger.Write(true, logLine, "Error");
                            }

                            // пересчет плановых бюджетов (из-за LSV)
                            BudgetsPromoCalculation.CalculateBudgets(promo, true, false, handlerLogger, info.HandlerId, context);

                            calculateError = PlanPromoParametersCalculation.CalculatePromoParameters(promoId, context);

                            if (calculateError != null)
                            {
                                logLine = String.Format("Error when calculating the plan parameters Promo: {0}", calculateError);
                                handlerLogger.Write(true, logLine, "Error");
                            }

                            swPromoProduct.Stop();
                            logLine = String.Format("Calculation of plan parameters was completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, swPromoProduct.Elapsed.TotalSeconds);
                            handlerLogger.Write(true, logLine, "Message");
                            handlerLogger.Write(true, "");
                        }
                        else
                        {
                            logLine = String.Format("Plan parameters don't recalculate for promo in statuses: {0}", promoStatusesNotPlanRecalculateSetting);
                            handlerLogger.Write(true, logLine, "Warning");
                        }

                        if (promo.PromoStatus.SystemName == "Finished")
                        {
                            Stopwatch swActual = new Stopwatch();
                            swActual.Start();
                            logLine = String.Format("The calculation of the actual parameters began at {0:yyyy-MM-dd HH:mm:ss}", DateTimeOffset.Now);
                            handlerLogger.Write(true, logLine, "Message");
                            handlerLogger.Write(true, "");

                            CalulateActual(promo, context, handlerLogger, info.HandlerId);

                            swActual.Stop();
                            handlerLogger.Write(true, "");
                            logLine = String.Format("The calculation of the actual parameters was completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, swActual.Elapsed.TotalSeconds);
                            handlerLogger.Write(true, logLine, "Message");
                        }

						//promo.Calculating = false;
						context.SaveChanges();
					}
				}
				catch (Exception e)
				{
					handlerLogger.Write(true, e.Message, "Error");
				}
				finally
				{
					sw.Stop();
					handlerLogger.Write(true, "");
					logLine = String.Format("Calculation of parameters completed at {0:yyyy-MM-dd HH:mm:ss}. Duration: {1} seconds", DateTimeOffset.Now, sw.Elapsed.TotalSeconds);
					handlerLogger.Write(true, logLine, "Message");

					//разблокировку промо необходимо производить после всего, иначе не успевает прочитаться последнее сообщение и (самое главное) окончательный статус хендлера
					//для перестраховки добавлена небольшая задержка
					if (promoId != nullGuid)
					{
						Promo promo = context.Set<Promo>().Where(x => x.Id == promoId).FirstOrDefault();
						//TODO: УБРАТЬ ЗАДЕРЖКУ !!!
						Thread.Sleep(5000);

						CalculationTaskManager.UnLockPromoForHandler(info.HandlerId);
						//CalculationTaskManager.UnLockPromo(promo.Id);
					}
				}
			}
		}

        /// <summary>
        /// Расчитать фактические показатели для Промо
        /// </summary>
        /// <param name="promo">Промо</param>
        /// <param name="context">Контекст БД</param>
        /// <param name="handlerLogger">Лог</param>
        private void CalulateActual(Promo promo, DatabaseContext context, ILogWriter handlerLogger, Guid handlerId)
        {
            string errorString = null;
            // Продуктовые параметры считаем только, если были загружены Actuals
            var promoProductList = context.Set<PromoProduct>().Where(x => x.PromoId == promo.Id && !x.Disabled).ToList();
            if (!promo.LoadFromTLC && promoProductList.Any(x => x.ActualProductPCQty.HasValue))
            {
                // если есть ошибки, они перечисленны через ;
                errorString = ActualProductParametersCalculation.CalculatePromoProductParameters(promo, context);
                // записываем ошибки если они есть
                if (errorString != null)
                    WriteErrorsInLog(handlerLogger, errorString);
            }
            // пересчет фактических бюджетов (из-за LSV)
            BudgetsPromoCalculation.CalculateBudgets(promo, false, true, handlerLogger, handlerId, context);

            // Параметры промо считаем только, если промо из TLC или если были загружены Actuals
            if (promo.LoadFromTLC || promoProductList.Any(x => x.ActualProductPCQty.HasValue))
            {
                errorString = ActualPromoParametersCalculation.CalculatePromoParameters(promo, context);
            }

			// записываем ошибки если они есть
			if (errorString != null)
				WriteErrorsInLog(handlerLogger, errorString);
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

		private bool NeedUpliftFinding(Promo promo)
		{
			// если стоит флаг inout, то подбирать uplift не требуется
			return (!promo.NeedRecountUplift.HasValue || promo.NeedRecountUplift.Value) && (!promo.InOut.HasValue || !promo.InOut.Value);
		}
	}
}