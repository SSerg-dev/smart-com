Ext.define('App.controller.core.ImportExportController', {
    extend: 'Ext.app.Controller',

    init: function () {
        this.listen({
            component: {
                'uploadfilewindow importparamform field[vtype=marsDateRange]': {
                    validitychange: this.onMarsDateFieldValidityChange
                },
                'uploadfilewindow importparamform field[isConstrain=true]': {
                    change: this.onImportParamFieldChange
                }
            }
        });
    },


    onMarsDateFieldValidityChange: function (field, valid) {
        var form = field.up('form'),
            altField;

        if (field.startDateField) {
            altField = form.down('[name=' + field.startDateField + ']');
        } else if (field.finishDateField) {
            altField = form.down('[name=' + field.finishDateField + ']');
        }

        if (altField) {
            altField.validate();
        }
    },

    onImportParamFieldChange: function (field, newValue) {
        var form = field.up('importparamform');

        if (!Ext.isEmpty(newValue)) {
            form.removeCls('error-import-form');
            form.down('#errormsg').getEl().hide();
        }
    }
});