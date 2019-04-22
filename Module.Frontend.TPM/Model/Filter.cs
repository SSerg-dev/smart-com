using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Frontend.TPM.Model {
    [Serializable]
    public class Filter {
        public string fieldName { get; set; }
        public List<Guid> value { get; set; }
        public string displayValue { get; set; }
    }
}
