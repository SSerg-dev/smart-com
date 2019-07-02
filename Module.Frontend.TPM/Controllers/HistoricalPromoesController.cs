using Core.History;
using Module.Persist.TPM.Model.History;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers {

    public class HistoricalPromoesController : ODataController {
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
        public IQueryable<HistoricalPromo> GetHistoricalPromoes()
        {
            // Для поддержки старой истории ищем по id промо(legacy)
            int indexGuid = Request.RequestUri.AbsoluteUri.IndexOf("guid");
            if (indexGuid >= 0)
            {
                Guid promoId = Guid.Parse(Request.RequestUri.AbsoluteUri.Substring(indexGuid + 7, 36));
                // все записи истории для данного промо
                HistoricalPromo[] allHistPromo = HistoryReader.Query<HistoricalPromo>().Where(n => n._ObjectId == promoId).ToArray();

                // promo у которых есть Operation ID
                HistoricalPromo[] hisPromoWithOperationID = allHistPromo.Where(n => n.OperationId.HasValue && n.OperationId != Guid.Empty).ToArray();
                // остальные записи, сюда же добавим последнюю запись из группы (по OperationId)
                List<HistoricalPromo> preliminaryList = allHistPromo.Where(n => !n.OperationId.HasValue || n.OperationId == Guid.Empty).ToList();

                // в самом Raven нет Group By, поэтому необходима была материализация
                // группируем по Operation ID
                var groupedPromoes = hisPromoWithOperationID.GroupBy(n => n.OperationId);
                foreach (var group in groupedPromoes)
                {
                    var sortedGroup = group.OrderBy(n => n._EditDate);
                    // запись после вызова первого save change позволяет определить вид операции (Create или Update)
                    HistoricalPromo firstRecordInGroup = sortedGroup.First();
                    // последняя запись и является итоговой для группы
                    HistoricalPromo lastRecordInGroup = sortedGroup.Last();
                    lastRecordInGroup._Operation = firstRecordInGroup._Operation;
                    preliminaryList.Add(lastRecordInGroup);
                }

                // сортируем по дате
                preliminaryList = preliminaryList.OrderBy(n => n._EditDate).ToList();
                // итоговый список
                List<HistoricalPromo> result = new List<HistoricalPromo>();

                // исключаем записи без изменений (например в расчетах может не произойти изменений)
                for (int i = 0; i < preliminaryList.Count; i++)
                    if (i == 0 || ChangedPromo(preliminaryList[i], preliminaryList[i - 1]))
                        result.Add(preliminaryList[i]);                

                return result.AsQueryable();
            }
            else
                return HistoryReader.Query<HistoricalPromo>();
        }

        /// <summary>
        /// Определить различаются ли 2 экземпляра
        /// </summary>
        /// <param name="current">Текущая запись</param>
        /// <param name="previous">Предыдущая запись</param>
        /// <returns></returns>
        private bool ChangedPromo(HistoricalPromo current, HistoricalPromo previous)
        {
            // немножко рефлексии
            Type typeHistoricalPromo = current.GetType(); // тип
            PropertyInfo[] properties = typeHistoricalPromo.GetProperties(); // список свойств
            // список полей, которые не нужно сравнивать
            string[] systemProperyNames = { "_Id", "_ObjectId", "_User", "_Role", "_EditDate", "_Operation", "OperationId" };
            // результат
            bool result = false;

            for (int i = 0; i < properties.Length && !result; i++)
            {
                if (!systemProperyNames.Contains(properties[i].Name))
                {
                    // если поля не равны, значит были изменения
                    object currentValue = properties[i].GetValue(current);
                    object oldValue = properties[i].GetValue(previous);

                    result = !Object.Equals(currentValue, oldValue);
                }
            }

            return result;
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                HistoryReader.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
