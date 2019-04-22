using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Frontend.TPM.Model {
    [Serializable]
    public class FilterContainer {
        public string displayFilter { get; set; }
        public List<Filter> filter { get; set; }

        public List<Guid> GetFilterValue(string fieldName) {
            List<Guid> result = null;
            if (filter != null) {
                Filter f = filter.FirstOrDefault(x => x.fieldName == fieldName);
                if (f != null) {
                    result = f.value;
                }
            }
            return result;
        }
    }
}
