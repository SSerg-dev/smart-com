Ext.define('App.view.tpm.client.ClientEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.clienteditor',
    width: 500,
    minWidth: 500,
    maxHeight: 500,

    items: {
        xtype: 'editorform',
        columnsCount: 1,
        items: [{
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Client').value('CommercialSubnetName'),
            name: 'CommercialSubnetId',
            selectorWidget: 'commercialsubnet',
            valueField: 'Id',
            displayField: 'Name',
            needUpdateMappings: true,
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.commercialsubnet.CommercialSubnet',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.commercialsubnet.CommercialSubnet',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            onTrigger2Click: function () {
                var commercialsubnetcommercialnetname = this.up('editorform').down('singlelinedisplayfield[name=CommercialSubnetCommercialNetName]');

                commercialsubnetcommercialnetname.setValue(null);
                this.setValue(null);
            },
            mapping: [{
                from: 'Name',
                to: 'CommercialSubnetName'
            }, {
                from: 'CommercialNetName',
                to: 'CommercialSubnetCommercialNetName'
            }]
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Client').value('CommercialSubnetCommercialNetName'),
            name: 'CommercialSubnetCommercialNetName',
        }]
    }
});
