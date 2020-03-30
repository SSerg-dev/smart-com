using Core.History;
using Frontend.Core.Controllers.Base;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.History;
using Module.Persist.TPM.Model.TPM;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{
    public class HistoricalPromoSupportsController : EFContextController
    {
        [Inject]
        public IHistoryReader HistoryReader { get; set; }

        [ClaimsAuthorize]
        [EnableQuery(
            MaxNodeCount = int.MaxValue,
            EnsureStableOrdering = false,
            HandleNullPropagation = HandleNullPropagationOption.False,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = false,
            MaxTop = 1024)]
        public IQueryable<HistoricalPromoSupport> GetHistoricalPromoSupports(Guid? Id)
        {
            var recordNumber = Context.Set<PromoSupport>().Where(e => e.Id == Id).Select(e => e.Number).FirstOrDefault();

            var history = HistoryReader.GetAllById<HistoricalPromoSupport>(Id.ToString());
            history.ToList().ForEach(e => e.Number = recordNumber);

            return history;
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<HistoricalPromoSupport> GetFilteredData(ODataQueryOptions<HistoricalPromoSupport> options)
        {
            var query = Enumerable.Empty<HistoricalPromoSupport>().AsQueryable();
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);

            bool isArgumentExists = Helper.IsValueExists(bodyText, "Id");
            if (isArgumentExists)
            {
                Guid? Id = Helper.GetValueIfExists<Guid?>(bodyText, "Id");
                var recordNumber = Context.Set<PromoSupport>().Where(e => e.Id == Id).Select(e => e.Number).FirstOrDefault();

                query = HistoryReader.GetAllById<HistoricalPromoSupport>(Id.ToString());
                query.ToList().ForEach(e => e.Number = recordNumber);
            }

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False,
                EnableConstantParameterization = false,
            };

            var optionsPost = new ODataQueryOptionsPost<HistoricalPromoSupport>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<HistoricalPromoSupport>;
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                HistoryReader.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}