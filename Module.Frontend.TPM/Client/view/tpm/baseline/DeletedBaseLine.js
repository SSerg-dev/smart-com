Ext.define('App.view.tpm.baseline.DeletedBaseLine', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedbaseline',
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
            model: 'App.model.tpm.baseline.DeletedBaseLine',
            storeId: 'deletedbaselinestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.baseline.DeletedBaseLine',
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
                text: l10n.ns('tpm', 'BaseLine').value('ProductZREP'),
                dataIndex: 'ProductZREP',
                filter: {
                    type: 'search',
                    selectorWidget: 'product',
                    valueField: 'ZREP',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.product.Product',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.product.Product',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'BaseLine').value('ClientTreeDemandCode'),
                dataIndex: 'ClientTreeDemandCode',
                minWidth: 200,
                filter: {
                    xtype: 'treefsearchfield',
                    trigger2Cls: '',
                    selectorWidget: 'clienttree',
                    valueField: 'DemandCode',
                    displayField: 'DemandCode',
                    multiSelect: true,
                    operator: 'conts',
                    store: {
                        model: 'App.model.tpm.clienttree.ClientTree',
                        autoLoad: false,
                        root: {}
                    },
                }
            }, {
                text: l10n.ns('tpm', 'BaseLine').value('StartDate'),
                dataIndex: 'StartDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'BaseLine').value('QTY'),
                dataIndex: 'QTY'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'BaseLine').value('Price'),
                dataIndex: 'Price'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'BaseLine').value('BaselineLSV'),
                dataIndex: 'BaselineLSV'
            }, {
                xtype: 'numbercolumn',
                format: '0.',
                text: l10n.ns('tpm', 'BaseLine').value('Type'),
                dataIndex: 'Type'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.baseline.DeletedBaseLine',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductZREP',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('ProductZREP'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientTreeDemandCode',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('ClientTreeDemandCode'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('StartDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'QTY',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('QTY'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Price',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('Price'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BaselineLSV',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('BaselineLSV'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Type',
            fieldLabel: l10n.ns('tpm', 'BaseLine').value('Type'),
        }]
    }]
});