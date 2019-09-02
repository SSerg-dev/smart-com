Ext.define('App.view.tpm.incrementalpromo.DeletedIncrementalPromo', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedincrementalpromo',
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
            model: 'App.model.tpm.incrementalpromo.DeletedIncrementalPromo',
            storeId: 'deletedincrementalpromostore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.incrementalpromo.DeletedIncrementalPromo',
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
                text: l10n.ns('tpm', 'IncrementalPromo').value('ProductZREP'),
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
                text: l10n.ns('tpm', 'IncrementalPromo').value('ProductName'),
                dataIndex: 'ProductName',
                filter: {
                    type: 'search',
                    selectorWidget: 'product',
                    valueField: 'ProductEN',
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
                text: l10n.ns('tpm', 'IncrementalPromo').value('PromoClient'),
                dataIndex: 'PromoClient',                
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
            }, {
                text: l10n.ns('tpm', 'IncrementalPromo').value('PromoNumber'),
                dataIndex: 'PromoNumber'
            }, {
                text: l10n.ns('tpm', 'IncrementalPromo').value('PromoName'),
                dataIndex: 'PromoName'
            }, {
                text: l10n.ns('tpm', 'IncrementalPromo').value('PlanPromoIncrementalCases'),
                dataIndex: 'PlanPromoIncrementalCases',
                format: '0.00',
            }, {
                text: l10n.ns('tpm', 'IncrementalPromo').value('CasePrice'),
                dataIndex: 'CasePrice',
                format: '0.00',
            }, {
                text: l10n.ns('tpm', 'IncrementalPromo').value('PlanPromoIncrementalLSV'),
                dataIndex: 'PlanPromoIncrementalLSV',
                format: '0.00',
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.incrementalpromo.DeletedIncrementalPromo',
        items: []
    }]
});