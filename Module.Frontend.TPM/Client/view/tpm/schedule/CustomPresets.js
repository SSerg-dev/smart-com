// Представление Квартал
Sch.preset.Manager.registerPreset('monthQuarter', {
    shiftIncrement: 3,
    shiftUnit: 'MONTH',
    timeColumnWidth: 60,
    timeResolution: {
        unit: 'DAY',
        increment: 1
    },
    headerConfig: {
        bottom: {
            unit: 'MONTH',
            dateFormat: 'M',
            align: 'center',
            renderer: function (start, end, cfg) {
                return start.toLocaleString("en-Us", { month: 'short' });
            }
        },
        middle: {
            unit: 'QUARTER',
            align: 'center',
            renderer: function (start, end, cfg) {
                var quarter = Math.floor(start.getMonth() / 3) + 1;
                return Ext.String.format('Q{0} {1}', quarter, start.getFullYear());
            }
        }
    }
});
// Представление Месяц
Sch.preset.Manager.registerPreset('weekMonth', {
    shiftIncrement: 4,
    shiftUnit: 'WEEK', // если 'MONTH' ломается прокрутка (по shiftNext) !?!
    timeColumnWidth: 60,
    timeResolution: {
        unit: 'DAY',
        increment: 1
    },
    headerConfig: {
        bottom: {
            unit: 'WEEK',
            dateFormat: 'D d M',
            align: 'center',
            renderer: function (start, end, cfg) {
                var onejan = new Date(start.getFullYear(), 0, 1);
                return Math.ceil((((start - onejan) / 86400000) + onejan.getDay() + 1) / 7);
                //return Ext.String.format('{0} {1} {2}', start.toLocaleString("en-Us", { weekday: 'short' }), start.toLocaleString("en-Us", { day: '2-digit' }), start.toLocaleString("en-Us", { month: 'short' }));
            }
        },
        middle: {     // если middle, bottom ломается прокрутка (по shiftNext)
            unit: 'MONTH',
            dateFormat: 'M',
            align: 'center',
            renderer: function (start, end, cfg) {
                return Ext.String.format("{0} {1}", start.toLocaleString("en-Us", { month: 'short' }), start.getFullYear());
            }
        }
    }
});
// Представление Неделя
Sch.preset.Manager.registerPreset('dayWeek', {
    shiftIncrement: 1,
    shiftUnit: 'WEEK',
    timeColumnWidth: 40,
    timeResolution: {
        unit: 'DAY',
        increment: 1
    },
    headerConfig: {
        bottom: {
            unit: 'DAY',
            dateFormat: 'd',
            align: 'center',
        },
        middle: {
            unit: 'WEEK',
            align: 'center',
            renderer: function (start, end, cfg) {
                return Ext.String.format('{0} {1} {2}', start.toLocaleString("en-Us", { weekday: 'short' }), start.toLocaleString("en-Us", { day: '2-digit' }), start.toLocaleString("en-Us", { month: 'short' }));
            }
        }
    }
});
// Представление Марс-Кварталы
Sch.preset.Manager.registerPreset('marsmonthQuarter', {
    shiftIncrement: 3,
    shiftUnit: 'MONTH',
    timeColumnWidth: 60,
    timeResolution: {
        unit: 'WEEK',
        increment: 4
    },
    headerConfig: {
        bottom: {
            unit: 'MONTH',
            dateFormat: 'M',
            align: 'center',
            renderer: function (start, end, cfg) {
                return Ext.String.format('P{0}', new App.MarsDate(start).getPeriod())
            }
        },
        middle: {
            unit: 'QUARTER',
            //генерируем кастомные промежутки для отображения Марс-Кварталов
            cellGenerator: function (viewStart, viewEnd, cfg) {
                var cells = [],
                quarterEnd, start, startPeriod, quarter;
                while (viewStart < viewEnd) {
                    start = new App.MarsDate(viewStart);
                    startPeriod = start.getPeriod();
                    year = viewStart.getFullYear();
                    if (startPeriod < 4) {
                        // 1 период может начинаться в конце предыдущего года 
                        
                        if (startPeriod == 1) {
                            var nextMonthYear = Sch.util.Date.add(viewStart, Sch.util.Date.MONTH, 1).getFullYear();
                            year = nextMonthYear != year ? nextMonthYear : year;
                        }
                        quarterEnd = new App.MarsDate(year, 3).getPeriodEndDate();
                        quarter = 1;
                    } else if (startPeriod > 3 && startPeriod < 7) {
                        quarterEnd = new App.MarsDate(year, 6).getPeriodEndDate();
                        quarter = 2;
                    } else if (startPeriod > 6 && startPeriod < 10) {
                        quarterEnd = new App.MarsDate(year, 9).getPeriodEndDate();
                        quarter = 3;
                    } else if (startPeriod > 9) {
                        quarterEnd = new App.MarsDate(year, 13).getPeriodEndDate();
                        quarter = 4;
                    }
                    cells.push({
                        align: 'center', // нет в документации - свойство для выравнивания
                        start: viewStart,
                        end: quarterEnd,
                        header: Ext.String.format('Q{0} {1}', quarter, year)
                    });
                    viewStart = Sch.util.Date.add(quarterEnd, Sch.util.Date.DAY, 1);
                }
                return cells;
            }
        }
    }
});

