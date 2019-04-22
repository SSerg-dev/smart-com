using System.Collections.Generic;

namespace Module.Persist.TPM.PromoStateControl.RoleStateMap {
    public class RoleStateMap {
        public string StatusName { get; set; }
        public Dictionary<string, List<string>> StateMap { get; set; }

        public RoleStateMap(string statusName, Dictionary<string, List<string>> аvailableStates) {
            StatusName = statusName;
            StateMap = аvailableStates;
        }
    }
}
