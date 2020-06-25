using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Core.Dependency;
using Core.Settings;
using Interfaces.Implementation.Action;
using Looper.Core;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Persist;
using Utility.LogWriter;

namespace Module.Host.TPM.Actions
{
    public class PriceListMergeAction : BaseAction
    {
        private readonly DatabaseContext _databaseContext;
        private readonly FileLogWriter _fileLogWriter;
        private readonly ExecuteData _executeData;

        public PriceListMergeAction(DatabaseContext databaseContext, FileLogWriter fileLogWriter, ExecuteData executeData)
        {
            _databaseContext = databaseContext;
            _fileLogWriter = fileLogWriter;
            _executeData = executeData;
        }

        public override void Execute()
        {
            var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
            var dateSeparator = settingsManager.GetSetting<string>("PRICELIST_DATE_SEPARATOR", "2020-03-29 00:00:00.000");

            string dateSeparatorSqlString = $"CAST('{dateSeparator}' AS DATETIME)";
            string sql = $"SELECT * FROM [{nameof(PRICELIST_FDM)}] WHERE ([START_DATE] <= {dateSeparatorSqlString} AND {dateSeparatorSqlString} < [FINISH_DATE]) OR ({dateSeparatorSqlString} < [START_DATE])";

            var priceListFDMs = _databaseContext.Database.SqlQuery<PRICELIST_FDM>(sql);
            var priceLists = _databaseContext.Set<PriceList>().Where(x => !x.Disabled);
            var clientTrees = _databaseContext.Set<ClientTree>().Where(x => !x.EndDate.HasValue);
            var products = _databaseContext.Set<Product>().Where(x => !x.Disabled).Select(x => new { x.Id, x.ZREP }).ToList().Select(x => new Product { Id = x.Id, ZREP = x.ZREP });

            var priceListFDMsMaterialized = priceListFDMs.ToList();
            var priceListsMaterialized = priceLists.ToList();
            var clientTreesMaterialized = clientTrees.ToList();
            var productsMaterialized = products.ToList();

            FDMSync(ref priceListFDMsMaterialized, Convert.ToDateTime(dateSeparator));
            var validAfterClientsCheckPriceListFDMs = CheckClients(priceListFDMsMaterialized, clientTreesMaterialized);
            var validAfterProductsCheckPriceListFDMs =  CheckProducts(priceListFDMsMaterialized, products);
            var validAfterDublicatesCheckPriceListFDMs = CheckDublicates(priceListFDMsMaterialized);
            var validAfterIntersectionsCheckPriceListFDMs = CheckIntersections(priceListFDMsMaterialized);

            var validPriceListFDMs = validAfterClientsCheckPriceListFDMs
                                        .Intersect(validAfterProductsCheckPriceListFDMs)
                                        .Intersect(validAfterDublicatesCheckPriceListFDMs); 
                                        //.Intersect(validAfterIntersectionsCheckPriceListFDMs);

            var differentPriceLists = GetDifferentPriceList(validPriceListFDMs, clientTreesMaterialized, productsMaterialized);
            FillPriceListTable(differentPriceLists, priceListsMaterialized, clientTreesMaterialized, productsMaterialized);
        }

        private void FDMSync(ref List<PRICELIST_FDM> priceListFDMsMaterialized, DateTime dateSeparator)
        {
            priceListFDMsMaterialized.ForEach(p => 
            {
                if (p.START_DATE <= dateSeparator && p.FINISH_DATE > dateSeparator)
                    p.START_DATE = dateSeparator;
            });
        }

        private IEnumerable<PRICELIST_FDM> CheckClients(IEnumerable<PRICELIST_FDM> priceListFDMs, IEnumerable<ClientTree> clientTrees)
        {
            var priceListFDMClientGroups = priceListFDMs.GroupBy(x => new { x.G_HIERARCHY_ID });
            var invalidClientGroups = priceListFDMClientGroups.Where(x => !clientTrees.Any(y => y.GHierarchyCode?.TrimStart('0') == x.Key.G_HIERARCHY_ID));

            foreach (var invalidClientGroup in invalidClientGroups)
            {
                 var logString = $"Invalid client: ({invalidClientGroup.Count()}): \r\n {String.Join("\r\n", invalidClientGroup.Select(x => x.ToString()))}";
                 Errors.Add(logString);
                 _fileLogWriter.Write(true, logString, "Error");
            }

            var validPriceListFDMs = priceListFDMClientGroups.SelectMany(x => x.ToList()).Except(invalidClientGroups.SelectMany(x => x.ToList()));
            return validPriceListFDMs;
        }

