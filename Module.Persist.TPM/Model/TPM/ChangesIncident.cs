using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class ChangesIncident : IEntity<Guid> // интерфейс реализуем для экспорта
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string DirectoryName { get; set; }
        public string ItemId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? ProcessDate { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public bool Disabled { get; set; }

        public static ChangesIncident CreateIncident(string directoryName, Guid id)
        {
            return new ChangesIncident
            {
                Id = Guid.NewGuid(),
                DirectoryName = directoryName,
                ItemId = id.ToString(),
                CreateDate = DateTimeOffset.Now,
                ProcessDate = null,
                DeletedDate = null,
                Disabled = false
            };
        }
    }
}
