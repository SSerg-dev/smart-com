using Core.History;
using Module.Persist.TPM.Model.History;
using Ninject;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{
	public class HistoricalPLUDictionariesController : ODataController
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
        public IQueryable<HistoricalPLUDictionary> GetHistoricalPLUDictionaries(Guid? Id)
        {
            return HistoryReader.GetAllById<HistoricalPLUDictionary>(Id.ToString());
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<HistoricalPLUDictionary> GetFilteredData(ODataQueryOptions<HistoricalPLUDictionary> options)
        {
            //var query = Enumerable.Empty<HistoricalAssortmentMatrix>().AsQueryable();
            //string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);

            //bool isArgumentExists = Helper.IsValueExists(bodyText, "Id");
            //if (isArgumentExists)
            //{
            //    Guid? id = Helper.GetValueIfExists<Guid?>(bodyText, "Id");
            //    query = HistoryReader.GetAllById<HistoricalAssortmentMatrix>(id.ToString());
            //}
            //else
            //{
            //    query = HistoryReader.GetAll<HistoricalAssortmentMatrix>();
            //}

            //var querySettings = new ODataQuerySettings
            //{
            //    EnsureStableOrdering = false,
            //    HandleNullPropagation = HandleNullPropagationOption.False,
            //    EnableConstantParameterization = false,
            //};

            //var optionsPost = new ODataQueryOptionsPost<HistoricalAssortmentMatrix>(options.Context, Request, HttpContext.Current.Request);
            //return optionsPost.ApplyTo(query, querySettings) as IQueryable<HistoricalAssortmentMatrix>;
            return null;
        }
    }
}
