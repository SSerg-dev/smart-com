Ext.define('App.view.tpm.promosupport.CostProductionPromoLinked', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.costproductionpromolinked',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoLinked'),

    dockedItems: [{
        xtype: 'readonlydirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.promosupportpromo.PromoSupportPromo',
            storeId: 'promolinkedstore',
            autoLoad: false,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promosupportpromo.PromoSupportPromo',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            sorters: [{
                property: 'Number',
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
                text: l10n.ns('tpm', 'PromoSupportPromo').value('Number'),
                dataIndex: 'Number',
                width: 110
            }, {
                text: l10n.ns('tpm', 'PromoSupportPromo').value('Name'),
                dataIndex: 'Name',
                width: 150
            }, {
                text: l10n.ns('tpm', 'PromoSupportPromo').value('BrandTechName'),
                dataIndex: 'BrandTechName',
                width: 120,
                filter: {
                    type: 'search',
                    selectorWidget: 'brandtech',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.brandtech.BrandTech',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.brandtech.BrandTech',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'PromoSupportPromo').value('EventName'),
                dataIndex: 'EventName',
                width: 110
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'PromoSupportPromo').value('StartDate'),
                dataIndex: 'StartDate',
                width: 110,
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'PromoSupportPromo').value('EndDate'),
                dataIndex: 'EndDate',
                width: 100,
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'PromoSupportPromo').value('PromoStatusName'),
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
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promosupportpromo.PromoSupportPromo',
        items: []
    }]
});