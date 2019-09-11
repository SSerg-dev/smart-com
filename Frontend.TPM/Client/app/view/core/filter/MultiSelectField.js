Ext.define('App.view.core.filter.MultiSelectField', {
    extend: 'Ext.form.field.Trigger',
    alias: 'widget.multiselectfield',

    //triggerCls: Ext.baseCSSPrefix + 'form-search-trigger',
    editable: false,
    maxDisplayValueLength: 50,

    constructor: function (config) {
        this.callParent(arguments);
        this.initConfig(config);
    },

    onTriggerClick: function () {
        var picker = this.createPicker();

        if (picker) {
            if (!this.allowRange && this.multiSelectionType === 'range') {
                //TODO: ошибка
                return;
            }

            picker.show();
            picker.setValue(this.getValue());
        }
    },

    onCancelButtonClick: function (button) {
        this.picker.close();
    },

    onSelectionChange: function (selModel) {
        this.picker.down('#delete').setDisabled(!selModel.hasSelection());
    },

    createPicker: function () {
        return Ext.widget('multiselectwindow', Ext.apply({
            listeners: {
                scope: this,
                select: this.onSelect
            }
        }, this.getInitialConfig()));
    },

    onSelect: function (picker, value) {
        this.setValue(value);
    },

    valueToRaw: function (value) {
        return value ? value.toString(this.itemRenderer, this) : '';
    },

    rawToValue: function () {
        return this.value;
    }

});