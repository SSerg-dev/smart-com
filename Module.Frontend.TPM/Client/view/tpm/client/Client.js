Ext.define('App.view.tpm.client.Client', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.client',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Client'),

    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.client.Client',
            storeId: 'clientstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.client.Client',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            }
        },

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
                flex: 1,
                minWidth: 100
            },
            items: [{
                text: l10n.ns('tpm', 'Client').value('CommercialSubnetCommercialNetName'),
                dataIndex: 'CommercialSubnetCommercialNetName',
                filter: {
                    type: 'search',
                    selectorWidget: 'commercialnet',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.commercialnet.CommercialNet',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.commercialnet.CommercialNet',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'Client').value('CommercialSubnetName'),
                dataIndex: 'CommercialSubnetName',
                filter: {
                    type: 'search',
                    selectorWidget: 'commercialsubnet',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.commercialsubnet.CommercialSubnet',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.commercialsubnet.CommercialSubnet',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.client.Client',
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
    }]
});
