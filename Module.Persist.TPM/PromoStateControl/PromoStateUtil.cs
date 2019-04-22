using System.Collections.Generic;
using System.Linq;

namespace Module.Persist.TPM.PromoStateControl
{
    public static class PromoStateUtil
    {
        public static bool CheckAccess(Dictionary<string, List<string>> AvailableStates, string state, string role)
        {
            bool result = false;

            List<string> availableState;

            if (AvailableStates.TryGetValue(state, out availableState))
            {
                if (availableState.Contains(role))
                {
                    return true;
                }
            }

            return result;
        }

        public static bool CheckAccess(List<string> roles, string role)
        {
            var item = roles.Where(x => x.Contains(role)).FirstOrDefault();
            return (item == null) ? false : true;
        }
    }
}
