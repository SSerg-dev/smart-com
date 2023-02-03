Ext.define('App.view.tpm.rsmode.DeletedRSmode', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedrsmode',
    title: 'Cancelled Rolling Scenarios',

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
            model: 'App.model.tpm.rsmode.DeletedRSmode',
            storeId: 'deletedrsmodestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.rsmode.DeletedRSmode',
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
            items: [
                {
                text: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate'),
                dataIndex: 'DeletedDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
                },
                {
                    text: l10n.ns('tpm', 'RSmode').value('RSId'),
                    dataIndex: 'RSId'
                },
                {
                    text: l10n.ns('tpm', 'RSmode').value('ClientTreeFullPathName'),
                    dataIndex: 'ClientTreeFullPathName',
                    minWidth: 200,
                    filter: {
                        xtype: 'treefsearchfield',
                        trigger2Cls: '',
                        selectorWidget: 'clienttree',
                        valueField: 'FullPathName',
                        displayField: 'FullPathName',
                        multiSelect: true,
                        operator: 'conts',
                        store: {
                            model: 'App.model.tpm.clienttree.ClientTree',
                            autoLoad: false,
                            root: {}
                        },
                    },
                    renderer: function (value) {
                        return renderWithDelimiter(value, ' > ', '  ');
                    }
                },
                {
                    text: l10n.ns('tpm', 'RSmode').value('ExpirationDate'),
                    dataIndex: 'ExpirationDate',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y')
                },
                {
                    text: l10n.ns('tpm', 'RSmode').value('StartDate'),
                    dataIndex: 'StartDate',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y')
                },
                {
                    text: l10n.ns('tpm', 'RSmode').value('EndDate'),
                    dataIndex: 'EndDate',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y')
                },
                {
                    text: l10n.ns('tpm', 'RSmode').value('PromoStatusName'),
                    dataIndex: 'PromoStatusName',
                    width: 120,
                    filter: {
                        type: 'search',
                        selectorWidget: 'promostatus',
                        valueField: 'Name',
                        operator: 'eq',
                        store: {
                            type: 'directorystore',
                            model: 'App.model.tpm.promostatus.PromoStatus',
                            extendedFilter: {
                                xclass: 'App.ExtFilterContext',
                                supportedModels: [{
                                    xclass: 'App.ExtSelectionFilterModel',
                                    model: 'App.model.tpm.promostatus.PromoStatus',
                                    modelId: 'efselectionmodel'
                                }, {
                                    xclass: 'App.ExtTextFilterModel',
                                    modelId: 'eftextmodel'
                                }]
                            }
                        }
                    }
                },
                {
                    text: l10n.ns('tpm', 'RSmode').value('IsMLmodel'),
                    dataIndex: 'IsMLmodel',
                    renderer: function (value) {
                        return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                    }
                },
                {
                    text: l10n.ns('tpm', 'RSmode').value('TaskStatus'),
                    dataIndex: 'TaskStatus'
                },
            ]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.rsmode.DeletedRSmode',
        items: []
    }]
});