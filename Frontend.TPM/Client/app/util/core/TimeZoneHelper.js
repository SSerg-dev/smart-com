// ----------------------------------------------------------------------
// <copyright file="TimeZoneHelper.js" company="Смарт-Ком">
//     Copyright statement. All right reserved
// </copyright>
// Проект Transportation Workflow System
// Версия 2.0
// Автор: Максим Молоканов (EMail: maxim.molokanov@smart-com.su)
// ----------------------------------------------------------------------

(function () {
    Ext.ns('App.util.core.');
    // класс для работы с часовыми поясами
    Ext.define('App.util.core.TimeZoneHelper', {
        singleton: true,

        /**
         * Преобразовать строковое представление смещения часового пояса в числовое (в минутах)
         * Например:
         * '+04:00' => 4 * 60
         * '-05:00' => -5 * 60
         */
        offsetStringToInt: function (offsetString) {
            var sign = 1;
            if (offsetString[0] == '-') {
                sign = -1;
            }
            var hours = parseInt(offsetString.substr(1, 2), 10);
            var minutes = parseInt(offsetString.substr(4, 2), 10);
            var result = sign * (hours * 60 + minutes);
            return result;
        },

        /**
         * Конвертация даты из одного часового пояса в другой
         * Указывается:
         * datetime - конвертируемая дата
         * fromTimezoneOffset - смещение относительно UTC исходной даты
         * toTimezoneOffset - смещение относительно UTC в которую нужно сконвертировать
         */
        convertToAnotherTimezone: function (datetime, fromTimezoneOffset, toTimezoneOffset) {
            var result = null;
            if (datetime) {
                var offset = (toTimezoneOffset - fromTimezoneOffset);
                result = Ext.Date.add(datetime, Ext.Date.MINUTE, offset);
                result.offset = offset;
            }
            return result;
        },

        /**
         * Преобразовать название часового пояса в числовое представление смещения часового пояса в минутах
         * Например:
         * '(UTC+04:00) Волгоград, Москва, Санкт-Петербург' => 4 * 60
         * '(GTC-05:00) Восточное время (США и Канада)' => -5 * 60
         */
        timezoneNameToOffsetInt: function (timezoneName) {
            var offsetString = timezoneName.substr(4, 6);
            var offsetInt = this.offsetStringToInt(offsetString);
            return offsetInt;
        },

        /** Преобразовать смещение от UTC в минутах в строку смещения в часах
         * Например: 240 => +04:00 
         */
        offsetToOffsetString: function (offset) {
            var minutes = offset % 60;
            var hours = Math.abs((offset - minutes) / 60);
            var result = ((offset >= 0) ? '+' : '-') +
                Ext.String.leftPad(hours, 2, '0') + ':' +
                Ext.String.leftPad(minutes, 2, '0');
            return result;
        },

        // Получить дату в "UTC", offset - смещение в минутах 
        convertToUtc: function (datetime, offset) {
            if (datetime) {
                return Ext.Date.add(datetime, Ext.Date.MINUTE, -(offset || 0));
                //return new Date(datetime.getTime() - 60000 * (offset || 0));
            } else {
                return datetime;
            }
        },

        // Добавляет к локальной дате указанное смещение
        convertToTimezone: function (localDate, offset) {
            var dateInUtc = new Date(
                localDate.getUTCFullYear(),
                localDate.getUTCMonth(),
                localDate.getUTCDate(),
                localDate.getUTCHours(),
                localDate.getUTCMinutes(),
                localDate.getUTCSeconds(),
                localDate.getUTCMilliseconds()
            );
            var dateWithOffset = Ext.Date.add(dateInUtc, Ext.Date.MINUTE, offset);
            dateWithOffset.offset = offset;
            return dateWithOffset;
        },

        // Получить значение часового пояса поля
        getFieldTimezone: function (model, fieldName) {
            var fieldValue = model.get(fieldName);
            var offset = undefined;
            if (fieldValue && fieldValue instanceof Date) {
                offset = fieldValue.offset;
            }
            return offset;
        },

        dateWithOffsetRenderer: function (value) {
            var res = "";
            if (value) {
                res = Ext.Date.format(value, Ext.Date.defaultFormat + ' H:i');
                if (value.offset !== undefined && value.offset !== null) {
                    var offset = App.util.core.TimeZoneHelper.offsetToOffsetString(value.offset);
                    res += ' (UTC' + offset + ')';
                }
            }
            return res;
        }
    });
}());
