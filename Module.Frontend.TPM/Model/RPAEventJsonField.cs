using System;
using System.Collections.Generic;

namespace Module.Frontend.TPM.Model
{
    [Serializable]
    public class RPAEventJsonField
    {
        public string name { get; set; }
        public List<Object> parametrs { get; set; }
        public List<string> roles { get; set; }
        public List<Object> templateColumns { get; set; }
    }
}
