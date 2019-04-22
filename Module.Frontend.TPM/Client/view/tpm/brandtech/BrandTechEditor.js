Ext.define('App.view.tpm.brandtech.BrandTechEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.brandtecheditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandName'),
            name: 'BrandId',
            selectorWidget: 'brand',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.brand.Brand',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.brand.Brand',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'BrandName'
            }]
        },
        {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('TechnologyName'),
            name: 'TechnologyId',
            selectorWidget: 'technology',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.technology.Technology',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.technology.Technology',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'TechnologyName'
            }]
        }]
    }
});