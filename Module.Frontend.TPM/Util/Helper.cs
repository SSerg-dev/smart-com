using Core.Dependency;
using Core.KeyVault;
using Core.Settings;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Microsoft.Ajax.Utilities;
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
using CoreUtil = Frontend.Core.Util;

namespace Module.Frontend.TPM.Util
{
    public static class Helper
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

        static public List<ClientTree> getClientTreeAllChildrenWithoutContext(int parentId, List<ClientTree> clientTrees, bool stopAtDMDGroup = false)
        {
            List<ClientTree> tempClientTrees;
            if (stopAtDMDGroup)
            {
                tempClientTrees = clientTrees.Where(x => !x.EndDate.HasValue && x.parentId == parentId && x.DMDGroup.IsNullOrWhiteSpace()).ToList();
            }
            else
            {
                tempClientTrees = clientTrees.Where(x => !x.EndDate.HasValue && x.parentId == parentId).ToList();
            }
            var resClientTrees = new List<ClientTree>(tempClientTrees);
            foreach (var clientTree in tempClientTrees)
            {
                resClientTrees.AddRange(getClientTreeAllChildrenWithoutContext(clientTree.ObjectId, clientTrees));
            }
            return resClientTrees;
        }

        public static T GetValueIfExists<T>(string jsonText, string propertyName)
        {
            return CoreUtil.Helper.GetValueIfExists<T>(jsonText, propertyName);
        }

        public static bool IsValueExists(string jsonText, string propertyName)
        {
            return CoreUtil.Helper.IsValueExists(jsonText, propertyName);
        }

        public static string GetRequestBody(HttpRequest request)
        {
            return CoreUtil.Helper.GetRequestBody(request);
        }

        public static string GetSecretSetting(string settingName, string defaultValue)
        {
            var keyVault = KeyStorageManager.GetKeyVault();
            var secretName = AppSettingsManager.GetSetting(settingName, defaultValue);
            return keyVault.GetSecret(secretName, "");
        }
    }
}
