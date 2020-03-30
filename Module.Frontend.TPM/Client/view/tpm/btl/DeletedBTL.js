Ext.define('App.view.tpm.btl.DeletedBTL', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedbrandtech',
    title: l10n.ns('core', 'compositePanelTitles').value('deletedPanelTitle'),

    dockedItems: [{
        xtype: 'readonlydeleteddirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.btl.DeletedBTL',
            storeId: 'deletedbtlstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.btl.DeletedBTL',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            sorters: [{
                property: 'DeletedDate',
                direction: 'DESC'
            }]
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
                text: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate'),
                dataIndex: 'DeletedDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('tpm', 'BTL').value('EventName'),
                dataIndex: 'EventName',
                filter: {
                    type: 'search',
                    selectorWidget: 'event',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.event.Event',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.event.Event',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'BTL').value('Number'),
                dataIndex: 'Number',
            }, {
                text: l10n.ns('tpm', 'BTL').value('PlanBTLTotal'),
                dataIndex: 'PlanBTLTotal',
            }, {
                text: l10n.ns('tpm', 'BTL').value('ActualBTLTotal'),
                dataIndex: 'ActualBTLTotal',
            }, {
                text: l10n.ns('tpm', 'BTL').value('StartDate'),
                dataIndex: 'StartDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            }, {
                text: l10n.ns('tpm', 'BTL').value('EndDate'),
                dataIndex: 'EndDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            }, {
                text: l10n.ns('tpm', 'BTL').value('InvoiceNumber'),
                dataIndex: 'InvoiceNumber',
            }
            ]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.btl.DeletedBTL',
        items: []
    }]

});