// Представление МАРС-периоды
Sch.preset.Manager.registerPreset('marsweekMonth', {
    shiftIncrement: 4,
    shiftUnit: 'WEEK', // если 'MONTH' ломается прокрутка (по shiftNext) !?!
    timeColumnWidth: 60,
    timeResolution: {
        unit: 'WEEK',
        increment: 1
    },
    headerConfig: {
        middle: {     // если middle, bottom ломается прокрутка (по shiftNext)
            unit: 'MONTH',
            cellGenerator: function (viewStart, viewEnd) {
                var cells = [],
                    intervalEnd;

                while (viewStart < viewEnd) {
                    var marsDate = new App.MarsDate(viewStart);

                    intervalEnd = marsDate.getPeriodEndDate();
                    cells.push({
                        align: 'center', // нет в документации - свойство для выравнивания
                        start: viewStart,
                        end: intervalEnd,
                        header: Ext.String.format('{0} P{1}',marsDate.getYear(), marsDate.getPeriod())
                    });
                    viewStart = Sch.util.Date.add(intervalEnd, Sch.util.Date.DAY, 1);
                }
                return cells;
            }
        },
        bottom: {
            unit: 'WEEK',
            align: 'center',
            renderer: function (start, end, cfg) {
                return Ext.String.format('W{0}', new App.MarsDate(start).getWeek())
            }
        }
    }
});

// Представление МАРС-неделя
Sch.preset.Manager.registerPreset('marsdayWeek', {
    shiftIncrement: 1,
    shiftUnit: 'WEEK',
    timeColumnWidth: 40,
    timeResolution: {
        unit: 'DAY',
        increment: 1
    },
    headerConfig: {
        bottom: {
            unit: 'DAY',
            dateFormat: 'd',
            align: 'center',
            renderer: function (start, end, cfg) {
                var marsDate = new App.MarsDate(start);
                var day = marsDate.getDay();
                day = day == 7 ? 1 : day + 1;
                return Ext.String.format('D{0}', day)
            }
        },
        middle: {
            unit: 'WEEK',
            align: 'center',
            cellGenerator: function (viewStart, viewEnd) {
                var cells = [],
                    intervalEnd;
                var isFirst = true; // первую неделю начинаем как есть, остальные с начала марс-недели.
                while (viewStart < viewEnd) {
                    var marsDate = new App.MarsDate(viewStart);
                    intervalEnd = marsDate.getWeekEndDate();
                    cells.push({
                        align: 'center',
                        start: isFirst ? viewStart : Sch.util.Date.add(marsDate.getWeekStartDate(), Sch.util.Date.HOUR, -3),
                        end: Sch.util.Date.add(intervalEnd, Sch.util.Date.HOUR, -3), // getWeekEndDate и getEndDate возвращают разное время - разница 3 часа, видимо часовой пояс
                        header: Ext.String.format('{0} P{1} W{2}', marsDate.getYear(), marsDate.getPeriod(), marsDate.getWeek())
                    });
                    isFirst = false;
                    viewStart = Sch.util.Date.add(intervalEnd, Sch.util.Date.DAY, 1); // Если присвоить weekStartDate следующей недели, то в следующей итерации получим marsDate = прошлой неделе (
                }
                return cells;
            }
        }
    }
});