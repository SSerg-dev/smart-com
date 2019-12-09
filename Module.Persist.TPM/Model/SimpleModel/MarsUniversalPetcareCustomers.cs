using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.SimpleModel
{
    public class MarsUniversalPetcareCustomers
    {
        public string ZCUSTHG01 { get;set;}
        public string ZCUSTHG01___T { get;set;}
        public string ZCUSTHG02 { get;set;}
        public string ZCUSTHG02___T { get;set;}
        public string ZCUSTHG03 { get;set;}
        public string ZCUSTHG03___T { get;set;}
        public string ZCUSTHG04 { get;set;}
        public string ZCUSTHG04___T { get;set;}
        public DateTime Active_From { get;set;}
        public DateTime Active_Till { get;set;}
        public string G_H_ParentID { get;set;}
        public string G_H_level { get;set;}
        public string SoldToPoint { get;set;}
        public string SP_Description { get; set; }
    }
}
