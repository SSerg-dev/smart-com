using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Utils
{
    /// <summary>
    /// Вспомогательный класс для манипуляций с временными зонами
    /// </summary>
    
    public static class ChangeTimeZoneUtil
    {
        // по дефолту мы считаем что всё происходит в Москве UTC +03:00
        private static byte defaultTimeZone = 3;

        /// <summary>
        /// Изменить только часовой пояс без смещения времени
        /// Example 01.09.2019 00:00:00 +02:00 => 01.09.2019 00:00:00 +03:00
        /// </summary>
        /// <param name="original">Время, требуемое корректировки</param>
        public static DateTimeOffset? ResetTimeZone(DateTimeOffset? original)
        {
            return original.HasValue ? new DateTimeOffset(original.Value.DateTime, TimeSpan.FromHours(defaultTimeZone)) : original;
        }

        /// <summary>
        /// Изменить только часовой пояс без смещения времени
        /// Example 01.09.2019 00:00:00 +02:00 => 01.09.2019 00:00:00 +03:00
        /// </summary>
        /// <param name="original">Время, требуемое корректировки</param>
        /// <param name="timeZone">Требуемая временная зона</param>
        public static DateTimeOffset? ResetTimeZone(DateTimeOffset? original, byte timeZone)
        {
            return original.HasValue ? new DateTimeOffset(original.Value.DateTime, TimeSpan.FromHours(timeZone)) : original;
        }

        /// <summary>
        /// Изменить часовой пояс
        /// Example 01.09.2019 00:00:00 +02:00 => 01.09.2019 01:00:00 +03:00
        /// </summary>
        /// <param name="original">Время, требуемое корректировки</param>
        public static DateTimeOffset? ChangeTimeZone(DateTimeOffset? original)
        {
            return original.HasValue ? original.Value.ToOffset(TimeSpan.FromHours(defaultTimeZone)) : original;
        }

        /// <summary>
        /// Изменить часовой пояс
        /// Example 01.09.2019 00:00:00 +02:00 => 01.09.2019 01:00:00 +03:00
        /// </summary>
        /// <param name="original">Время, требуемое корректировки</param>
        /// <param name="timeZone">Требуемая временная зона</param>
        public static DateTimeOffset? ChangeTimeZone(DateTimeOffset? original, byte timeZone)
        {
            return original.HasValue ? original.Value.ToOffset(TimeSpan.FromHours(timeZone)) : original;
        }
    }

    // PS: зачем этот класс? Делаем вид что клиенты в +3, но ExtJs вообще не хочет говорить о часовом поясе при отправке данных :(
}
