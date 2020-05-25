using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.DTO
{
    public class PRICELIST_FDM
    {
        public string G_HIERARCHY_ID { get; set; }
        public string ZREP { get; set; }
        public double PRICE { get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime FINISH_DATE { get; set; }
        public string UNIT_OF_MEASURE { get; set; }
        public string CURRENCY { get; set; }

        public override string ToString()
        {
            var stringData =
               $"{nameof(G_HIERARCHY_ID)} = {G_HIERARCHY_ID}, " +
               $"{nameof(ZREP)} = {ZREP}, " +
               $"{nameof(PRICE)} = {PRICE}, " +
               $"{nameof(START_DATE)} = {START_DATE}, " +
               $"{nameof(FINISH_DATE)} = {FINISH_DATE}, " +
               $"{nameof(UNIT_OF_MEASURE)} = {UNIT_OF_MEASURE}, " +
               $"{nameof(CURRENCY)} = {CURRENCY}";

            return stringData;
        }
    }
}
