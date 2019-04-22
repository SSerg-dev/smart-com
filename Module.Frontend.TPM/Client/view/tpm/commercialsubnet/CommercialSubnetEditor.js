Ext.define('App.view.tpm.commercialsubnet.CommercialSubnetEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.commercialsubneteditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'CommercialSubnet').value('Name'),
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'CommercialSubnet').value('CommercialNetName'),
            name: 'CommercialNetId',
            selectorWidget: 'commercialnet',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.commercialnet.CommercialNet',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.commercialnet.CommercialNet',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'CommercialNetName'
            }]
        }]
    }
});
