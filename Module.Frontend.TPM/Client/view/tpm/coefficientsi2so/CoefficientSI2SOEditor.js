Ext.define('App.view.tpm.coefficientsi2so.CoefficientSI2SOEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.coefficientsi2soeditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'DemandCode',
            fieldLabel: l10n.ns('tpm', 'CoefficientSI2SO').value('DemandCode'),
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'BrandTech').value('Name'),
            name: 'BrandTechId',
            selectorWidget: 'brandtech',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.brandtech.BrandTech',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.brandtech.BrandTech',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'BrandTechName'
            }]
        }, {
            xtype: 'numberfield',
            name: 'CoefficientValue',
            fieldLabel: l10n.ns('tpm', 'CoefficientSI2SO').value('CoefficientValue'),
            minValue: 0.01,
        }]
    }
});     