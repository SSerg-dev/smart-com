using AutoMapper;
using Core.MarsCalendar;
using Interfaces.Implementation.Action;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Host.TPM.Actions
{
    public class RollingVolumeQTYRecalculationActions : BaseAction
    {
        public override void Execute()
        {
            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    IEnumerable<RollingVolume> query = context.Set<RollingVolume>();
                    var queryToDifferrence = BuildDTORollingVolumeList(query, context);
                    try
                    {
                        foreach (var item in queryToDifferrence)
                        {
                            var proxy = context.Set<PromoProductDifference>().Create<PromoProductDifference>();
                            var result = (PromoProductDifference)Mapper.Map(item, proxy, typeof(PromoProductDifference), proxy.GetType(), opts => opts.CreateMissingTypeMaps = true);
                            context.Set<PromoProductDifference>().Add(result);
                        }

                        context.SaveChanges();

                    }
                    catch (Exception e)
                    {
                        Errors.Add(String.Format("Error during : {0}", e.ToString()));
                    }

                }
            }
            catch (Exception e)
            {
                string msg = String.Format("An error occurred while inserting: {0}", e.ToString());
                Errors.Add(msg);
            }
        }

        private IEnumerable<PromoProductDifference> BuildDTORollingVolumeList(IEnumerable<RollingVolume> query, DatabaseContext context)
        {
            List<PromoProductDifference> result = new List<PromoProductDifference>();
            var groupedRollingVolume = query.Select(y => (RollingVolume)y).GroupBy(bl => new { bl.DMDGroup, bl.Product });
            var promotionStartDate = GetPromotionStartDate(context);

            if (query != null)
            {
                foreach (var item in groupedRollingVolume)
                {
                    var dmd = item.Key.DMDGroup;
                    var ghierarhy = GetGHierarhyCode(context, dmd);
                    var salesDistChannel = context.Database.SqlQuery<string>(
                       String.Format("SELECT TOP 1 [0DISTR_CHAN] FROM dbo.MARS_UNIVERSAL_PETCARE_CUSTOMERS where ZCUSTHG04 = {0} ", ghierarhy)).FirstOrDefault();

                    if (String.IsNullOrEmpty(salesDistChannel))
                    {
                        Warnings.Add(String.Format("Not found sales dist channel for DMDGroup = {0} and ZREP = {1}",item.Key.DMDGroup,item.Key.Product.ZREP));
                    }

                    var startDate = new MarsDate(item.FirstOrDefault().Week).ToDateTime();

                    result.Add(new PromoProductDifference()
                    {
                        DemandUnit = "000000000000" + item.Key.Product.ZREP + "_0125",
                        DMDGROUP = "00" + item.Key.DMDGroup,
                        LOC = "RU",
                        StartDate = GetFmtStringDate(startDate),
                        DURInMinutes = 10080,
                        Type = "7",
                        ForecastID = string.Format("ROLLING_{0}", item.Key.DMDGroup),
                        QTY = Math.Round(Convert.ToDecimal(item.Select(e => e.ManualRollingTotalVolumes).Sum()),6),
                        MOE = "0125",
                        Source = "Jupiter",
                        SALES_ORG = "261",
                        SALES_DIST_CHANNEL = String.IsNullOrEmpty(salesDistChannel) ? "0" : salesDistChannel,
                        SALES_DIVISON = "51",
                        BUS_SEG = "05",
                        MKT_SEG = item.Key.Product.Segmen_code.TrimStart('0'),
                        DELETION_FLAG = "N",
                        DELETION_DATE = "99991231 23:59:59",
                        INTEGRATION_STAMP = GetFmtStringDate((DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)),
                        Roll_FC_Flag = "0",
                        Promotion_Start_Date = promotionStartDate,
                        Promotion_Duration = 36500,
                        Promotion_Status = "Auto Approved",
                        Promotion_Campaign = "JPT"
                    });
                }
                return result.AsEnumerable();
            }
            else
                return null;
        }

        private string GetPromotionStartDate(DatabaseContext context)
        {
            var promotionStartDate = context.Set<ServiceInfo>().Where(e => e.Name.Equals("ROLLING_PROMO_START_DATE")).FirstOrDefault();

            DateTimeOffset date;
            DateTimeOffset.TryParse(promotionStartDate?.Value, out date);
            var strDate = GetFmtStringDate(date);

            return strDate;
        }

        private string GetGHierarhyCode(DatabaseContext context, string DMDGroup)
        { 
            int objectId = context.Set<ClientTree>().Where(e => e.DMDGroup.Equals(DMDGroup) && e.EndDate == null).FirstOrDefault().ObjectId;
            string gHierarhyCode = context.Set<ClientTree>().Where(e => e.DMDGroup.Equals(DMDGroup) && e.EndDate == null).FirstOrDefault().GHierarchyCode;

            while (String.IsNullOrEmpty(gHierarhyCode))
            {
                objectId = context.Set<ClientTree>().Where(e => e.ObjectId.Equals(objectId.ToString()) && e.EndDate == null).FirstOrDefault().parentId;
                gHierarhyCode = context.Set<ClientTree>().Where(e => e.ObjectId.Equals(objectId.ToString()) && e.EndDate == null).FirstOrDefault().GHierarchyCode;
                if (objectId == 5000000)
                    break;
            }
            
            return gHierarhyCode;
        }

        private string GetFmtStringDate(DateTimeOffset date)
        {
            var fmt = "00";
            var yearFmt = "0000";
            return date.Year.ToString(yearFmt) + date.Month.ToString(fmt) + date.Day.ToString(fmt) + " " +
                   date.Hour.ToString(fmt) + ":" + date.Minute.ToString(fmt) + ":" + date.Second.ToString(fmt);
        }
    }
}