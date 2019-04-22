Ext.define('App.view.tpm.commercialsubnet.CommercialSubnet', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.commercialsubnet',
    title: l10n.ns('tpm', 'compositePanelTitles').value('CommercialSubnet'),

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
            model: 'App.model.tpm.commercialsubnet.CommercialSubnet',
            storeId: 'commercialsubnetstore',
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
                text: l10n.ns('tpm', 'CommercialSubnet').value('Name'),
                dataIndex: 'Name'
            }, {
                text: l10n.ns('tpm', 'CommercialSubnet').value('CommercialNetName'),
                dataIndex: 'CommercialNetName',
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
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.commercialsubnet.CommercialSubnet',
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
    }]
});
