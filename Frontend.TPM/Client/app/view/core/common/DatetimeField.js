Ext.define('App.view.core.common.DatetimeField', {
    extend: 'Ext.form.field.Trigger',
    alias: 'widget.datetimefield',

    trigger1Cls: 'x-form-timenow-trigger',
    trigger2Cls: 'x-form-date-trigger',
    //trigger3Cls: 'x-form-clear-trigger',

    editable: false,
    showClearBtn: true,
    dateFormat: 'd/m/Y H:i',
    increment: 15,

    selectorTitle: l10n.ns('core').value('datetimePickerTitle'),
    selectorWidth: undefined,
    selectorHeight: undefined,

    onTrigger1Click: function () {
        this.setValue(new Date());
    },

    onTrigger2Click: function () {
        this.createPicker().show();
        this.inputEl.focus();
    },

    onTrigger3Click: function () {
        if (this.showClearBtn) {
            this.setValue(null);
        }
    },

    valueToRaw: function (value) {
        return Ext.Date.format(value, this.dateFormat);
    },

    rawToValue: function () {
        return this.value;
    },

    createPicker: function () {
        return this.picker = Ext.widget('datetimepicker', {
            pickerField: this,
            title: this.selectorTitle,
            width: this.selectorWidth,
            height: this.selectorHeight,
            value: this.getValue() || new Date(),
            listeners: {
                scope: this,
                select: this.onSelect
            },
            increment: this.increment
        });
    },

    onSelect: function (picker, datetime) {
        this.setValue(datetime);
        this.fireEvent('select', this, datetime);
        picker.close();
        this.focus(true, 10);
    },

    initComponent: function () {
        this.addEvents('select');

        if (this.showClearBtn) {
            this.trigger3Cls = 'x-form-clear-trigger';
        }

        this.callParent(arguments);
    },

    triggerBlur: function () {
        var picker = this.picker;

        this.callParent(arguments);

        if (picker && picker.isVisible()) {
            picker.close();
        }
    },

    mimicBlur: function (e) {
        var picker = this.picker;

        // ignore mousedown events within the picker element
        if (!picker || !e.within(picker.el, false, true) && !this.isEventWithinPicker(e) && !this.isEventWithinModalMask(e)) {
            this.callParent(arguments);
        }
    },

    isEventWithinModalMask: function (e) {
        var modalMask = Ext.WindowManager.mask;

        return modalMask ? e.within(modalMask.maskEl, false, true) || e.within(modalMask.el, false, true) : false;
    },

    isEventWithinPicker: function (e) {
        var loadMask = this.picker.loadMask,
            timePicker = this.picker.timeField.picker;
        
        var isInsidePicker = loadMask && (e.within(loadMask.maskEl, false, true) || e.within(loadMask.el, false, true))
            || timePicker && (e.within(timePicker.el, false, true) || e.within(timePicker.loadMask.maskEl, false, true) || e.within(timePicker.loadMask.el, false, true));

        return isInsidePicker;
    }

});
