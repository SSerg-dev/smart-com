// ------------------------------------------------------------------------
// <copyright file="MarsDate.js" company="Смарт-Ком">
//     Copyright statement. All right reserved
// </copyright>
// Проект Transportation Workflow System
// Версия 1.0
// Автор: Сергей Токарев (EMail: sergey.tokarev@smart-com.su)
// ------------------------------------------------------------------------

Ext.ns('App');
App.MarsDate = (function () {
    var
        // Разница между 3 янв 1993 (по Mars-системе (1993,1,1,1) - точка отсчета
        // Mars-календаря) и 1 янв 1970, представленная в миллисекундах.
        //STARTING_DATE = 726004800000,
        STARTING_DATE = Date.UTC(1993, 0, 3),//726004800000,

        STARTING_YEAR = 1993, // Год начала Mars-календаря.

        FIRST_LONG_YEAR = 5, // Индекс первого длинного года.
        SECOND_LONG_YEAR = 10, // Индекс второго длинного года.
        THIRD_LONG_YEAR = 16, // Индекс третьего длинного года.

        FIRST_LONG_PERIOD = 77, // Индекс первого длинного периода.
        SECOND_LONG_PERIOD = 142, // Индекс второго длинного периода.
        THIRD_LONG_PERIOD = 220, // Индекс третьего длинного периода.

        FIRST_EXTRA_WEEK = 312, // Индекс первой дополнительной недели.
        SECOND_EXTRA_WEEK = 573, // Индекс второй дополнительной недели.
        THIRD_EXTRA_WEEK = 886, // Индекс третьей дополнительной недели.

        MSECS_PER_DAY = 86400000, // Количество миллисекунд в одном дне.

        DAYS_PER_WEEK = 7, // Количество дней в неделе.

        DAYS_PER_SHORT_PERIOD = 28, // Количество дней в коротком периоде.
        WEEKS_PER_SHORT_PERIOD = 4, // Количество недель в коротком периоде.
        WEEKS_PER_LONG_PERIOD = 5, // Количество недель в длинном периоде.

        DAYS_PER_SHORT_YEAR = 364, // Количество дней в малом году.
        PERIODS_PER_YEAR = 13, // Количество периодов в году.

        DAYS_PER_CYCLE = 6209, // Количество дней в 17-летнем цикле.
        WEEKS_PER_CYCLE = 887, // Количество недель в 17-летнем цикле.
        PERIODS_PER_CYCLE = 221, // Количество периодов в 17-летнем цикле.
        YEARS_PER_CYCLE = 17; // Количество лет в 17-летнем цикле.

        TICKS_PER_MILLISECOND = 10000; // Количество тиков в миллисекунде
    // Конструктор.
    // Представляет Mars-дату.
    //
    // Текущая Mars-дата:
    // var marsDate = new Application.util.MarsDate();
    //
    // Создание Mars-даты на основе количества миллисекунд прошедших с 03.01.1993:
    // var marsDate = new Application.util.MarsDate(655455190451);
    //
    // Создание Mars-даты на основе Григорианской даты:
    // var marsDate = new Application.util.MarsDate(new Date(2015, 0, 1));
    //
    // Создание Mars-даты на основе отдельных компонент:
    // var marsDate = new Application.util.MarsDate(2015, 1);
    // var marsDate = new Application.util.MarsDate(2015, 1, 1);
    // var marsDate = new Application.util.MarsDate(2015, 1, 1, 1);
    function MarsDate() {
        this.ticks = null; // Значение Mars-даты, представленное в миллисекундах.
        this.format = null; // Формат Mars-даты.

        if (arguments.length > 1) {
            var year = arguments[0] || 0,
                period = arguments[1] || 0,
                week = arguments[2] || 0,
                day = arguments[3] || 0;

            this.format = formatDetermination(year, period, week, day);
            this.ticks = initFromComponent(year, period, week, day);
        } else if (arguments.length === 1) {
            var arg = arguments[0] || 0;

            if (Ext.isNumber(arg)) {
                this.format = MarsDate.MD_FORMAT_PWD;
                this.ticks = arg > 0 ? arg || 0 : null;
            } else if (Ext.isDate(arg)) {
                this.format = MarsDate.MD_FORMAT_PWD;
                this.ticks = initFromDate(arg);
            }

        } else {
            this.format = MarsDate.MD_FORMAT_PWD;
            this.ticks = initFromDate(new Date());
        }
    }

    // Static
    Ext.apply(MarsDate, {

        // Регулярное выражение для разбора строки, представляющей Mars-дату.
        MD_FORMAT_REGEX: /^(\d{4}) *P(\d{1,2}) *(W(\d{1}) *(D(\d{1}))?)?$/,

        MD_FORMAT_P: 1, // Название формата для Mars-даты вида "1993 P1".
        MD_FORMAT_PW: 2, // Название формата для Mars-даты вида "1993 P1W1".
        MD_FORMAT_PWD: 3, // Название формата для Mars-даты вида "1993 P1W1D1".

        MD_FORMAT_TEMPLATES: [
            'Invalid MarsDate',
            '{0}P{1}', // Year + Period
            '{0}P{1}W{2}', // Year + Period + Week
            '{0}P{1}W{2}D{3}' // Year + Period + Week + Day
        ],

        // Создание Mars-даты на основе строкового представления:
        // var marsDate = App.MarsDate.parse('2015 P1');
        parse: function (value, autoCorrect) {
            var m = (value || '').match(MarsDate.MD_FORMAT_REGEX);

            if (autoCorrect) {
                console.warn('The autocorrect function does not implemented yet');
            }

            if (m) {
                var year = Number(m[1]) || 0,
                    period = Number(m[2]) || 0,
                    week = Number(m[4]) || 0,
                    day = Number(m[6]) || 0;

                return MarsDate.isComponentsValid(year, period, week, day)
                    ? new MarsDate(year, period, week, day)
                    : null;
            }

            return null;
        },

        isComponentsValid: function (year, period, week, day) {
            if (year < STARTING_YEAR) {
                return false;
            }

            if (period < 1 || period > PERIODS_PER_YEAR) {
                return false;
            }

            if (!week) {
                return true;
            }

            var isLongPeriod = isLongYear(year) && period === 13;

            if (week < 1 || week > (isLongPeriod ? WEEKS_PER_LONG_PERIOD : WEEKS_PER_SHORT_PERIOD)) {
                return false;
            }

            if (!day) {
                return true;
            }

            if (day < 1 || day > DAYS_PER_WEEK) {
                return false;
            }

            return true;
        }

    });

    // Public
    Ext.apply(MarsDate.prototype, {

        // Возвращает строковое представление Mars-даты.
        toString: function () {
            if (Ext.isArray(MarsDate.MD_FORMAT_TEMPLATES)) {
                var template = MarsDate.MD_FORMAT_TEMPLATES[this.getFormat() || 0].toString(),
                    yearString = Ext.String.leftPad(this.getYear(), 4, '0'),
                    periodString = Ext.String.leftPad(this.getPeriod(), 2, '0'),
                    weekString = this.getWeek(),
                    dayString = this.getDay();

                return Ext.String.format(template, yearString, periodString, weekString, dayString);
            } else if (Ext.isFunction(MarsDate.MD_FORMAT_TEMPLATES)) {
                return MarsDate.MD_FORMAT_TEMPLATES.call(this);
            }

            return Object.prototype.toString.apply(this, arguments);
        },

        // Возвращает Григорианскую дату, 
        // соответстующую текущей Mars-дате.
        toDate: function () {
            var sd = new Date(STARTING_DATE);
            return Ext.Date.add(sd, Ext.Date.MILLI, this.ticks);
        },

        // Возвращает формат Mars-даты.
        getFormat: function () {
            return this.format;
        },

        // Возвращает год.
        getYear: function () {
            return STARTING_YEAR + this.getPeriods() / PERIODS_PER_YEAR | 0;
        },

        // Возвращает номер периода в году.
        getPeriod: function () {
            return this.getPeriods() % PERIODS_PER_YEAR + 1;
        },

        // Возвращает номер недели в периоде.
        getWeek: function () {
            var wIdx = this.getWeekIndex();
            if (wIdx === FIRST_EXTRA_WEEK
                || wIdx === SECOND_EXTRA_WEEK
                || wIdx === THIRD_EXTRA_WEEK) {
                return WEEKS_PER_LONG_PERIOD;
            } else {
                return (wIdx - this.getExtraWeeks(true)) % WEEKS_PER_SHORT_PERIOD + 1;
            }
        },

        // Возвращает номер дня в неделе.
        getDay: function () {
            return this.getDays() % DAYS_PER_WEEK + 1;
        },

        // Возвращает количество недель в текущем периоде.
        getWeeksInPeriod: function () {
            var pIdx = this.getPeriods() % PERIODS_PER_CYCLE;
            return (pIdx === FIRST_LONG_PERIOD
                || pIdx === SECOND_LONG_PERIOD
                || pIdx === THIRD_LONG_PERIOD) ? WEEKS_PER_LONG_PERIOD : WEEKS_PER_SHORT_PERIOD;
        },

        // Возвращает Григорианскую дату, 
        // соответствующую началу текущего периода.
        getPeriodStartDate: function () {
            var sd = new Date(STARTING_DATE),
                days = this.getPeriods() * DAYS_PER_SHORT_PERIOD
                + this.getCycles() * 3 * DAYS_PER_WEEK
                + this.getExtraWeeks() * DAYS_PER_WEEK;

            return Ext.Date.add(sd, Ext.Date.DAY, days);
        },

        // Возвращает Григорианскую дату, 
        // соответствующую концу текущего периода.
        getPeriodEndDate: function () {
            var sd = new Date(STARTING_DATE),
                days = this.getPeriods() * DAYS_PER_SHORT_PERIOD
                + this.getCycles() * 3 * DAYS_PER_WEEK
                + this.getExtraWeeks() * DAYS_PER_WEEK
                + this.getWeeksInPeriod() * DAYS_PER_WEEK - 1;

            return Ext.Date.add(sd, Ext.Date.MILLI, (days + 1) * MSECS_PER_DAY - 1);
        },

        // Возвращает Григорианскую дату, 
        // соответствующую началу текущей недели.
        getWeekStartDate: function () {
            var sd = new Date(STARTING_DATE),
                days = this.getWeeks() * DAYS_PER_WEEK;

            return Ext.Date.add(sd, Ext.Date.DAY, days);
        },

        // Возвращает Григорианскую дату, 
        // соответствующую концу текущей недели.
        getWeekEndDate: function () {
            var sd = new Date(STARTING_DATE),
                days = (this.getWeeks() + 1) * DAYS_PER_WEEK - 1;

            return Ext.Date.add(sd, Ext.Date.MILLI, (days + 1) * MSECS_PER_DAY - 1);
        },

        // Возвращает Григорианскую дату, 
        // соответствующую началу текущего дня.
        getDayStartDate: function () {
            return this.toDate();
        },

        // Возвращает Григорианскую дату, 
        // соответствующую концу текущего дня.
        getDayEndDate: function () {
            var sd = new Date(STARTING_DATE);
            return Ext.Date.add(sd, Ext.Date.MILLI, this.ticks + MSECS_PER_DAY - 1);
        },

        // Возвращает Григорианскую дату, 
        // соответствующую началу диапазона в
        // зависимости от формата Mars-даты.
        getStartDate: function () {
            return this.toDate();
        },

        // Возвращает Григорианскую дату, 
        // соответствующую концу диапазона в
        // зависимости от формата Mars-даты.
        getEndDate: function () {
            switch (this.format) {
                case MarsDate.MD_FORMAT_P: return this.getPeriodEndDate();
                case MarsDate.MD_FORMAT_PW: return this.getWeekEndDate();
                case MarsDate.MD_FORMAT_PWD: return this.getDayEndDate();
            }
        },

        // Вычисляет индекс недели в 17 летнем цикле.
        getWeekIndex: function () {
            return this.getWeeks() % WEEKS_PER_CYCLE;
        },

        // Вычисляет количество дополнительных недель
        // в промежутке от конца последнего полного цикла
        // до текущей даты.
        //
        // При includeCurrentWeek = true - будет учитываться
        // текущая неделя если она является дополнительной.
        getExtraWeeks: function (includeCurrentWeek) {
            var wIdx = this.getWeekIndex();

            if (includeCurrentWeek || false) {
                return 0 + (wIdx >= FIRST_EXTRA_WEEK) +
                    (wIdx >= SECOND_EXTRA_WEEK) +
                    (wIdx >= THIRD_EXTRA_WEEK);
            } else {
                return 0 + (wIdx > FIRST_EXTRA_WEEK) +
                    (wIdx > SECOND_EXTRA_WEEK)
            }
        },

        // Количество дней прошедших с 3 янв 1993.
        getDays: function () {
            return this.ticks / MSECS_PER_DAY | 0;
        },

        // Количество недель прошедших с 3 янв 1993.
        getWeeks: function () {
            return this.getDays() / DAYS_PER_WEEK | 0;
        },

        // Количество периодов прошедших с 3 янв 1993.
        getPeriods: function () {
            var wIdx = this.getWeekIndex();

            return this.getCycles() * PERIODS_PER_CYCLE
                + (wIdx - this.getExtraWeeks(true)) / WEEKS_PER_SHORT_PERIOD | 0;
        },

        // Количество циклов прошедших с 3 янв 1993.
        getCycles: function () {
            return this.getDays() / DAYS_PER_CYCLE | 0;
        },

        
        // Получить Mars-дату, большую на заданное количество дней
        addDays: function(days) {
            var t = this.ticks + (days * MSECS_PER_DAY);
            return new MarsDate(t);
        },

        // Получить Mars-дату, большую на заданное количество недель
        addWeeks: function(weeks) {
            var t = this.ticks + (weeks * DAYS_PER_WEEK * MSECS_PER_DAY);
            return new MarsDate(t);
        },

        setFormat: function (format) {
            this.format = format;
            return this;
        }

    });

    // Private

    // Определяет формат Mars-даты.
    function formatDetermination(year, period, week, day) {
        if (year < STARTING_YEAR) {
            console.warn('Год должен быть больше ' + STARTING_YEAR);
            return null;
        }

        if (period === 0) {
            console.warn('Период не указан');
            return null;
        }

        if (week === 0) {
            return MarsDate.MD_FORMAT_P;
        }

        if (day === 0) {
            return MarsDate.MD_FORMAT_PW;
        }

        return MarsDate.MD_FORMAT_PWD;
    }

    // Преобразует Mars-дату, представленную в виде отдельных
    // компонент, в миллисекунды.
    function initFromComponent(year, period, week, day) {
        year = year || STARTING_YEAR;
        period = period || 1;
        week = week || 1;
        day = day || 1;

        if (year < STARTING_YEAR) {
            console.warn('Год должен быть больше ' + STARTING_YEAR);
            return null;
        }

        var yIdx = getYearIndex(year),
            extraWeeks = 0 + (yIdx > FIRST_LONG_YEAR) + (yIdx > SECOND_LONG_YEAR),
            days = (day - 1)
                + (week - 1) * DAYS_PER_WEEK + extraWeeks * DAYS_PER_WEEK
                + ((period - 1) * DAYS_PER_SHORT_PERIOD)
                + yIdx * DAYS_PER_SHORT_YEAR
                + (((year - STARTING_YEAR) / YEARS_PER_CYCLE) | 0) * DAYS_PER_CYCLE;

        return days * MSECS_PER_DAY;
    }

    // Преобразует объект класса Date в Mars-дату, 
    // представвленную в миллисекундах.
    function initFromDate(date) {
        var sd = new Date(STARTING_DATE);

        if (date < sd) {
            console.warn('Дата должна быть больше или равна 3 янв 1993');
            return null;
        }

        return Ext.Date.clearTime(date, true).getTime() - sd.getTime();
    }

    // Проверяет наличие дополнительной недели
    // в 13 периоде указанного года.
    function isLongYear(year) {
        var yIdx = getYearIndex(year);

        return yIdx === FIRST_LONG_YEAR
            || yIdx === SECOND_LONG_YEAR
            || yIdx === THIRD_LONG_YEAR;
    }

    // Вычисляет индекс года в 17-летнем цикле.
    function getYearIndex(year) {
        return (year - STARTING_YEAR) % YEARS_PER_CYCLE;
    }

    // Export MarsDate constructor.
    return MarsDate;

})();