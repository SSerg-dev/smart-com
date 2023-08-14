using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
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
                { StateNames.DRAFT_PUBLISHED, new List<string> { "Administrator", "SupportAdministrator", "CustomerMarketing", "KeyAccountManager", "FunctionalExpert" } },
                { StateNames.DELETED, new List<string> { "Administrator", "SupportAdministrator", "CustomerMarketing", "KeyAccountManager", "FunctionalExpert" } }
            }),
            new RoleStateMap(StateNames.DRAFT_PUBLISHED, new Dictionary<string, List<string>>(){
                { StateNames.DRAFT, new List<string> { "Administrator", "SupportAdministrator", "CustomerMarketing", "FunctionalExpert", "KeyAccountManager" } },
                { StateNames.ON_APPROVAL, new List<string> { "System", "Administrator", "SupportAdministrator", "CustomerMarketing", "FunctionalExpert", "KeyAccountManager" } },
                { StateNames.DELETED, new List<string> { "Administrator", "SupportAdministrator", "CustomerMarketing", "FunctionalExpert", "KeyAccountManager" } }
            }),
            new RoleStateMap(StateNames.ON_APPROVAL, new Dictionary<string, List<string>>() {
                { StateNames.DRAFT_PUBLISHED, new List<string> { "Administrator", "SupportAdministrator", "CMManager", "CustomerMarketing", "FunctionalExpert", "DemandPlanning", "KeyAccountManager", "GAManager" } },
                { StateNames.ON_APPROVAL, new List<string> { "System" } },
                { StateNames.APPROVED, new List<string> { "CMManager", "SupportAdministrator", "DemandFinance", "DemandPlanning", "GAManager" } }
            }),
            new RoleStateMap(StateNames.APPROVED, new Dictionary<string, List<string>>() {
                { StateNames.DRAFT_PUBLISHED, new List<string> { "Administrator", "SupportAdministrator" } },
                { StateNames.PLANNED, new List<string> { "Administrator", "SupportAdministrator", "KeyAccountManager", "FunctionalExpert" } },
                { StateNames.ON_APPROVAL, new List<string> { "System", "SupportAdministrator" } },
                { StateNames.CANCELLED, new List<string> { "Administrator", "SupportAdministrator", "KeyAccountManager", "FunctionalExpert" } }
            }),
            new RoleStateMap(StateNames.CANCELLED, new Dictionary<string, List<string>>(){ }),
            new RoleStateMap(StateNames.PLANNED, new Dictionary<string, List<string>>(){
                { StateNames.ON_APPROVAL, new List<string> { "System", "SupportAdministrator" } },
                { StateNames.STARTED, new List<string> { "System", "SupportAdministrator"} },
                { StateNames.DRAFT_PUBLISHED, new List<string> { "Administrator", "SupportAdministrator" } },
				{ StateNames.CANCELLED, new List<string> { "Administrator", "SupportAdministrator", "KeyAccountManager", "FunctionalExpert" } }
			}),
            new RoleStateMap(StateNames.STARTED, new Dictionary<string, List<string>>(){
                { StateNames.FINISHED, new List<string> { "System", "SupportAdministrator" } },
                { StateNames.CANCELLED, new List<string> { "Administrator", "KeyAccountManager", "SupportAdministrator", "FunctionalExpert" } }
            }),
            new RoleStateMap(StateNames.FINISHED, new Dictionary<string, List<string>>(){
                { StateNames.CLOSED, new List<string> { "System", "Administrator", "SupportAdministrator", "KeyAccountManager", "FunctionalExpert" } }
            }),
            new RoleStateMap(StateNames.CLOSED, new Dictionary<string, List<string>>() {
                { StateNames.FINISHED, new List<string> { "Administrator", "SupportAdministrator", "FunctionalExpert" } },
            }),
            new RoleStateMap(StateNames.UNDEFINED, new Dictionary<string, List<string>>(){
                { StateNames.DRAFT, new List<string> { "Administrator", "SupportAdministrator", "CMManager", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" } },
                { StateNames.DRAFT_PUBLISHED, new List<string> { "System", "Administrator", "SupportAdministrator", "CMManager", "CustomerMarketing", "DemandFinance", "DemandPlanning", "FunctionalExpert", "KeyAccountManager" } }
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
                return roleStateMap.StateMap;
            }
        }

        /// <summary>
        /// Проверка на то, что данная роль может изменить статус данного промо
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="statusName"></param>
        /// <returns></returns>
        public static bool RoleCanChangeState(string roleName, string statusName) {
			bool isChangeAvailable = GetMapForStatus(statusName).Any(x => x.Value.Contains(roleName));

			if (roleName.Equals("KeyAccountManager") && (statusName.Equals(StateNames.PLANNED) || statusName.Equals(StateNames.ON_APPROVAL)))
			{
				isChangeAvailable = false;
			}
			return isChangeAvailable;
        }

        /// <summary>
        /// Доступен ли текущий шаг согласования для текущей роли
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="promo"></param>
        /// <returns></returns>
        public static bool IsOnApprovalRoleOrder(string roleName, Promo promo) {
            bool isAvailable = true;
            bool IsCMManagerApproved = promo.IsCMManagerApproved ?? false;
            bool IsDemandPlanningApproved = promo.IsDemandPlanningApproved ?? false;
            bool IsGAManagerApproved = promo.IsGAManagerApproved ?? false;
            if (promo.PromoStatus.SystemName == StateNames.ON_APPROVAL) {
                if (!promo.IsGrowthAcceleration && !promo.IsInExchange)
                    switch (roleName)
                    {
                        case "CustomerMarketing":
                        case "CMManager":
                            isAvailable = !IsCMManagerApproved;
                            break;
                        case "DemandPlanning":
                            isAvailable = IsCMManagerApproved && !IsDemandPlanningApproved;
                            break;
                        case "DemandFinance":
                            isAvailable = false;
                            break;
                    }
                else
                    switch (roleName)
                    {
                        case "CMManager":
                            isAvailable = !IsCMManagerApproved;
                            break;
                        case "DemandPlanning":
                            isAvailable = IsCMManagerApproved && !IsDemandPlanningApproved;
                            break;
                        case "GAManager":
                            isAvailable = IsCMManagerApproved && IsDemandPlanningApproved && !IsGAManagerApproved;
                            break;
                    }
            }
            return isAvailable;
        }
        /// <summary>
        /// Доступен ли текущий шаг согласования для текущей роли
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="promo"></param>
        /// <returns></returns>
        public static bool IsOnApprovalRoleOrder(string roleName, PromoGridView promo) {
            bool isAvailable = true;
            bool IsCMManagerApproved = promo.IsCMManagerApproved ?? false;
            bool IsDemandPlanningApproved = promo.IsDemandPlanningApproved ?? false;
            bool IsGAManagerApproved = promo.IsGAManagerApproved ?? false;
            if (promo.PromoStatusSystemName == StateNames.ON_APPROVAL) {
                if (!promo.IsGrowthAcceleration && !promo.IsInExchange)
                    switch (roleName)
                    {
                        case "CustomerMarketing":
                        case "CMManager":
                            isAvailable = !IsCMManagerApproved;
                            break;
                        case "DemandPlanning":
                            isAvailable = IsCMManagerApproved && !IsDemandPlanningApproved;
                            break;
                        case "DemandFinance":
                            isAvailable = false;
                            break;
                    }
                else
                    switch (roleName)
                    {
                        case "CMManager":
                            isAvailable = !IsCMManagerApproved;
                            break;
                        case "DemandPlanning":
                            isAvailable = IsCMManagerApproved && !IsDemandPlanningApproved;
                            break;
                        case "DemandFinance":
                            isAvailable = IsCMManagerApproved && IsDemandPlanningApproved;
                            break;
                        case "GAManager":
                            isAvailable = IsCMManagerApproved && IsDemandPlanningApproved && !IsGAManagerApproved;
                            break;
                    }
            }
            return isAvailable;
        }
    }
}
