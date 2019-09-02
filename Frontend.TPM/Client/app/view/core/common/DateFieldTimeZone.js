Ext.define('App.view.core.common.DateFieldTimeZone', {
    extend: 'Ext.form.field.Date',
    alias: 'widget.datefieldtimezone',

    timeZone: null,
    editable: false,
    dateFormat: 'd.m.Y',
    submitFormat: 'Y-m-d',
    //increment: 15,

    selectorTitle: l10n.ns('core').value('datetimePickerTitle'),

    onExpand: function () {
        if (this.value) {
            var picker = this.getPicker();
            picker.setValue(this.value);
        }
        else {
            this.callParent(arguments);
        }
    },

    setValue: function (value) {
        if (value && value.getHours() !== 0) {
            var currentTimeZone = value.getTimezoneOffset() / -60;
            var hour = this.timeZone - currentTimeZone;
            var date = new Date(value);

            date.setHours(date.getHours() + hour);
            value = date;
        }

        this.callParent(arguments);
    },

    getValue: function () {
        var result = this.value;

        if (this.value) {
            var currentTimeZone = this.value.getTimezoneOffset() / -60;
            var hour = this.timeZone - currentTimeZone;
            var date = new Date(this.value.getFullYear(), this.value.getMonth(), this.value.getDate(), 0, 0, 0);

            date.setHours(date.getHours() - hour);
            result = date;
        }

        return result;
    },
});
