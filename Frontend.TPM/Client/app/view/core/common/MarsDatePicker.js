// ------------------------------------------------------------------------
// <copyright file="MarsDatePicker.js" company="Смарт-Ком">
//     Copyright statement. All right reserved
// </copyright>
// Проект Transportation Workflow System
// Версия 1.0
// Автор: Сергей Токарев (EMail: sergey.tokarev@smart-com.su)
// ------------------------------------------------------------------------

Ext.define('App.view.core.common.MarsDatePicker', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.marsdatepicker',

    title: l10n.ns('core', 'marsCalendar').value('title'),
    resizable: false,

    items: [{
        xtype: 'panel',
        frame: true,
        ui: 'light-gray-panel',
        layout: 'fit',
        bodyPadding: '30 30 30 0',
        margin: '10 8 15 15',

        layout: {
            type: 'hbox',
            pack: 'center',
            align: 'middle'
        },

        defaults: {
            margin: '0 0 0 5',
            width: 90,
            displayField: 'data',
            valueField: 'id',
            queryMode: 'local',
            editable: false
        },

        items: [{
            xtype: 'label',
            margin: '0 0 0 10',
            width: null,
            forId: 'nfYear',
            text: l10n.ns('core', 'marsCalendar').value('year')
        }, {
            xtype: 'numberfield',
            itemId: 'nfYear',
            allowBlank: false,
            allowDecimals: false,
            allowExponential: false,
            allowOnlyWhitespace: false,
            maxValue: 9999,
            minValue: 1993
        }, {
            xtype: 'label',
            margin: '0 0 0 10',
            width: null,
            forId: 'cbPeriod',
            text: l10n.ns('core', 'marsCalendar').value('period')
        }, {
            xtype: 'combobox',
            itemId: 'cbPeriod'
        }, {
            xtype: 'label',
            margin: '0 0 0 10',
            width: null,
            forId: 'cbWeek',
            text: l10n.ns('core', 'marsCalendar').value('week')
        }, {
            xtype: 'combobox',
            itemId: 'cbWeek'
        }]

    }],

    buttons: [{
        text: l10n.ns('core', 'marsCalendar', 'buttons').value('cancel'),
        handler: function () {
            this.up('window').close();
        }
    }, {
        text: l10n.ns('core', 'marsCalendar', 'buttons').value('ok'),
        ui: 'green-button-footer-toolbar',
        handler: function () {
            var picker = this.up('window');
            picker.fireEvent('select', picker, picker.getValue());
            picker.close();
        }
    }],

    onYearBlur: function (field, ev, options) {
        this.updateGregorianDate();
    },

    onYearChange: function (field, newValue, oldValue, options) {
        this.updateGregorianDate();
    },

    onYearSelect: function (combo, records, options) {
        this.updateGregorianDate();
    },

    onPeriodSelect: function (combo, records, options) {
        this.updateGregorianDate();
    },

    onWeekSelect: function (combo, records, options) {
        var isDisable = records[0].getId() === 0;
        this.updateGregorianDate();
    },

    onDaySelect: function (combo, records, options) {
        this.updateGregorianDate();
    },

    onDateSelect: function (datemenu, date, options) {
        this.updateMarsDate(date);
    },

    setValue: function (value) {
        if (Ext.isString(value)) {
            var marsDate;
            marsDate = App.MarsDate.parse(value) || new App.MarsDate();
            this.setMarsDate(marsDate);
        } else if (value instanceof App.MarsDate) {
            this.setMarsDate(value);
        } else {
            this.setMarsDate(new App.MarsDate());
        }
    },

    getValue: function () {
        var y = this.nfYear.getValue(),
            p = this.cbPeriod.getValue(),
            w = this.cbWeek.getValue();

        return new App.MarsDate(y, p, w);
    },

    getEmptyValue: function() {
        return this.weekRequired ? null : '-';
    },

    updateGregorianDate: function () {
        var marsDate = this.getValue();

        this.loadStore(this.cbWeek, 1, marsDate.getWeeksInPeriod(), 0, this.getEmptyValue());
    },

    updateMarsDate: function (date) {
        var md = new App.MarsDate(date);
        this.setMarsDate(md);
    },

    setMarsDate: function (marsDate) {
        var y = marsDate.getYear(),
            p = marsDate.getPeriod(),
            w = marsDate.getWeek(),
            d = marsDate.getDay(),
            fmt = marsDate.getFormat();

        this.loadStore(this.cbWeek, 1, marsDate.getWeeksInPeriod(), 0, this.getEmptyValue());
        this.nfYear.suspendEvents();
        this.nfYear.setValue(y);
        this.nfYear.resumeEvents();

        switch (fmt) {
            case App.MarsDate.MD_FORMAT_PWD:
            case App.MarsDate.MD_FORMAT_PW:
                this.cbWeek.select(w);
            case App.MarsDate.MD_FORMAT_P:
                this.cbPeriod.select(p);
        }

        switch (fmt) {
            case App.MarsDate.MD_FORMAT_P:
                this.cbWeek.select(0);
        }
    },

    loadStore: function (combo, from, to, pad, emptyValue) {
        var store = createStore(from, to, pad, emptyValue),
            oldValue = +combo.getValue();

        function createStore(from, to, pad, emptyValue) {
            var store = null,
                data = emptyValue ? [{ id: 0, data: emptyValue }] : [];

            for (var i = from; i <= to; i++) {
                data.push({
                    id: i,
                    data: Ext.String.leftPad(i, pad || 0, '0')
                });
            }

            store = Ext.create('Ext.data.Store', {
                fields: ['id', 'data'],
                data: data
            });

            return store;
        }

        combo.getStore().removeAll();
        combo.bindStore(store);

        if (oldValue > to) {
            combo.suspendEvents();
            combo.select(to);
            combo.resumeEvents();
        }
    },

    initComponent: function () {
        this.callParent(arguments);

        var nfYear = this.nfYear = this.down('#nfYear'),
            cbPeriod = this.cbPeriod = this.down('#cbPeriod'),
            cbWeek = this.cbWeek = this.down('#cbWeek');

        this.loadStore(cbPeriod, 1, 13, 2);
        this.loadStore(cbWeek, 1, 4, 0, this.getEmptyValue());

        nfYear.on('change', this.onYearChange, this);
        nfYear.on('blur', this.onYearBlur, this);
        cbPeriod.on('select', this.onPeriodSelect, this);
        cbWeek.on('select', this.onWeekSelect, this);

        this.addEvents('select');
    }

});