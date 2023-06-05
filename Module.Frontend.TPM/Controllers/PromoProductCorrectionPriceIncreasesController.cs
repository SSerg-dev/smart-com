using Core.Security;
using Core.Security.Models;
using Frontend.Core.Controllers.Base;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{
    public class PromoProductCorrectionPriceIncreasesController : EFContextController
    {
        private readonly UserInfo user;
        private readonly Role role;
        private readonly Guid? roleId;
        private readonly IAuthorizationManager authorizationManager1;

        public PromoProductCorrectionPriceIncreasesController(IAuthorizationManager authorizationManager)
        {
            authorizationManager1 = authorizationManager;
            user = authorizationManager.GetCurrentUser();
            var roleInfo = authorizationManager.GetCurrentRole();
            role = new Role { Id = roleInfo.Id.Value, SystemName = roleInfo.SystemName };
            roleId = role.Id;
        }

        protected IQueryable<PromoProduct> GetConstraintedQuery(ODataQueryOptions<PromoProductPriceIncrease> options)
        {
            UserInfo user = authorizationManager1.GetCurrentUser();
            string role = authorizationManager1.GetCurrentRoleName();
            IList<Constraint> constraints = user.Id.HasValue ? Context.Constraints
                .Where(x => x.UserRole.UserId.Equals(user.Id.Value) && x.UserRole.Role.SystemName.Equals(role))
                .ToList() : new List<Constraint>();

            IQueryable<PromoProduct> query = Context.Set<PromoProduct>().Where(e => !e.Disabled).FixOdataExpand(options);
            return query;
        }

        [ClaimsAuthorize]
        [EnableQuery(MaxNodeCount = int.MaxValue, MaxExpansionDepth = 3)]
        public IQueryable<PromoProduct> GetPromoProductCorrectionPriceIncreases(ODataQueryOptions<PromoProductPriceIncrease> options)
        {
            return GetConstraintedQuery(options);
        }

        [ClaimsAuthorize]
        public async Task<IHttpActionResult> Post(PromoProductCorrectionPriceIncrease model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //если существует коррекция на данный PromoProduct, то не создаем новый объект
            var item = Context.Set<PromoProductCorrectionPriceIncrease>()
                .Include(g => g.PromoProductPriceIncrease.PromoPriceIncrease.Promo)
                .FirstOrDefault(x => x.PromoProductPriceIncreaseId == model.PromoProductPriceIncreaseId); //через UserId передается Id, так как для ext js store.sync() нельзя менять Id

            if (item != null)
            {
                if (item.Disabled)
                {
                    item.DeletedDate = null;
                    item.Disabled = false;
                }
                if (item.CreateDate == null)
                {
                    item.CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                }
                item.PlanProductUpliftPercentCorrected = model.PlanProductUpliftPercentCorrected;
                item.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                item.UserId = user.Id;
                item.UserName = user.Login;
                if (item.PromoProductPriceIncrease.PromoPriceIncrease.Promo.NeedRecountUpliftPI == true)
                {
                    return InternalServerError(new Exception("Promo Locked Update"));
                }


                try
                {
                    var saveChangesResult = await Context.SaveChangesAsync();
                    if (saveChangesResult > 0)
                    {
                        CreateChangesIncident(Context.Set<ChangesIncident>(), item);
                        await Context.SaveChangesAsync();
                    }
                }
                catch (Exception e)
                {
                    return GetErorrRequest(e);
                }

                return Created(model);
            }
            else
            {
                model.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                model.UserId = user.Id;
                model.UserName = user.Login;
                model.CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                Context.Set<PromoProductCorrectionPriceIncrease>().Add(model);
                try
                {
                    var saveChangesResult = await Context.SaveChangesAsync();
                    if (saveChangesResult > 0)
                    {
                        CreateChangesIncident(Context.Set<ChangesIncident>(), model);
                        await Context.SaveChangesAsync();
                    }
                }
                catch (Exception e)
                {
                    return GetErorrRequest(e);
                }

                return Created(model);
            }
        }
        public static void CreateChangesIncident(DbSet<ChangesIncident> changesIncidents, PromoProductCorrectionPriceIncrease promoProductCorrection)
        {
            changesIncidents.Add(new ChangesIncident
            {
                Id = Guid.NewGuid(),
                DirectoryName = nameof(PromoProductCorrectionPriceIncrease),
                ItemId = promoProductCorrection.Id.ToString(),
                CreateDate = DateTimeOffset.Now,
                ProcessDate = null,
                DeletedDate = null,
                Disabled = false
            });
        }

        private bool EntityExists(System.Guid key)
        {
            return Context.Set<PromoProductsCorrection>().Count(e => e.Id == key) > 0;
        }

        private ExceptionResult GetErorrRequest(Exception e)
        {
            // обработка при создании дублирующей записи
            SqlException exc = e.GetBaseException() as SqlException;

            if (exc != null && (exc.Number == 2627 || exc.Number == 2601))
            {
                return InternalServerError(new Exception("This PromoProductCorrection has already existed"));
            }
            else
            {
                return InternalServerError(GetExceptionMessage.GetInnerException(e));
            }
        }
    }
}
