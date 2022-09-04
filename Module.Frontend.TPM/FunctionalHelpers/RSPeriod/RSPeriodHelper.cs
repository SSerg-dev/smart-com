using AutoMapper;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Module.Frontend.TPM.FunctionalHelpers.RSPeriod
{
    public static class RSPeriodHelper
    {
        public static StartEndModel GetRSPeriod(DatabaseContext Context)
        {
            string weeks = Context.Set<Setting>().Where(g => g.Name == "RS_START_WEEKS").FirstOrDefault().Value;
            DateTimeOffset today = DateTimeOffset.Now;
            DateTimeOffset endDate = new DateTimeOffset(today.Year, 12, 31, 23, 0, 0, new TimeSpan(0, 0, 0));
            StartEndModel startEndModel = new StartEndModel
            {
                EndDate = endDate
            };

            if (Int32.TryParse(weeks, out int intweeks))
            {

                DateTimeOffset RsStartDate = today.AddDays(intweeks * 7);
                startEndModel.StartDate = RsStartDate;

                return startEndModel;
            }
            else
            {
                startEndModel.StartDate = DateTimeOffset.MinValue;
                return startEndModel;
            }
        }
        public static void CreateRSPeriod(Promo promo, DatabaseContext Context)
        {
            RollingScenario rollingScenarioExist = Context.Set<RollingScenario>()
                .Include(g => g.Promoes)
                .Include(g => g.PromoStatus)
                .FirstOrDefault(g => g.ClientTreeId == promo.ClientTreeKeyId && !g.Disabled && g.PromoStatus.SystemName != StateNames.APPROVED);

            List<PromoStatus> promoStatuses = Context.Set<PromoStatus>().Where(g => !g.Disabled).ToList();
            StartEndModel startEndModel = GetRSPeriod(Context);
            RollingScenario rollingScenario = new RollingScenario();
            if (rollingScenarioExist == null)
            {
                ClientTree client = Context.Set<ClientTree>().FirstOrDefault(g => g.ObjectId == promo.ClientTreeId);
                rollingScenario = new RollingScenario
                {
                    StartDate = startEndModel.StartDate,
                    EndDate = startEndModel.EndDate,
                    PromoStatus = promoStatuses.FirstOrDefault(g => g.SystemName == "Draft"),
                    ClientTree = client,
                    Promoes = new List<Promo>()
                };
                rollingScenario.Promoes.Add(promo);
                Context.Set<RollingScenario>().Add(rollingScenario);
            }
            else
            {
                rollingScenarioExist.Promoes.Add(promo);
            }
            Context.SaveChanges();
        }
        public static void CreateRSPeriod(List<Promo> promoes, DatabaseContext Context)
        {
            foreach (Promo promo in promoes)
            {
                CreateRSPeriod(promo, Context);
            }
        }
        public static void DeleteRSPeriod(Guid rollingScenarioId, DatabaseContext Context)
        {
            RollingScenario rollingScenario = Context.Set<RollingScenario>()
                                            .Include(g => g.PromoStatus)
                                            .Include(g => g.Promoes)
                                            .FirstOrDefault(g => g.Id == rollingScenarioId);
            PromoStatus promoStatusCancelled = Context.Set<PromoStatus>().FirstOrDefault(v => v.SystemName == "Cancelled");
            rollingScenario.IsSendForApproval = false;
            rollingScenario.Disabled = true;
            rollingScenario.DeletedDate = DateTimeOffset.Now;
            rollingScenario.PromoStatus = promoStatusCancelled;
            Context.Set<Promo>().RemoveRange(rollingScenario.Promoes);
            Context.SaveChanges();
        }
        public static void OnApprovalRSPeriod(Guid rollingScenarioId, DatabaseContext Context)
        {
            RollingScenario RS = Context.Set<RollingScenario>()
                    .Include(g => g.PromoStatus)
                    .FirstOrDefault(g => g.Id == rollingScenarioId);
            PromoStatus promoStatusOnApproval = Context.Set<PromoStatus>().FirstOrDefault(v => v.SystemName == "OnApproval");
            RS.IsSendForApproval = true;
            RS.PromoStatus = promoStatusOnApproval;
            DateTimeOffset today = DateTimeOffset.Now;
            DateTimeOffset expirationDate = new DateTimeOffset(today.Year, today.Month, today.Day, 23, 0, 0, new TimeSpan(0, 0, 0));
            RS.ExpirationDate = expirationDate.AddDays(14);
            Context.SaveChanges();
        }
        public static void MassApproveRSPeriod(List<RollingScenario> rollingScenarios, DatabaseContext Context)
        {

        }
        public static void ApproveRSPeriod(Guid rollingScenarioId, DatabaseContext Context)
        {
            RollingScenario RS = Context.Set<RollingScenario>()
                    .Include(g => g.PromoStatus)
                    .Include(g => g.Promoes)
                    .FirstOrDefault(g => g.Id == rollingScenarioId);
            List<Guid> PromoRSIds = RS.Promoes.Select(f => f.Id).ToList();
            if (Context.Set<BlockedPromo>().Any(x => x.Disabled == false && PromoRSIds.Contains(x.PromoId)))
            {
                throw new System.Web.HttpException("there is a blocked Promo");
            }
            PromoStatus promoStatusApproved = Context.Set<PromoStatus>().FirstOrDefault(v => v.SystemName == "Approved");
            RS.IsCMManagerApproved = true;
            RS.PromoStatus = promoStatusApproved;
            CopyBackPromoes(RS.Promoes.ToList(), Context);
            Context.Set<Promo>().RemoveRange(RS.Promoes);
            Context.SaveChanges();
        }

        public static void CopyBackPromoes(List<Promo> promoesRS, DatabaseContext Context)
        {
            var cfgPromoBack = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Promo, Promo>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    .ForMember(pTo => pTo.TPMmode, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.BTLPromoes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Brand, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Technology, opt => opt.Ignore())
                    .ForMember(pTo => pTo.BrandTech, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ClientTree, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoStatus, opt => opt.Ignore())
                    .ForMember(pTo => pTo.MarsMechanic, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PlanInstoreMechanic, opt => opt.Ignore())
                    .ForMember(pTo => pTo.MarsMechanicType, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PlanInstoreMechanicType, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoTypes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Color, opt => opt.Ignore())
                    .ForMember(pTo => pTo.RejectReason, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Event, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualInStoreMechanic, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualInStoreMechanicType, opt => opt.Ignore())
                    .ForMember(pTo => pTo.MasterPromo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.RollingScenarioId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.RollingScenario, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoUpliftFailIncidents, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoSupportPromoes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoStatusChanges, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductTrees, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PreviousDayIncrementals, opt => opt.Ignore())
                    .ForMember(pTo => pTo.IncrementalPromoes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProducts, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoOnApprovalIncidents, opt => opt.Ignore())
                    .ForMember(pTo => pTo.CurrentDayIncrementals, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promoes, opt => opt.Ignore());
            });
            var mapperPromoBack = cfgPromoBack.CreateMapper();
            //var cfgBTLPromoBack = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<BTLPromo, BTLPromo>()
            //        .ForMember(pTo => pTo.Id, opt => opt.Ignore())
            //        .ForMember(pTo => pTo.TPMmode, opt => opt.Ignore())
            //        //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
            //        //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
            //        .ForMember(pTo => pTo.BTL, opt => opt.Ignore())
            //        .ForMember(pTo => pTo.ClientTree, opt => opt.Ignore())
            //        .ForMember(pTo => pTo.PromoId, opt => opt.Ignore())
            //        .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
            //});
            //var mapperBTLPromoBack = cfgBTLPromoBack.CreateMapper();
            var cfgPromoSupportPromoBack = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PromoSupportPromo, PromoSupportPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    .ForMember(pTo => pTo.TPMmode, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoSupport, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
            });
            var mapperPromoSupportPromoBack = cfgPromoSupportPromoBack.CreateMapper();
            var cfgPromoProductTreeBack = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PromoProductTree, PromoProductTree>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    .ForMember(pTo => pTo.TPMmode, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
            });
            var mapperPromoProductTreeBack = cfgPromoProductTreeBack.CreateMapper();
            var cfgIncrementalPromoBack = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IncrementalPromo, IncrementalPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    .ForMember(pTo => pTo.TPMmode, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore());
            });
            var mapperIncrementalPromoBack = cfgIncrementalPromoBack.CreateMapper();
            var cfgPromoProductBack = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PromoProduct, PromoProduct>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    .ForMember(pTo => pTo.TPMmode, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ProductId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductsCorrections, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Plu, opt => opt.Ignore());
            });
            var mapperPromoProductBack = cfgPromoProductBack.CreateMapper();
            var cfgPromoProductsCorrectionBack = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PromoProductsCorrection, PromoProductsCorrection>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    .ForMember(pTo => pTo.TPMmode, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore());
            });
            var mapperPromoProductsCorrectionBack = cfgPromoProductsCorrectionBack.CreateMapper();

            Guid promoStatusOnApproval = Context.Set<PromoStatus>().FirstOrDefault(g => g.SystemName == StateNames.ON_APPROVAL).Id;
            List<Guid> promoRSids = promoesRS.Select(h => h.Id).ToList();
            promoesRS = Context.Set<Promo>()
                    //.Include(g => g.BTLPromoes)
                    .Include(g => g.PromoSupportPromoes)
                    .Include(g => g.PromoProductTrees)
                    .Include(g => g.IncrementalPromoes)
                    .Include(g => g.PreviousDayIncrementals)
                    .Include(g => g.PromoStatusChanges)
                    .Include(g => g.PromoUpliftFailIncidents)
                    .Include(g => g.PromoOnApprovalIncidents)
                    .Include(g => g.PromoOnRejectIncidents)
                    .Include(g => g.PromoCancelledIncidents)
                    .Include(g => g.PromoApprovedIncidents)
                    .Include(g => g.CurrentDayIncrementals)
                    .Include(x => x.PromoProducts.Select(y => y.PromoProductsCorrections))
                .Where(x => promoRSids.Contains(x.Id)).ToList();

            List<int> promoRSnumbers = promoesRS.Select(h => h.Number).Cast<int>().ToList();
            List<Promo> promos = Context.Set<Promo>()
                    //.Include(g => g.BTLPromoes)
                    .Include(g => g.PromoSupportPromoes)
                    .Include(g => g.PromoProductTrees)
                    .Include(g => g.IncrementalPromoes)
                    .Include(x => x.PromoProducts.Select(y => y.PromoProductsCorrections))
                .Where(x => promoRSnumbers.Contains((int)x.Number) && x.TPMmode == TPMmode.Current).ToList();
            List<int> numbers = promos.Select(g => g.Number).Cast<int>().ToList();
            foreach (Promo promoRS in promoesRS.ToList())
            {
                if (numbers.Contains((int)promoRS.Number)) // существующий
                {
                    Promo promo = promos.FirstOrDefault(g => g.Number == promoRS.Number);
                    mapperPromoBack.Map(promoRS, promo);
                    //foreach (BTLPromo bTLPromoRS in promoRS.BTLPromoes)
                    //{
                    //    if (promo.BTLPromoes.Select(g => g.BTLId).Contains(bTLPromoRS.BTLId)) // существующий btlpromo
                    //    {
                    //        BTLPromo bTLPromo = promo.BTLPromoes.FirstOrDefault(g => g.BTLId == bTLPromoRS.BTLId);
                    //        mapperBTLPromoBack.Map(bTLPromoRS, bTLPromo);

                    //    }
                    //    else // новый btlpromo
                    //    {
                    //        bTLPromoRS.PromoId = promo.Id;
                    //        bTLPromoRS.Promo = promo;
                    //        bTLPromoRS.TPMmode = TPMmode.Current;
                    //    }
                    //}
                    foreach (PromoSupportPromo promoSupportPromoRS in promoRS.PromoSupportPromoes)
                    {
                        if (promo.PromoSupportPromoes.Select(g => g.PromoSupportId).Contains(promoSupportPromoRS.PromoSupportId)) // существующий PromoSupportPromo
                        {
                            PromoSupportPromo promoSupportPromo = promo.PromoSupportPromoes.FirstOrDefault(g => g.PromoSupportId == promoSupportPromoRS.PromoSupportId);
                            mapperPromoSupportPromoBack.Map(promoSupportPromoRS, promoSupportPromo);

                        }
                        else // новый PromoSupportPromo
                        {
                            promoSupportPromoRS.PromoId = promo.Id;
                            promoSupportPromoRS.Promo = promo;
                            promoSupportPromoRS.TPMmode = TPMmode.Current;
                        }
                    }
                    foreach (IncrementalPromo incrementalPromoRS in promoRS.IncrementalPromoes)
                    {
                        if (promo.IncrementalPromoes.Select(g => g.ProductId).Contains(incrementalPromoRS.ProductId)) // существующий IncrementalPromo
                        {
                            IncrementalPromo incrementalPromo = promo.IncrementalPromoes.FirstOrDefault(g => g.ProductId == incrementalPromoRS.ProductId);
                            mapperIncrementalPromoBack.Map(incrementalPromoRS, incrementalPromo);

                        }
                        else // новый IncrementalPromo
                        {
                            incrementalPromoRS.PromoId = promo.Id;
                            incrementalPromoRS.Promo = promo;
                            incrementalPromoRS.TPMmode = TPMmode.Current;
                        }
                    }
                    foreach (PromoProductTree promoProductTreeRS in promoRS.PromoProductTrees)
                    {
                        if (promo.PromoProductTrees.Select(g => g.ProductTreeObjectId).Contains(promoProductTreeRS.ProductTreeObjectId)) // существующий PromoProductTree
                        {
                            PromoProductTree promoProductTree = promo.PromoProductTrees.FirstOrDefault(g => g.ProductTreeObjectId == promoProductTreeRS.ProductTreeObjectId);
                            mapperPromoProductTreeBack.Map(promoProductTreeRS, promoProductTree);

                        }
                        else // новый PromoProductTree
                        {
                            promoProductTreeRS.PromoId = promo.Id;
                            promoProductTreeRS.Promo = promo;
                            promoProductTreeRS.TPMmode = TPMmode.Current;
                        }
                    }
                    foreach (PromoProduct promoProductRS in promoRS.PromoProducts.ToList())
                    {
                        if (promo.PromoProducts.Select(g => g.ProductId).Contains(promoProductRS.ProductId)) // существующий PromoProduct
                        {
                            PromoProduct promoProduct = promo.PromoProducts.FirstOrDefault(g => g.ProductId == promoProductRS.ProductId);
                            foreach (PromoProductsCorrection promoProductsCorrectionRS in promoProductRS.PromoProductsCorrections.ToList())
                            {
                                PromoProductsCorrection promoProductsCorrection = promoProduct.PromoProductsCorrections.FirstOrDefault();
                                mapperPromoProductsCorrectionBack.Map(promoProductsCorrectionRS, promoProductsCorrection);
                                //promoProductRS.PromoProductsCorrections.Remove(promoProductsCorrectionRS);
                                Context.Set<PromoProductsCorrection>().Remove(promoProductsCorrectionRS);
                            }
                            mapperPromoProductBack.Map(promoProductRS, promoProduct);
                            //promoRS.PromoProducts.Remove(promoProductRS);
                            Context.Set<PromoProduct>().Remove(promoProductRS);
                        }
                        else // новый PromoProduct
                        {
                            promoProductRS.PromoId = promo.Id;
                            promoProductRS.Promo = promo;
                            promoProductRS.TPMmode = TPMmode.Current;
                        }
                    }
                    promo.PromoStatusId = promoStatusOnApproval;
                    //promoesRS.Remove(promoRS); - нельзя сделать
                    Context.Set<Promo>().Remove(promoRS); // не отследит EF
                    //ChangeStatusOnApproval(Context, promo);
                }
                else // новый
                {
                    promoRS.TPMmode = TPMmode.Current;
                    promoRS.RollingScenario = null; // unlink
                    foreach (var item in promoRS.IncrementalPromoes)
                    {
                        item.TPMmode = TPMmode.Current;
                    }
                    foreach (var item in promoRS.PromoSupportPromoes)
                    {
                        item.TPMmode = TPMmode.Current;
                    }
                    foreach (var item in promoRS.PromoProductTrees)
                    {
                        item.TPMmode = TPMmode.Current;
                    }
                    foreach (var item in promoRS.PromoProducts)
                    {
                        item.TPMmode = TPMmode.Current;
                        foreach (var item2 in item.PromoProductsCorrections)
                        {
                            item2.TPMmode = TPMmode.Current;
                        }
                    }
                    promoRS.PromoStatusId = promoStatusOnApproval;
                    //ChangeStatusOnApproval(Context, promoRS);
                }



                // Context.SaveChanges(); //удалить
            }

            Context.SaveChanges();
        }
        public static void ChangeStatusOnApproval(DatabaseContext context, Promo promo)
        {
            using (PromoStateContext promoStateContext = new PromoStateContext(context, promo))
            {
                var status = promoStateContext.ChangeState(promo, PromoStates.OnApproval, "System", out string message);
                if (status)
                {
                    //Сохранение изменения статуса
                    var promoStatusChange = context.Set<PromoStatusChange>().Create<PromoStatusChange>();
                    promoStatusChange.PromoId = promo.Id;
                    promoStatusChange.StatusId = promo.PromoStatusId;
                    promoStatusChange.Date = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).Value;

                    context.Set<PromoStatusChange>().Add(promoStatusChange);
                }
            }
        }
    }
}
