using Model.Host.TPM.Handlers.DataFlow;
using Module.Host.TPM.Handlers.DataFlow.Modules;
using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Module.Host.TPM.Handlers.DataFlow.Modules.TradeInvestmentDataFlowModule;

namespace Module.Host.TPM.Handlers.DataFlow.Filters
{
    public class TradeInvestmentDataFlowFilter : DataFlowFilter
    {
        public IEnumerable<TradeInvestmentDataFlowModule.TradeInvestmentDataFlowSimpleModel> ChangedModels { get; }

        public TradeInvestmentDataFlowFilter(
            IEnumerable<ChangesIncidentDataFlowModule.ChangesIncidentDataFlowSimpleModel> changesIncidents, DataFlowModuleCollection dataFlowModuleCollection)
            : base(dataFlowModuleCollection)
        {
            var currentChangesIncidents = changesIncidents.Where(x => x.DirectoryName == nameof(TradeInvestment));
            var currentChangesIncidentsIds = base.GetChangedModelsGuid(currentChangesIncidents);

            this.ChangedModels = this.DataFlowModuleCollection.TradeInvestmentDataFlowModule.Collection
                .Where(x => currentChangesIncidentsIds.Contains(x.Id));
        }

        public Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string> Apply(TradeInvestmentDataFlowModule.TradeInvestmentDataFlowSimpleModel tradeInvestment, IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel> promoes)
        {
            var filteredPromoes = new List<PromoDataFlowModule.PromoDataFlowSimpleModel>();
            foreach (var promo in promoes)
            {
                if (this.InnerApply(promo, tradeInvestment))
                {
                    filteredPromoes.Add(promo);
                }
            }

            return new Tuple<IEnumerable<PromoDataFlowModule.PromoDataFlowSimpleModel>, string>(filteredPromoes, this.ToString());
        }

        private bool InnerApply(PromoDataFlowModule.PromoDataFlowSimpleModel promo, TradeInvestmentDataFlowSimpleModel tradeInvestment)
        {
            //выполняется стандартный подбор TI для проверяемого промо, если в результирующем наборе окажется 
            //отобранный по инциденту TI, то промо отберется на пересчет

            var clientNode = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection
                .Where(x => x.ObjectId == promo.ClientTreeId && !x.EndDate.HasValue)
                .FirstOrDefault();
            var isPromoFiltered = false;

            // Список всех подошедших записей из таблицы TI
            List<TradeInvestmentDataFlowSimpleModel> tradeInvestments = new List<TradeInvestmentDataFlowSimpleModel>();

            // Пока в отфильтрованном списке пусто и мы не достигли корневого элемента
            while ((tradeInvestments == null || tradeInvestments.Count() == 0) && clientNode != null && clientNode.Type != "root")
            {
                tradeInvestments = this.DataFlowModuleCollection.TradeInvestmentDataFlowModule.Collection
                    // Фильтр по клиенту
                    .Where(x => x.ClientTreeId == clientNode.Id && !x.Disabled)
                    // Фильтр по брендтеху
                    .Where(x => x.BrandTechId == null || x.BrandTechId == promo.BrandTechId)
                    // promo start date должна лежать в интервале между TI start date и TI end date
                    .Where(x => x.StartDate.HasValue && x.EndDate.HasValue && promo.StartDate.HasValue
                           && DateTimeOffset.Compare(x.StartDate.Value, promo.StartDate.Value) <= 0
                           && DateTimeOffset.Compare(x.EndDate.Value, promo.StartDate.Value) >= 0).ToList();

                clientNode = this.DataFlowModuleCollection.ClientTreeDataFlowModule.Collection
                    .Where(x => x.ObjectId == clientNode.ParentId && !x.EndDate.HasValue)
                    .FirstOrDefault();

            }

            var tradeInvestmentsList = new List<TradeInvestmentDataFlowSimpleModel>(tradeInvestments);
            bool containsDublicate = false;

            // Если присутсвуют записи с пустым и заполненным брендтехом, берем только с заполненным
            // при условии, что тип и подтип совпадают
            if (tradeInvestments.Any(x => x.BrandTechId == null) && tradeInvestments.Any(x => x.BrandTechId != null))
            {
                tradeInvestmentsList = new List<TradeInvestmentDataFlowSimpleModel>();
                // Группируем по типу и подтипу
                var tradeInvestmentTypeSubtypeGroups = tradeInvestments.GroupBy(x => new { x.TIType, x.TISubType });

                // Перебираем группы с ключами типом и подтипом
                foreach (var tradeInvestmentTypeSubtypeGroup in tradeInvestmentTypeSubtypeGroups)
                {
                    // Если в списке TI есть запись с пустым брендтехом
                    if (!containsDublicate && tradeInvestmentTypeSubtypeGroup.Any(x => x.BrandTechId == null))
                    {
                        containsDublicate = true;
                    }

                    // Формируем новый список TI записей (без пустых брендтехов)
                    tradeInvestmentsList.AddRange(tradeInvestmentTypeSubtypeGroup.Where(x => x.BrandTechId != null));
                }
            }

            if (tradeInvestmentsList.Contains(tradeInvestment))
            {
                isPromoFiltered = true;
            }

            return isPromoFiltered;
        }

        public override string ToString()
        {
            return nameof(TradeInvestmentDataFlowFilter);
        }
    }
}