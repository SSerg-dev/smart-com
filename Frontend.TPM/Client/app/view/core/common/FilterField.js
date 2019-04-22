Ext.define('App.view.core.common.FilterField', {
    extend: 'Ext.form.field.Trigger',
    alias: 'widget.filterfield',

    trigger1Cls: Ext.baseCSSPrefix + 'form-search-trigger',
    trigger2Cls: Ext.baseCSSPrefix + 'form-clear-trigger',

    editable: false,

    selectorTitle: l10n.ns('core').value('Filter'),

    onTrigger1Click: function () {
        this.createPicker().show();
        this.inputEl.focus();
    },

    onTrigger2Click: function () {
        this.setValue(null);
    },

    setValue: function (value) {
        if (value && Ext.isString(value)) {
            this.value = Ext.JSON.decode(value, true); // TODO
        } else {
            this.value = value;
        }
        var displayValue = this.value ? (this.value.displayFilter || '') : '';
        this.setRawValue(displayValue);
        return this;
    },

    getValue: function () {
        return Ext.JSON.encode(this.value);
    },

    filterItems: [],

    createPicker: function () {
        return this.picker = Ext.widget('filterpicker', {
            pickerField: this,
            title: this.selectorTitle,
            value: this.value, // NOT getValue(),
            viewItems: this.filterItems,
            listeners: {
                scope: this,
                select: this.onSelect
            }
        });
    },

    onSelect: function (picker, val) {
        this.setValue(val);
        this.fireEvent('select', this, val);
        picker.close();
        this.focus(true, 10);
    },

    initComponent: function () {
        this.addEvents('select');
        this.callParent(arguments);
    }
});
