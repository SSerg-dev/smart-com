using Core.History;
using Frontend.Core.Controllers.Base;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.History;
using Module.Persist.TPM.Model.TPM;
using Ninject;
using Persist;
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
    public class HistoricalBTLsController : EFContextController
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
        public IQueryable<HistoricalBTL> GetHistoricalBTLs()
        {
            return HistoryReader.GetAll<HistoricalBTL>();
        }

        [ClaimsAuthorize]
        [EnableQuery(
            MaxNodeCount = int.MaxValue,
            EnsureStableOrdering = false,
            HandleNullPropagation = HandleNullPropagationOption.False,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = false,
            MaxTop = 1024)]
        public IQueryable<HistoricalBTL> GetHistoricalBTLs(Guid? Id)
        {
            var recordNumber = Context.Set<BTL>().Where(e => e.Id == Id).Select(e => e.Number).FirstOrDefault();
            
            var history = HistoryReader.GetAllById<HistoricalBTL>(Id.ToString());
            history.ToList().ForEach(e => e.Number = recordNumber);
            
            return history;
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<HistoricalBTL> GetFilteredData(ODataQueryOptions<HistoricalBTL> options)
        {
            var query = Enumerable.Empty<HistoricalBTL>().AsQueryable();
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);

            bool isArgumentExists = Helper.IsValueExists(bodyText, "Id");
            if (isArgumentExists)
            {
                Guid? Id = Helper.GetValueIfExists<Guid?>(bodyText, "Id");
                var recordNumber = Context.Set<BTL>().Where(e => e.Id == Id).Select(e => e.Number).FirstOrDefault();

                query = HistoryReader.GetAllById<HistoricalBTL>(Id.ToString());
                query.ToList().ForEach(e => e.Number = recordNumber);
            }
            else
            {
                query = HistoryReader.GetAll<HistoricalBTL>();
            }

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False,
                EnableConstantParameterization = false,
            };

            var optionsPost = new ODataQueryOptionsPost<HistoricalBTL>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<HistoricalBTL>;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                HistoryReader.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
