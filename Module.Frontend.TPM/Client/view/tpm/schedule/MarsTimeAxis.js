Ext.define('App.view.tpm.schedule.MarsTimeAxis', {
    extend: "Sch.data.TimeAxis",
    continuous: false,
    marsMode: true,
    autoAdjust: false,

    isMarsMode: function () {
        return this.marsMode;
    },

    setMarsMode: function (value) {
        this.marsMode = value;
    },

    generateTicks: function (start, end, unit, increment) {
        if (this.isMarsMode()) {
            // Use our own custom time intervals for MONTH time-axis
            if (unit === Sch.util.Date.MONTH) {
                var ticks = [],
                    intervalEnd;

                while (start < end) {
                    var marsDate = new App.MarsDate(start);
                    intervalEnd = marsDate.getPeriodEndDate();
                    ticks.push({
                        start: start,
                        end: intervalEnd
                    });
                    start = Sch.util.Date.add(intervalEnd, Sch.util.Date.DAY, 1);
                }
                return ticks;
            } else if (unit === Sch.util.Date.WEEK) {
                var ticks = [],
                intervalEnd;
                while (start < end) {
                    var marsDate = new App.MarsDate(start);
                    intervalEnd = marsDate.getWeekEndDate();
                    ticks.push({
                        start: start,
                        end: intervalEnd
                    });
                    start = Sch.util.Date.add(intervalEnd, Sch.util.Date.DAY, 1);
                }
                return ticks;
            } else {
                return this.callParent(arguments);
            }
        } else {
            return this.callParent(arguments);
        }
    }
});