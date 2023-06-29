using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.DTO
{
    public class JobFlagView
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Prefix { get; set; }
        public byte Value { get; set; }
        public string Description { get; set; }
    }
}
