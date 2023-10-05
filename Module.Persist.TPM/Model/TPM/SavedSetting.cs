using Core.Data;
using Persist.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.TPM
{
    public class SavedSetting : IEntity<Guid>
    {
        /// <summary>
        /// Модель пользовательских настроек 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Ключ настройки вида widgetname#modename
        /// </summary>
        [Required]
        [MaxLength(512)]
        public string Key { get; set; }

        /// <summary>
        /// Конфиг в формате json
        /// </summary>
        [Required]
        public string Value { get; set; }

        /// <summary>
        /// Идентификатор связки пользователя и роли
        /// </summary>
        public Guid UserRoleId { get; set; }

        /// <summary>
        /// Ссылка на связку пользователя и роли
        /// </summary>
        public virtual UserRole UserRole { get; set; }
    }
}
