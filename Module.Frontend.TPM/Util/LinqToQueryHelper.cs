using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.OData.Query;

namespace Module.Frontend.TPM.Util
{
    static class LinqToQueryHelper
    {
        public static string BuildQueryString(ODataRawQueryOptions filters)
        {
            string result = "?";
            if (filters.Expand != null)
            {
                result += ("$expand=" + filters.Expand);
            }
            if (filters.Filter != null)
            {
                result += ("$filter=" + filters.Filter);
            }
            if (filters.OrderBy != null)
            {
                result += ("$orderby=" + filters.OrderBy);
            }
            if (filters.Select != null)
            {
                result += ("$select=" + filters.Select);
            }
            if (result != "?")
            {
                return result;
            }
            else
            {
                return "";
            }
        }
    }
}