        private IEnumerable<PRICELIST_FDM> CheckProducts(IEnumerable<PRICELIST_FDM> priceListFDMs, IEnumerable<Product> products)
        {
            var priceListFDMProductGroups = priceListFDMs.GroupBy(x => new { x.ZREP });
            var invalidProductGroups = priceListFDMProductGroups.Where(x => !products.Any(y => y.ZREP == x.Key.ZREP));

            foreach (var invalidProductGroup in invalidProductGroups)
            {
                 var logString = $"Invalid product: ({invalidProductGroup.Count()}): \r\n {String.Join("\r\n", invalidProductGroup.Select(x => x.ToString()))}";
                 Errors.Add(logString);
                 _fileLogWriter.Write(true, logString, "Error");
            }

            var validPriceListFDMs = priceListFDMProductGroups.SelectMany(x => x.ToList()).Except(invalidProductGroups.SelectMany(x => x.ToList()));
            return validPriceListFDMs;
        }

        private IEnumerable<PRICELIST_FDM> CheckDublicates(IEnumerable<PRICELIST_FDM> priceListFDMs)
        {
            var priceListFDMClientProductGroups = priceListFDMs.GroupBy(x => new { x.G_HIERARCHY_ID, x.ZREP, x.START_DATE, x.FINISH_DATE });
            var dupblicateGroups = priceListFDMClientProductGroups.Where(x => x.Count() > 1);

            foreach (var dublicateGroup in dupblicateGroups)
            {
                if (dublicateGroup.Any())
                {
                    var logString = $"Dublicates ({dublicateGroup.Count()}): \r\n {String.Join("\r\n", dublicateGroup.Select(x => x.ToString()))}";
                    Errors.Add(logString);
                    _fileLogWriter.Write(true, logString, "Error");
                }
            }

            var validPriceListFDMs = priceListFDMClientProductGroups.SelectMany(x => x.ToList()).Except(dupblicateGroups.SelectMany(x => x.ToList()));
            return validPriceListFDMs;
        }

        private IEnumerable<PRICELIST_FDM> CheckIntersections(IEnumerable<PRICELIST_FDM> priceListFDMs)
        {
            var priceListFDMClientProductGroups = priceListFDMs.GroupBy(x => new { x.G_HIERARCHY_ID, x.ZREP });

            var intersectionGroups = priceListFDMClientProductGroups.Where(x => 
                x.Any(y => x.Count(z => !((z.START_DATE >= y.START_DATE && z.START_DATE >= y.FINISH_DATE) || (z.FINISH_DATE <= y.START_DATE && z.FINISH_DATE <= y.FINISH_DATE))) > 1));

            foreach (var intersectionGroup in intersectionGroups)
            {
                if (intersectionGroup.Any())
                {
                    var logString = $"Intersections ({intersectionGroup.Count()}): \r\n {String.Join("\r\n", intersectionGroup.Select(x => x.ToString()))}";
                    Warnings.Add(logString);
                    _fileLogWriter.Write(true, logString, "Warning");
                }
            }

            var validPriceListFDMs = priceListFDMClientProductGroups.SelectMany(x => x.ToList()).Except(intersectionGroups.SelectMany(x => x.ToList()))
                .GroupBy(x => new { x.START_DATE, x.FINISH_DATE }).Select(x => x.OrderByDescending(y => y.START_DATE).FirstOrDefault());

            return validPriceListFDMs;
        }

        private IEnumerable<PriceList> GetDifferentPriceList(IEnumerable<PRICELIST_FDM> priceListFDMs, IEnumerable<ClientTree> clientTrees, IEnumerable<Product> products)
        {
            var currentPriceLists = _databaseContext.Set<PriceList>().ToList();
            var newPriceLists = GetNewPriceListRecords(priceListFDMs, clientTrees, products);
            var differentPriceLists = newPriceLists.Except(currentPriceLists, new PriceListEqualityComparer());
            return differentPriceLists;
        }

