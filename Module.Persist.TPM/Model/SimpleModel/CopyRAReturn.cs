using Module.Persist.TPM.Model.TPM;
using System.Collections.Generic;

namespace Module.Persist.TPM.Model.SimpleModel
{
    public class CopyRAReturn
    {
        public List<Promo> Promos { get; set; }
        public List<string> Errors { get; set; }
    }
}
