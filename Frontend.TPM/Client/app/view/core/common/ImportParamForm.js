Ext.define('App.view.core.common.ImportParamForm', {
    extend: 'App.view.core.common.EditorForm',
    alias: 'widget.importparamform',
    margin: '10 15 5 15',
    columnsCount: 1,

    dockedItems: {
        xtype: 'label',
        itemId: 'errormsg',
        dock: 'bottom',
        text: l10n.ns('core').value('errorImportFormMessage'),
        cls: 'error-import-form-msg',
        floating: true,
        shadow: false,
        hidden: true
    },

    itemDefaults: {
        labelAlign: 'left',
        labelWidth: 120,
        labelSeparator: '',
        inputValue: true,
        uncheckedValue: false,
        //isConstrain: true
    },

    initFields: function (fieldValues, constraintValues) {
        if (fieldValues) {
            var me = this;
            Object.keys(fieldValues).map(function (key, index) {
                var value = fieldValues[key];
                var field = me.down('field[name=' + key + ']');
                if (field) {
                    field.setValue(value);
                }
            });
        }
    }

});