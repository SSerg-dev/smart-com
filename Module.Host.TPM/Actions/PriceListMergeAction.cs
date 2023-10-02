using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Core.Dependency;
using Core.Extensions;
using Core.Settings;
using Interfaces.Implementation.Action;
using Looper.Core;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using Utility.LogWriter;
using static Module.Persist.TPM.Model.TPM.PriceListEqualityComparer;

namespace Module.Host.TPM.Actions
{
    public class PriceListMergeAction : BaseAction
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogWriter _fileLogWriter;
        private readonly ExecuteData _executeData;

        public PriceListMergeAction(DatabaseContext databaseContext, ILogWriter fileLogWriter, ExecuteData executeData)
        {
            _databaseContext = databaseContext;
            _fileLogWriter = fileLogWriter;
            _executeData = executeData;
        }

        public override void Execute()
        {
            var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));

            var dateSeparator = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
            string dateSeparatorSqlString = $"CAST('{dateSeparator}' AS DATETIME)";
            string sql = $"SELECT * FROM [DefaultSchemaSetting].[{nameof(PRICELIST_FDM)}] WHERE {dateSeparatorSqlString} <= [FINISH_DATE]";

            var priceListFDMs = _databaseContext.SqlQuery<PRICELIST_FDM>(sql);
            var priceLists = _databaseContext.Set<PriceList>().Where(x => !x.Disabled);
            var clientTrees = _databaseContext.Set<ClientTree>().Where(x => !x.EndDate.HasValue);
            var products = _databaseContext.Set<Product>().Where(x => !x.Disabled).Select(x => new { x.Id, x.ZREP }).ToList().Select(x => new Product { Id = x.Id, ZREP = x.ZREP });

            var priceListFDMsMaterialized = priceListFDMs.ToList();
            var priceListsMaterialized = priceLists.ToList();
            var clientTreesMaterialized = clientTrees.ToList();
            var productsMaterialized = products.ToList();

            var activeCodes = priceListFDMs.Select(x => x.G_HIERARCHY_ID).Distinct().ToList();

            //FDMSync(ref priceListFDMsMaterialized, Convert.ToDateTime(dateSeparator));
            var validAfterClientsCheckPriceListFDMs = CheckClients(priceListFDMsMaterialized, clientTreesMaterialized);
            var validAfterProductsCheckPriceListFDMs = CheckProducts(priceListFDMsMaterialized, products);
            var validAfterDublicatesCheckPriceListFDMs = CheckDublicates(priceListFDMsMaterialized);

            var validPriceListFDMs = validAfterClientsCheckPriceListFDMs
                                        .Intersect(validAfterProductsCheckPriceListFDMs)
                                        .Intersect(validAfterDublicatesCheckPriceListFDMs);

            var differentPriceLists = GetDifferentPriceList(validPriceListFDMs, priceListsMaterialized, clientTreesMaterialized, productsMaterialized, activeCodes);
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
            var validPriceListFDMs = priceListFDMClientGroups.SelectMany(x => x.ToList()).Except(invalidClientGroups.SelectMany(x => x.ToList()));
            return validPriceListFDMs;
        }

        private IEnumerable<PRICELIST_FDM> CheckProducts(IEnumerable<PRICELIST_FDM> priceListFDMs, IEnumerable<Product> products)
        {
            var priceListFDMProductGroups = priceListFDMs.GroupBy(x => new { x.ZREP });
            var invalidProductGroups = priceListFDMProductGroups.Where(x => !products.Any(y => y.ZREP == x.Key.ZREP));
            var validPriceListFDMs = priceListFDMProductGroups.SelectMany(x => x.ToList()).Except(invalidProductGroups.SelectMany(x => x.ToList()));
            return validPriceListFDMs;
        }

        private IEnumerable<PRICELIST_FDM> CheckDublicates(IEnumerable<PRICELIST_FDM> priceListFDMs)
        {
            var priceListFDMClientProductGroups = priceListFDMs.GroupBy(x => new { x.G_HIERARCHY_ID, x.ZREP, x.START_DATE, x.FINISH_DATE });
            var dupblicateGroups = priceListFDMClientProductGroups.Where(x => x.Count() > 1);

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

        private IEnumerable<PriceList> GetDifferentPriceList(IEnumerable<PRICELIST_FDM> priceListFDMs, IEnumerable<PriceList> priceListsMaterialized, IEnumerable<ClientTree> clientTrees, IEnumerable<Product> products, IEnumerable<string> activeCodes)
        {
            var newPriceLists = GetNewPriceListRecords(priceListFDMs, clientTrees, products, activeCodes);
            var differentPriceLists = newPriceLists.Except(priceListsMaterialized, new PriceListEqualityComparer());
            return differentPriceLists;
        }

        private void FillPriceListTable(IEnumerable<PriceList> differentPriceLists, List<PriceList> priceListMaterialized, List<ClientTree> clientTrees, List<Product> products)
        {
            using (var context = new DatabaseContext())
            {
                var priceLists = context.Set<PriceList>();

                //var invalidPriceListRecords = differentPriceLists.Intersect(priceListMaterialized, new checkPriceListEqualityComparer());
                var invalidPriceListRecords = new List<PriceList>();
                foreach (var differentPriceList in differentPriceLists)
                {
                    invalidPriceListRecords.AddRange(priceListMaterialized.Where(x =>
                        x.DeletedDate == differentPriceList.DeletedDate &&
                        x.StartDate == differentPriceList.StartDate &&
                        x.ClientTreeId == differentPriceList.ClientTreeId &&
                        x.ProductId == differentPriceList.ProductId &&
                        x.FuturePriceMarker == differentPriceList.FuturePriceMarker));
                }
                if (invalidPriceListRecords.Count() > 0)
                {
                    UpdateRecords(invalidPriceListRecords);
                    CreateIncident(invalidPriceListRecords);
                }

                foreach (var differentPriceList in differentPriceLists)
                {
                    differentPriceList.Product = null;
                    differentPriceList.ClientTree = null;
                    differentPriceList.ModifiedDate = DateTimeOffset.Now;
                }
                var logLine = $"New {nameof(PriceList)}: {differentPriceLists.Count()}";
                _fileLogWriter.Write(true, logLine, "Message");

                foreach (IEnumerable<PriceList> items in differentPriceLists.Partition(1000))
                {
                    context.Set<PriceList>().AddRange(items);
                    context.SaveChanges();
                }
            }

        }

        private void CreateIncident(IEnumerable<PriceList> invalidPriceListRecords)
        {
            _databaseContext.Set<ChangesIncident>();
            var incidents = invalidPriceListRecords.Select(x => new ChangesIncident
            {
                DirectoryName = nameof(PriceList),
                ItemId = x.Id.ToString(),
                CreateDate = DateTimeOffset.Now,
                Disabled = false
            });
            _databaseContext.Set<ChangesIncident>().AddRange(incidents);

        }

        private void UpdateRecords(IEnumerable<PriceList> invalidPriceListRecords)
        {
            var ids = invalidPriceListRecords.Select(x => x.Id);
            var records = _databaseContext.Set<PriceList>().Where(x => ids.Contains(x.Id));
            foreach (var rec in records)
            {
                rec.DeletedDate = DateTimeOffset.Now;
                rec.Disabled = true;
                rec.ModifiedDate = DateTimeOffset.Now;
            }
            _databaseContext.SaveChanges();
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

        private Dictionary<string, List<ClientTree>> GetClientsWithActiveCodes(IEnumerable<ClientTree> clientTrees, IEnumerable<string> activeCodes) 
        {
            var result = new Dictionary<string, List<ClientTree>>();
            var baseClients = clientTrees.Where(x => x.IsBaseClient);

            foreach (var client in baseClients) 
            {
                var hierarchyForBaseClient = GetHierarhcy(clientTrees, client);

                var activeNode = hierarchyForBaseClient
                    .Where(x => !String.IsNullOrEmpty(x.GHierarchyCode))
                    .OrderByDescending(x => x.ObjectId)
                    .FirstOrDefault(x => activeCodes.Contains(x.GHierarchyCode.TrimStart('0')));

                if (activeNode == null) continue;

                var activeCode = activeNode.GHierarchyCode.TrimStart('0');

                if (result.ContainsKey(activeCode))
                    result[activeCode].Add(client);
                else
                    result.Add(activeCode, new List<ClientTree> { client });
            }

            return result;
        }

        private IEnumerable<PriceList> GetNewPriceListRecords(IEnumerable<PRICELIST_FDM> priceListFDMs, IEnumerable<ClientTree> clientTrees, IEnumerable<Product> products, IEnumerable<string> activeCodes)
        {
            var newPriceListRecords = new List<PriceList>();

            var clientsWithCodes = GetClientsWithActiveCodes(clientTrees, activeCodes);

            var existingCodes = clientsWithCodes.Select(x => x.Key);

            foreach (var priceListFDM in priceListFDMs.Where(x => existingCodes.Contains(x.G_HIERARCHY_ID)))
            {
                var baseClientTrees = clientsWithCodes[priceListFDM.G_HIERARCHY_ID];

                foreach (var baseClientTree in baseClientTrees)
                {
                    var product = products.FirstOrDefault(x => x.ZREP == priceListFDM.ZREP);
                    if (product != null)
                    {
                        var newPriceListRecord = new PriceList
                        {
                            Disabled = false,
                            DeletedDate = null,
                            StartDate = priceListFDM.START_DATE.Date,
                            EndDate = priceListFDM.FINISH_DATE.Date,
                            Price = priceListFDM.PRICE,
                            ClientTreeId = baseClientTree.Id,
                            ProductId = product.Id,
                            ClientTree = baseClientTree,
                            Product = product,
                            FuturePriceMarker = priceListFDM.RELEASE_STATUS == "A"
                        };

                        newPriceListRecord.StartDate = ChangeTimeZoneUtil.ResetTimeZone(newPriceListRecord.StartDate);
                        newPriceListRecord.EndDate = ChangeTimeZoneUtil.ResetTimeZone(newPriceListRecord.EndDate);

                        newPriceListRecords.Add(newPriceListRecord);
                    }
                }
            }

            return newPriceListRecords;
        }

        private IEnumerable<ClientTree> GetBaseClientsByPriceListFDM(IEnumerable<ClientTree> clientTrees, PRICELIST_FDM priceListFDM, IEnumerable<string> activeCodes)
        {
            var baseClients = clientTrees.Where(x => x.IsBaseClient);
            var baseClientsForCurrentPriceListFDM = new List<ClientTree>();

            foreach (var baseClient in baseClients)
            {
                var hierarchyForBaseClient = GetHierarhcy(clientTrees, baseClient);
                if (hierarchyForBaseClient.Any(x => !string.IsNullOrEmpty(x.GHierarchyCode) && x.GHierarchyCode.TrimStart('0') == priceListFDM.G_HIERARCHY_ID))
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
