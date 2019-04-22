using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Persist.TPM.PromoStateControl.RoleStateMap {
    /// <summary>
    /// Класс для определения возможности перевода промо под текущей ролью в другой статус
    /// </summary>
    public static class RoleStateUtil {
        public static List<RoleStateMap> StatusRoleStateMap = new List<RoleStateMap>() {
            new RoleStateMap(StateNames.DELETED, new Dictionary<string, List<string>>(){ }),
            new RoleStateMap(StateNames.DRAFT, new Dictionary<string, List<string>>(){
                { StateNames.DRAFT_PUBLISHED, new List<string> { "Administrator", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" } },
                { StateNames.DELETED, new List<string> { "Administrator", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" } }
            }),
            new RoleStateMap(StateNames.DRAFT_PUBLISHED, new Dictionary<string, List<string>>(){
                { StateNames.DRAFT, new List<string> { "Administrator", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" } },
                { StateNames.ON_APPROVAL, new List<string> { "Administrator", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" } }
            }),
            new RoleStateMap(StateNames.ON_APPROVAL, new Dictionary<string, List<string>>() {
                { StateNames.DRAFT_PUBLISHED, new List<string> { "Administrator", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" } },
                { StateNames.APPROVED, new List<string> { "Administrator", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" } }
            }),
            new RoleStateMap(StateNames.APPROVED, new Dictionary<string, List<string>>() {
                { StateNames.DRAFT_PUBLISHED, new List<string> { "Administrator", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" } },
                { StateNames.PLANNED, new List<string> { "Administrator", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" } }
            }),
            new RoleStateMap(StateNames.PLANNED, new Dictionary<string, List<string>>(){
                { StateNames.STARTED, new List<string> { "System" } },
                { StateNames.DRAFT_PUBLISHED, new List<string> { "Administrator", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" } }
            }),
            new RoleStateMap(StateNames.STARTED, new Dictionary<string, List<string>>(){
                { StateNames.FINISHED, new List<string> { "System", "Administrator", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" } }
            }),
            new RoleStateMap(StateNames.FINISHED, new Dictionary<string, List<string>>(){
                { StateNames.CLOSED, new List<string> { "System", "Administrator", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" } }
            }),
            new RoleStateMap(StateNames.CLOSED, new Dictionary<string, List<string>>()),
            new RoleStateMap(StateNames.UNDEFINED, new Dictionary<string, List<string>>(){
                { StateNames.DRAFT, new List<string> { "Administrator", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" } },
                { StateNames.DRAFT_PUBLISHED, new List<string> { "Administrator", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" } }
            })
        };

        /// <summary>
        /// Получение словаря Статус: Список ролей для статуса
        /// </summary>
        /// <param name="statusName"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetMapForStatus(string statusName) {
            var roleStateMap = StatusRoleStateMap.FirstOrDefault(s => s.StatusName == statusName);
            if (roleStateMap == null) {
                throw new Exception(String.Format("Can't find role state map for Status Name '{0}'", statusName));
            } else {
                return StatusRoleStateMap.FirstOrDefault(s => s.StatusName == statusName).StateMap;
            }
        }

        /// <summary>
        /// Проверка на то, что данная роль может изменить статус данного промо
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="statusName"></param>
        /// <returns></returns>
        public static bool RoleCanChangeState(string roleName, string statusName) {
            return GetMapForStatus(statusName).Any(x => x.Value.Contains(roleName));
        } 
    }
}
