using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class RPA : IEntity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }



        [Column(TypeName = "NVARCHAR(MAX)")]
        [Required]
        public string HandlerName { get; set; }

        public DateTimeOffset? CreateDate { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string UserName { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]        
        public string Constraint { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]        
        public string Parametrs { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]        
        public string Status { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]        
        public string FileURL { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }
        public Guid? HandlerId { get; set; }
    }
}
