using System;
using System.Collections.Generic;

namespace Module.Frontend.TPM.Model
{
    [Serializable]
    public class RPAEventJsonField
    {
        public string handlerName { get; set; }
        public List<Object> parametrs { get; set; }
        public List<string> roles { get; set; }
    }
}
