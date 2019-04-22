Ext.define('App.view.tpm.promosupport.PromoLinkedViewer', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.promolinkedviewer',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Promo'),

    deletedPromoLinkedIds: [],

    listeners: {
        afterrender: function (grid) {
            var toolbar = grid.down('custombigtoolbar');
            var addBtn = toolbar.down('#addbutton');
            var deleteBtn = toolbar.down('#deletebutton');
            var editorWind = grid.up('custompromosupporteditor');
            var columns = grid.down('grid').columns;
            var columnsForHide;

            // для Cost Production нельзя редактировать список промо
            if (editorWind && editorWind.costProduction) {
                addBtn.hide();
                deleteBtn.hide();

                columnsForHide = columns.filter(function (n) { return n.dataIndex == "PlanCalculation" || n.dataIndex == 'FactCalculation' });
            }
            else
            {
                columnsForHide = columns.filter(function (n) { return n.dataIndex == "PlanCostProd" || n.dataIndex == 'FactCostProd' });
            }

            columnsForHide.forEach(function (n) {
                n.hide(true);
            })
        }
    },

    dockedItems: [{
        xtype: 'custombigtoolbar',

        items: [{
            xtype: 'widthexpandbutton',
            ui: 'fill-gray-button-toolbar',
            text: l10n.ns('core', 'selectablePanelButtons').value('toolbarCollapse'),
            glyph: 0xf13d,
            glyph1: 0xf13e,
            target: function () {
                return this.up('toolbar');
            },
        }, {
            itemId: 'addbutton',
            action: 'Post',
            glyph: 0xf415,
            text: l10n.ns('core', 'crud').value('createButtonText'),
            tooltip: l10n.ns('core', 'crud').value('createButtonText')
        }, {
            itemId: 'updatebutton',
            action: 'Patch',
            glyph: 0xf64f,
            text: l10n.ns('core', 'crud').value('updateButtonText'),
            tooltip: l10n.ns('core', 'crud').value('updateButtonText')
        }, {
            itemId: 'deletebutton',
            action: 'Delete',
            glyph: 0xf5e8,
            text: l10n.ns('core', 'crud').value('deleteButtonText'),
            tooltip: l10n.ns('core', 'crud').value('deleteButtonText')
        }],
        dock: 'right'
    }],

    minHeight: 0,

    systemHeaderItems: [],
    customHeaderItems: [],

    style: {
        "border-left": "1px solid #ccd1d9",
        "margin": "5px 0 5px 0",
        "box-shadow": "0px 3px 8px rgba(0,0,0,.25)"
    },

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorWindowModel',
        store: {
            type: 'directorystore',
            autoLoad: false,
            model: 'App.model.tpm.promosupportpromo.PromoSupportPromoView',
            storeId: 'promolinkedviewerstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promosupportpromo.PromoSupportPromoView',
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
                filter: false
            },
            items: [{
                text: l10n.ns('tpm', 'PromoSupportPromoView').value('Number'),
                dataIndex: 'Number',
                width: 110
            }, {
                text: l10n.ns('tpm', 'PromoSupportPromoView').value('Name'),
                dataIndex: 'Name',
                width: 150
            }, {
                text: l10n.ns('tpm', 'PromoSupportPromoView').value('BrandTechName'),
                dataIndex: 'BrandTechName',
                width: 120,
                /*filter: {
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
                }*/
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'PromoSupportPromoView').value('PlanCostTE'),
                dataIndex: 'PlanCalculation'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'PromoSupportPromoView').value('ActualCostTE'),
                dataIndex: 'FactCalculation'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'PromoSupportPromoView').value('PlanCostProd'),
                dataIndex: 'PlanCostProd'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'PromoSupportPromoView').value('ActualCostProd'),
                dataIndex: 'FactCostProd'
            }, {
                text: l10n.ns('tpm', 'PromoSupportPromoView').value('EventName'),
                dataIndex: 'EventName',
                width: 110
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'PromoSupportPromoView').value('StartDate'),
                dataIndex: 'StartDate',
                width: 110,
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'PromoSupportPromoView').value('EndDate'),
                dataIndex: 'EndDate',
                width: 100,
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'PromoSupportPromoView').value('PromoStatusName'),
                dataIndex: 'PromoStatusName',
                width: 120,
                /*filter: {
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
                }*/
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promosupportpromo.PromoSupportPromoView',
        items: []
    }]
});