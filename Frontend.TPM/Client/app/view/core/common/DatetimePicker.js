Ext.define('App.view.core.common.DatetimePicker', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.datetimepicker',
    cls: 'date-time-picker',
    resizable: false,
    showTimeZone: false,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    items: [{
        xtype: 'timefield',
        format: 'H:i',
        listeners: {
            collapse: function (field, eOpts) {
                field.focus();
            },
            expand: function (field, eOpts) {
                field.focus();
            }
        }
    }, {
        xtype: 'datepicker',
        showToday: false,
        border: '1 0 0 0'
    }],

    buttons: [{
        glyph: 0xf0f6,
        maxWidth: 32,
        padding: 7,
        tooltip: l10n.ns('core').value('datetimePickerNowBtnText'),
        action: 'now'
    }, {
        text: l10n.ns('core', 'buttons').value('cancel'),
        action: 'cancel'
    }, {
        text: l10n.ns('core', 'buttons').value('ok'),
        ui: 'green-button-footer-toolbar',
        action: 'ok'
    }],

    constructor: function () {
        this.callParent(arguments);
        this.addEvents('select');
    },

    initComponent: function () {
        var me = this;
        var time = me.items[0];
        if (time) {
            time.increment = me.increment
        }
        me.callParent(arguments);

        me.timeField = me.down('timefield');
        me.datePicker = me.down('datepicker');
        me.cancelButton = me.down('button[action=cancel]');
        me.selectButton = me.down('button[action=ok]');
        me.nowButton = me.down('button[action=now]');
    },

    initEvents: function () {
        this.callParent(arguments);

        this.cancelButton.on('click', this.onCancelButtonClick, this);
        this.selectButton.on('click', this.onSelectButtonClick, this);
        this.nowButton.on('click', this.onNowButtonClick, this);
    },

    initKeyNav: function () {
        var datepicker = this.datePicker,
            timepicker = this.timeField,
            button = this.selectButton;

        Ext.defer(function () {
            timepicker.focus();
        }, 100);

        // кеймап для дэйтпикера
        datepicker.keyNav = new Ext.util.KeyNav(datepicker.eventEl, Ext.apply({
            scope: datepicker,
            enter: function (e) {
                if (e.ctrlKey) {
                    button.fireEvent('click', button);
                } else {
                    timepicker.focus();
                }
            }
        }, datepicker.keyNavConfig));

        // кеймап для таймпикера
        new Ext.KeyMap(timepicker.getEl().dom, [{
            key: 13, // enter
            fn: function () {
                datepicker.focus();
            }
        }, {
            key: 13,
            ctrl: true,
            fn: function () {
                button.fireEvent('click', button);
            }
        }]);

        // внутренний элемент таймпикера
        new Ext.KeyMap(timepicker.inputEl, [{
            key: 13, // enter
            fn: function () {
                datepicker.focus();
            }
        }, {
            key: 13,
            ctrl: true,
            fn: function () {
                button.fireEvent('click', button);
            }
        }]);
    },

    onCancelButtonClick: function (btn) {
        this.close();
    },

    onSelectButtonClick: function (btn) {
        this.updateData();
        this.fireEvent('select', this, this.getValue());
    },

    onNowButtonClick: function (btn) {
        this.setValue(new Date());
    },

    afterRender: function () {
        this.callParent(arguments);
        this.updateView();
        this.initKeyNav();
    },

    setValue: function (value) {
        if (!value) {
            value = new Date();
        } else {
            value = new Date(value);
        }

        this.value = value;
        this.updateView();
    },

    getValue: function () {
        return this.value;
    },

    updateView: function () {
        var value = this.getValue();
        this.datePicker.setValue(value);
        this.timeField.setValue(value);
    },

    updateData: function () {
        if (!this.value) {
            this.value = new Date();
        }

        var newTime = this.timeField.getValue(),
            newDate = this.datePicker.getValue();

        this.value = new Date(
            newDate.getFullYear(),
            newDate.getMonth(),
            newDate.getDate(),
            newTime.getHours(),
            newTime.getMinutes(),
            newTime.getSeconds()
        );
    }

});
