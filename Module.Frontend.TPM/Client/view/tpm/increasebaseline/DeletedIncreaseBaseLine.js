Ext.define('App.view.tpm.increasebaseline.DeletedIncreaseBaseLine', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedincreasebaseline',
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
            model: 'App.model.tpm.increasebaseline.DeletedIncreaseBaseLine',
            storeId: 'deletedincreasebaselinestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.increasebaseline.DeletedIncreaseBaseLine',
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
                text: l10n.ns('tpm', 'IncreaseBaseLine').value('ProductZREP'),
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
                text: l10n.ns('tpm', 'IncreaseBaseLine').value('ClientTreeDemandCode'),
                dataIndex: 'DemandCode'
            }, {
                text: l10n.ns('tpm', 'IncreaseBaseLine').value('StartDate'),
                dataIndex: 'StartDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'IncreaseBaseLine').value('InputBaselineQTY'),
                dataIndex: 'InputBaselineQTY'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'IncreaseBaseLine').value('SellInBaselineQTY'),
                dataIndex: 'SellInBaselineQTY'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'IncreaseBaseLine').value('SellOutBaselineQTY'),
                dataIndex: 'SellOutBaselineQTY'
            }, {
                xtype: 'numbercolumn',
                format: '0.',
                text: l10n.ns('tpm', 'IncreaseBaseLine').value('Type'),
                dataIndex: 'Type'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.baseline.DeletedIncreaseBaseLine',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductZREP',
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('ProductZREP'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DemandCode',
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('ClientTreeDemandCode'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('StartDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'InputBaselineQTY',
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('InputBaselineQTY'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SellInBaselineQTY',
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('SellInBaselineQTY'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'SellOutBaselineQTY',
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('SellOutBaselineQTY'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Type',
            fieldLabel: l10n.ns('tpm', 'IncreaseBaseLine').value('Type'),
        }]
    }]
});