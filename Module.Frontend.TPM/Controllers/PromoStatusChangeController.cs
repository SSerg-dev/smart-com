using AutoMapper;
using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{
    public class PromoStatusChangesController : EFContextController
    {
        private readonly IAuthorizationManager authorizationManager;

        public PromoStatusChangesController(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        protected IQueryable<PromoStatusChange> GetConstraintedQuery()
        {
            UserInfo user = authorizationManager.GetCurrentUser();
            string role = authorizationManager.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();
            IQueryable<PromoStatusChange> query = Context.Set<PromoStatusChange>();
            return query;
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IHttpActionResult PromoStatusChangesByPromo(String promoKey)
        {
            try
            {
                // Цвета для статусов
                Dictionary<string, string> statusColors = new Dictionary<string, string>();
                var promoStatuses = Context.Set<PromoStatus>().Where(x => !x.Disabled);
                foreach (PromoStatus promoStatus in promoStatuses)
                {
                    if (promoStatus.Color == null) promoStatus.Color = "#ffffff";
                    statusColors.Add(promoStatus.SystemName, promoStatus.Color);
                }

                Guid promoId = Guid.Empty;
                bool isGuid = Guid.TryParse(promoKey, out promoId);
                if (isGuid)
                {

                    Promo promoModel = Context.Set<Promo>().Where(x => x.Id == promoId).First();
                    Context.Configuration.LazyLoadingEnabled = false;
                    List<PromoStatusChange> pscsList = Context.Set<PromoStatusChange>()
                        .AsNoTracking()
                        .Include(g => g.Promo.PromoStatus)
                        .Where(e => e.PromoId == promoId)
                        .OrderByDescending(y => y.Date).ToList();
                    Context.Configuration.LazyLoadingEnabled = true;
                    foreach (var item in pscsList)
                    {
                        var user = Context.Set<User>().FirstOrDefault(x => x.Id == item.UserId);
                        if (user != null)
                        {
                            item.UserName = user.Name;
                        }
                        var role = Context.Set<Role>().FirstOrDefault(x => x.Id == item.RoleId);
                        if (role != null)
                        {
                            item.RoleName = role.DisplayName;
                        }
                        var status = item.PromoStatus;
                        if (status != null)
                        {
                            item.StatusColor = status.Color;
                            item.StatusName = status.Name;
                        }
                    }
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<PromoStatusChange, PromoStatusChange>();
                        //.ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                        //.ForMember(pTo => pTo.PromoStatus, opt => opt.Ignore())
                        //.ForMember(pTo => pTo.RejectReason, opt => opt.Ignore());
                        cfg.CreateMap<Promo, Promo>()
                            .ForMember(pTo => pTo.BTLPromoes, opt => opt.Ignore())
                            .ForMember(pTo => pTo.Brand, opt => opt.Ignore())
                            .ForMember(pTo => pTo.Technology, opt => opt.Ignore())
                            .ForMember(pTo => pTo.BrandTech, opt => opt.Ignore())
                            .ForMember(pTo => pTo.ClientTree, opt => opt.Ignore())
                            //.ForMember(pTo => pTo.PromoStatus, opt => opt.Ignore())
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
                            .ForMember(pTo => pTo.PromoUpliftFailIncidents, opt => opt.Ignore())
                            .ForMember(pTo => pTo.PromoSupportPromoes, opt => opt.Ignore())
                            .ForMember(pTo => pTo.PromoStatusChanges, opt => opt.Ignore())
                            .ForMember(pTo => pTo.PromoProductTrees, opt => opt.Ignore())
                            .ForMember(pTo => pTo.PreviousDayIncrementals, opt => opt.Ignore())
                            .ForMember(pTo => pTo.IncrementalPromoes, opt => opt.Condition(c => c.InOut == false))
                            .ForMember(pTo => pTo.PromoProducts, opt => opt.Ignore())
                            .ForMember(pTo => pTo.Promoes, opt => opt.Ignore());
                        cfg.CreateMap<PromoStatus, PromoStatus>()
                            .ForMember(pTo => pTo.PromoStatusChanges, opt => opt.Ignore());
                    });
                    var mapper = config.CreateMapper();
                    var pscsListMap = mapper.Map<List<PromoStatusChange>>(pscsList);
                    if (promoModel.PromoStatus.SystemName == "OnApproval")
                    {
                        bool isNoNegoPassed = CheckNoNego(promoModel);
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, isEmpty = false, data = pscsListMap, statusColors, isNoNegoPassed }, new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            MaxDepth = 1,
                            //Error = (object sender, ErrorEventArgs args) =>
                            //{
                            //    throw new Exception(String.Format("Parse error: {0}", args.ErrorContext.Error.Message));
                            //},
                        }));
                    }
                    else
                    {
                        return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, isEmpty = false, data = pscsListMap, statusColors }, new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            MaxDepth = 1,
                            //Error = (object sender, ErrorEventArgs args) =>
                            //{
                            //    throw new Exception(String.Format("Parse error: {0}", args.ErrorContext.Error.Message));
                            //},
                        }));
                    }
                }
                else
                {
                    return Content(HttpStatusCode.OK, JsonConvert.SerializeObject(new { success = true, isEmpty = true, statusColors }, new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        //Error = (object sender, ErrorEventArgs args) =>
                        //{
                        //    throw new Exception(String.Format("Parse error: {0}", args.ErrorContext.Error.Message));
                        //},
                    }));
                }

            }
            catch (DbUpdateException e)
            {
                return InternalServerError(new Exception(e.Message));
            }
        }

        private bool CheckNoNego(Promo model)
        {
            List<NoneNego> noNegoList = Context.Set<NoneNego>().Where(x => !x.Disabled && x.FromDate <= model.StartDate && x.ToDate >= model.EndDate).ToList();

            ClientTreeHierarchyView clientTreeHierarchy = Context.Set<ClientTreeHierarchyView>().FirstOrDefault(x => x.Id == model.ClientTreeId);

            // может быть выбрано несколько продуктов (subrange) в промо
            int[] productObjectIds = Context.Set<PromoProductTree>().Where(n => n.PromoId == model.Id && !n.Disabled).Select(n => n.ProductTreeObjectId).ToArray();
            ProductTreeHierarchyView[] productTreeHierarchies = Context.Set<ProductTreeHierarchyView>().Where(x => productObjectIds.Contains(x.Id)).ToArray();

            foreach (ProductTreeHierarchyView prodHierarchy in productTreeHierarchies)
            {
                bool resultForProduct = false;
                string productHierarchy = prodHierarchy.Hierarchy + "." + prodHierarchy.Id.ToString();
                int[] productHierarchyArr = Array.ConvertAll(productHierarchy.Split('.'), int.Parse);

                for (int i = (productHierarchyArr.Length - 1); i > 0 && !resultForProduct; i--)
                {
                    string clientHierarchy = clientTreeHierarchy.Hierarchy + "." + model.ClientTreeId.ToString();
                    int[] clientHierarchyArr = Array.ConvertAll(clientHierarchy.Split('.'), int.Parse);

                    for (int j = (clientHierarchyArr.Length - 1); j > 0 && !resultForProduct; j--)
                    {
                        List<NoneNego> noNegoForClientList = noNegoList.Where(x => x.ClientTree.ObjectId == clientHierarchyArr[j]).ToList();
                        foreach (NoneNego noNego in noNegoForClientList)
                        {
                            if (noNego.ProductTree.ObjectId == productHierarchyArr[i])
                            {
                                if (noNego.Mechanic != null && model.MarsMechanic != null && noNego.Mechanic.SystemName == model.MarsMechanic.SystemName)
                                {
                                    if (noNego.Discount >= model.MarsMechanicDiscount)
                                    {
                                        resultForProduct = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                // если хоть один subrange не прошел проверку, то отклоняем
                if (!resultForProduct)
                    return false;
            }

            return true;
        }

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<PromoStatusChange>().Count(e => e.Id == key) > 0;
        }
    }
}
