using System;
using Interfaces.Implementation.Action;
using NLog;
using System.Collections.Generic;
using Core.Notification;
using System.Globalization;
using System.Web.Http.OData.Query;
using System.Reflection;
using Module.Persist.TPM.Model.TPM;
using System.Linq;
using Module.Persist.TPM.Utils.Filter;
using Core.Dependency;

namespace Module.Host.TPM.Actions.Notifications {
    /// <summary>
    /// Класс для формирования и рассылки уведомления по новым продуктам подходящим для текущих промо
    /// </summary>
    public class BaseNotificationAction : BaseAction {
        public override void Execute() { }

        /// <summary>
        /// Отправка нотификации
        /// </summary>
        /// <param name="body"></param>
        /// <param name="name">Имя нотификации</param>
        protected virtual void SendNotification(string body, string name) {
            IDictionary<string, string> parameters = new Dictionary<string, string>() {
                                    { "HTML_SCAFFOLD", body }
                                };
            EmailGetterArgument eventArgument = new EmailGetterArgument();
            notifier.Notify(name, parameters, eventArgument);
        }

		/// <summary>
		/// Отправка нотификации по Email
		/// </summary>
		/// <param name="body"></param>
		/// <param name="name">Имя нотификации</param>
		/// <param name="emails">Emails получателей</param>
		protected virtual void SendNotificationByEmails(string body, string name, string[] emails)
		{
			IDictionary<string, string> parameters = new Dictionary<string, string>() {
									{ "HTML_SCAFFOLD", body }
								};
			EmailGetterArgument eventArgument = new EmailGetterArgument();
			notifier.NotifyByEmails(emails, name, parameters, eventArgument);
		}

		/// <summary>
		/// Преобразование записи в словарь поле/значение
		/// </summary>
		/// <param name="record"></param>
		/// <returns></returns>
		protected IDictionary<string, object> GetDictionary(object record) {
            IDictionary<string, object> dict;
            if (record is ISelectExpandWrapper) {
                dict = ((ISelectExpandWrapper) record).ToDictionary();
            } else {
                dict = new Dictionary<string, object>();
                Type elementType = record.GetType();
                PropertyInfo[] properties = elementType.GetProperties();
                foreach (PropertyInfo property in properties) {
                    dict[property.Name] = property.GetValue(record);
                }
            }
            return dict;
        }
        /// <summary>
        /// Получение значения поля из записи
        /// </summary>
        /// <param name="record"></param>
        /// <param name="field">Принимает так же навигационные поля "field1.field2"</param>
        /// <returns></returns>
        protected object GetValue(IDictionary<string, object> record, string field) {
            string[] path = field.Split(new char[] { '.' }, 2);
            object val;
            if (record.TryGetValue(path[0], out val)) {
                if (val != null && path.Length > 1) {
                    IDictionary<string, object> dict = GetDictionary(val);
                    return GetValue(dict, path[1]);
                } else {
                    return val;
                }
            } else {
                return null;
            }
        }
        /// <summary>
        /// Преобразование значения поля в строку
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected string EncodeValue(object val) {
			string value = val == null ? String.Empty : val.ToString();
			if (!String.IsNullOrEmpty(value)) {
				string[] dateFormats = new string[] {
					"dd/MM/yyyy hh:mm:ss tt zzz", "MM/dd/yyyy hh:mm:ss tt zzz",
					"d/MM/yyyy hh:mm:ss tt zzz", "MM/d/yyyy hh:mm:ss tt zzz",
					"dd/M/yyyy hh:mm:ss tt zzz", "M/dd/yyyy hh:mm:ss tt zzz",
					"d/M/yyyy hh:mm:ss tt zzz", "M/d/yyyy hh:mm:ss tt zzz",
					"dd/MM/yyyy hh:mm:ss tt", "MM/dd/yyyy hh:mm:ss tt",
					"d/MM/yyyy hh:mm:ss tt", "MM/d/yyyy hh:mm:ss tt",
					"dd/M/yyyy hh:mm:ss tt", "M/dd/yyyy hh:mm:ss tt",
					"d/M/yyyy hh:mm:ss tt", "M/d/yyyy hh:mm:ss tt" };
				DateTimeOffset date;
                Boolean boolVal;
				Double doubleVal;
				if (DateTimeOffset.TryParseExact(value, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date)) {
                    value = date.ToString("dd.MM.yyyy");
				} else if (Boolean.TryParse(value, out boolVal)) {
                    value = boolVal ? "Yes" : "No";
                } else if (Double.TryParse(value, out doubleVal)) {
					value = String.Format("{0:0.##}", val);
				}
            }
            return value;
        }
        /// <summary>
        /// Формирование строки оповещения
        /// </summary>
        /// <param name="record"></param>
        /// <param name="propertiesOrder"></param>
        /// <returns></returns>
        protected List<string> GetRow(object record, string[] propertiesOrder) {
            IDictionary<string, object> dict = GetDictionary(record);
            List<string> allRowCells = new List<string>();
            foreach (string property in propertiesOrder) {
                string value = EncodeValue(GetValue(dict, property));
                allRowCells.Add(String.Format(cellTemplate, value));
            }
            return allRowCells;
        }

        /// <summary>
        /// Список преобразованных в функции фильтров из узлов иерархии
        /// </summary>
        /// <param name="productTreeNodes"></param>
        /// <returns></returns>
        protected List<Func<Product, bool>> GetExpressionList(IQueryable<ProductTree> productTreeNodes) {
            List<Func<Product, bool>> expressionsList = new List<Func<Product, bool>>();
            foreach (ProductTree node in productTreeNodes) {
                if (node != null && !String.IsNullOrEmpty(node.Filter)) {
                    string stringFilter = node.Filter;
                    // Преобразованиестроки фильтра в соответствующий класс
                    FilterNode filter = stringFilter.ConvertToNode();
                    // Создание функции фильтрации на основе построенного фильтра
                    var expr = filter.ToExpressionTree<Product>();
                    expressionsList.Add(expr.Compile());
                }
            }
            return expressionsList;
        }

        protected const string rowTemplate = "<tr>{0}</tr>";
        protected const string cellTemplate = "<td class='tg-0pky'>{0}</td>";

        protected readonly static Logger logger = LogManager.GetCurrentClassLogger();
        IMailNotificationService notifier = (IMailNotificationService) IoC.Kernel.GetService(typeof(IMailNotificationService));
    }
}