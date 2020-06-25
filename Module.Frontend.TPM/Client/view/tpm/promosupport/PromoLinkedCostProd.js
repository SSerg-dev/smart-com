Ext.define('App.view.tpm.promosupport.PromoLinkedCostProd', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.promolinkedcostprod',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoLinked'),

    dockedItems: [{
        xtype: 'readonlydirectorytoolbar',
        dock: 'right'
    }],

    listeners: {
        //afterrender: function (grid) {
        //    var toolbar = grid.down('addonlydirectorytoolbar');
        //    var addBtn = toolbar.down('#addbutton');
        //    var deleteBtn = toolbar.down('#deletebutton');

        //    // для Cost Production нельзя редактировать список промо
        //    if (grid.up().down('costproduction')) {
        //        addBtn.hide();
        //        deleteBtn.hide();
        //    }
        //}
    },

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.promosupportpromo.PromoSupportPromoCostProd',
            storeId: 'promolinkedstore',
            autoLoad: false,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promosupportpromo.PromoSupportPromoCostProd',
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
                text: l10n.ns('tpm', 'PromoSupportPromoCostProd').value('Number'),
                dataIndex: 'Number',
                width: 110
            }, {
                text: l10n.ns('tpm', 'PromoSupportPromoCostProd').value('Name'),
                dataIndex: 'Name',
                width: 150
            }, {
                text: l10n.ns('tpm', 'PromoSupportPromoCostProd').value('BrandTechName'),
                dataIndex: 'BrandTechName',
                width: 120,
                filter: {
                    type: 'search',
                    selectorWidget: 'brandtech',
                    valueField: 'BrandsegTechsub',
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
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'PromoSupportPromoCostProd').value('PlanCostProd'),
                dataIndex: 'PlanCostProd'
            }, {
                xtype: 'numbercolumn',
				format: '0.00',
                text: l10n.ns('tpm', 'PromoSupportPromoCostProd').value('ActualCostProd'),
                dataIndex: 'FactCostProd'
            }, {
                text: l10n.ns('tpm', 'PromoSupportPromoCostProd').value('EventName'),
                dataIndex: 'EventName',
                width: 110
            },  {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'PromoSupportPromoCostProd').value('StartDate'),
                dataIndex: 'StartDate',
                width: 110,
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'PromoSupportPromoCostProd').value('EndDate'),
                dataIndex: 'EndDate',
                width: 100,
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'PromoSupportPromoCostProd').value('PromoStatusName'),
                dataIndex: 'PromoStatusName',
                width: 120,
                filter: {
                    type: 'search',
                    selectorWidget: 'promostatus',
                    valueField: 'Name',
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
        model: 'App.model.tpm.promosupportpromo.PromoSupportPromoCostProd',
        items: []
    }]
});