using Core.Dependency;
using Core.Settings;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Newtonsoft.Json.Linq;
using Persist;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.OData;

namespace Module.Frontend.TPM.Util
{
    static class Helper
    {
        static public List<ClientTree> getClientTreeAllChildren(int parentId, DatabaseContext Context)
        {
            var tempClientTrees = Context.Set<ClientTree>().Where(x => !x.EndDate.HasValue && x.parentId == parentId).ToList();
            var resClientTrees = new List<ClientTree>(tempClientTrees);
            foreach (var clientTree in tempClientTrees)
            {
                resClientTrees.AddRange(getClientTreeAllChildren(clientTree.ObjectId, Context));
            }
            return resClientTrees;
        }

        public static T GetValueIfExists<T>(string jsonText, string propertyName)
        {
            try
            {
                if (string.IsNullOrEmpty(jsonText) || string.IsNullOrEmpty(propertyName)) return default(T);
                JObject jObject = JObject.Parse(jsonText);
                dynamic dynamicObject = jObject;
                JToken jToken;
                if (jObject.TryGetValue(propertyName, out jToken))
                {
                    return (T)dynamicObject[propertyName];
                }

                return default(T);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static bool IsValueExists(string jsonText, string propertyName)
        {
            if (string.IsNullOrEmpty(jsonText) || string.IsNullOrEmpty(propertyName)) return false;
            JObject jObject = JObject.Parse(jsonText);
            JToken jToken;
            return jObject.TryGetValue(propertyName, out jToken);
        }

        public static string GetRequestBody(HttpRequest request)
        {
            var bodyStream = new StreamReader(request.InputStream);
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            return bodyStream.ReadToEnd();
        }
    }
}