        private void FillPriceListTable(IEnumerable<PriceList> differentPriceLists, IEnumerable<PriceList> priceListMaterialized, IEnumerable<ClientTree> clientTrees, IEnumerable<Product> products)
        {
            var priceLists = _databaseContext.Set<PriceList>();

            foreach (var differentPriceList in differentPriceLists)
            {
                var currentPriceLists = priceListMaterialized.Where(x => 
                    x.DeletedDate == differentPriceList.DeletedDate &&
                    x.StartDate == differentPriceList.StartDate &&
                    x.EndDate == differentPriceList.EndDate &&
                    x.ClientTreeId == differentPriceList.ClientTreeId &&
                    x.ProductId == differentPriceList.ProductId);

                foreach (var currentPriceList in currentPriceLists)
                {
                    var currentPriceListFromDatabase = priceLists.FirstOrDefault(x => x.Id == currentPriceList.Id);
                    if (currentPriceListFromDatabase != null)
                    {
                        currentPriceListFromDatabase.Disabled = true;
                        currentPriceListFromDatabase.DeletedDate = DateTimeOffset.Now;
                        CreateIncident(currentPriceList);
                    }
                }
                
                var logLine = $"New {nameof(PriceList)}: {differentPriceList.ToString()}";
                _fileLogWriter.Write(true, logLine, "Message");
                differentPriceList.Product = null;
                differentPriceList.ClientTree = null;
            }

            priceLists.AddRange(differentPriceLists);
            // Необходимо для дальнейшего сохранения ItemId в ChangesIncident
            _databaseContext.SaveChanges();

            
            var incidents = differentPriceLists.Select(d => new ChangesIncident() 
            {
                DirectoryName = nameof(PriceList),
                ItemId = d.Id.ToString(),
                CreateDate = DateTimeOffset.Now,
                Disabled = false
            });
            _databaseContext.Set<ChangesIncident>().AddRange(incidents);
        }

        private void CreateIncident(PriceList priceList)
        {
            _databaseContext.Set<ChangesIncident>().Add(new ChangesIncident
            { 
                DirectoryName = nameof(PriceList),
                ItemId = priceList.Id.ToString(),
                CreateDate = DateTimeOffset.Now,
                Disabled = false
            });
        }

        private IEnumerable<PriceList> GetNewPriceListRecords(IEnumerable<PRICELIST_FDM> priceListFDMs, IEnumerable<ClientTree> clientTrees, IEnumerable<Product> products)
        {
            var newPriceListRecords = new List<PriceList>();

            foreach (var priceListFDM in priceListFDMs)
            {
                var baseClientTrees = GetBaseClientsByPriceListFDM(clientTrees, priceListFDM);
                foreach (var baseClientTree in baseClientTrees)
                {
                    var product = products.FirstOrDefault(x => x.ZREP == priceListFDM.ZREP);
                    if (product != null)
                    {
                        var newPriceListRecord = new PriceList
                        {
                            Disabled = false,
                            DeletedDate = null,
                            StartDate = priceListFDM.START_DATE,
                            EndDate = priceListFDM.FINISH_DATE,
                            Price = priceListFDM.PRICE,
                            ClientTreeId = baseClientTree.Id,
                            ProductId = product.Id,
                            ClientTree = baseClientTree,
                            Product = product
                        };

                        newPriceListRecords.Add(newPriceListRecord);
                    }
                }
            }

            return newPriceListRecords;
        }

        private IEnumerable<ClientTree> GetBaseClientsByPriceListFDM(IEnumerable<ClientTree> clientTrees, PRICELIST_FDM priceListFDM)
        {
            var baseClients = clientTrees.Where(x => x.IsBaseClient);
            var baseClientsForCurrentPriceListFDM = new List<ClientTree>();

            foreach (var baseClient in baseClients)
            {
                var hierarchyForBaseClient = GetHierarhcy(clientTrees, baseClient);
                if (hierarchyForBaseClient.Any(x => !String.IsNullOrEmpty(x.GHierarchyCode) && x.GHierarchyCode.TrimStart('0') == priceListFDM.G_HIERARCHY_ID))
                {
                    baseClientsForCurrentPriceListFDM.Add(baseClient);
                }
            }

            return baseClientsForCurrentPriceListFDM;
        }

        private IEnumerable<ClientTree> GetHierarhcy(IEnumerable<ClientTree> clientTrees, ClientTree node)
        {
            var hierarchy = new List<ClientTree>();
            var currentNode = node;

            while (currentNode != null && currentNode.Type != "root")
            {
                hierarchy.Add(currentNode);
                currentNode = clientTrees.FirstOrDefault(x => x.ObjectId == currentNode.parentId);
            }

            return hierarchy;
        }
    }
}
