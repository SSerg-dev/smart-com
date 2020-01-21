Ext.define('App.view.tpm.promosupport.PromoLinkedTICosts', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.promolinkedticosts',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoLinked'),

    customHeaderItems: [
        ResourceMgr.getAdditionalMenu('core').base = {
            glyph: 0xf068,
            text: l10n.ns('core', 'additionalMenu').value('additionalBtn'),

            menu: {
                xtype: 'customheadermenu',
                items: [{
                    glyph: 0xf4eb,
                    itemId: 'gridsettings',
                    text: l10n.ns('core', 'additionalMenu').value('gridSettingsMenuItem'),
                    action: 'SaveGridSettings',
                    resource: 'Security'
                }]
            }
        },
        ResourceMgr.getAdditionalMenu('core').import = {
            glyph: 0xf21b,
            text: l10n.ns('core', 'additionalMenu').value('importExportBtn'),

            menu: {
                xtype: 'customheadermenu',
                items: [{
                    glyph: 0xf21d,
                    itemId: 'customexportxlsxbutton',
                    exactlyModelCompare: true,
                    text: l10n.ns('core', 'additionalMenu').value('exportXLSX'),
                    action: 'ExportXLSX',
                }]
            }
        }
    ],

    dockedItems: [{
        xtype: 'addonlydirectorytoolbar',
        dock: 'right'
    }],

    listeners: {
        afterrender: function (grid) {
             var toolbar = grid.down('addonlydirectorytoolbar');
            var addBtn = toolbar.down('#addbutton');
            var deleteBtn = toolbar.down('#deletebutton');

            // для Cost Production нельзя редактировать список промо
            if (grid.up().down('costproduction')) {
                addBtn.hide();
                deleteBtn.disable();
            }
        },
        
    },

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        listeners: {
           
            selectionchange: function (grid) { 
                if (grid.hasSelection()) 
                var status = grid.getSelection()[0].data.PromoStatusName;
                var panel = this.up();
                 var toolbar = panel.down('addonlydirectorytoolbar'); 
                var deleteBtn = toolbar.down('#deletebutton');

                // для Cost Production нельзя редактировать список промо
                if (status == 'Closed') {
                    deleteBtn.disable();
                } else if (!panel.up().down('costproduction')){ 
                    deleteBtn.enable();

                }
            }

        },
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.promosupportpromo.PromoSupportPromoTICost',
            storeId: 'promolinkedticostsstore',
            autoLoad: false,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promosupportpromo.PromoSupportPromoTICost',
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
                text: l10n.ns('tpm', 'PromoSupportPromoTICost').value('Number'),
                dataIndex: 'Number',
                width: 110
            }, {
                text: l10n.ns('tpm', 'PromoSupportPromoTICost').value('Name'),
                dataIndex: 'Name',
                width: 150
            }, {
                text: l10n.ns('tpm', 'PromoSupportPromoTICost').value('BrandTechName'),
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
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'PromoSupportPromoTICost').value('PlanCostTE'),
                dataIndex: 'PlanCalculation'
            }, {
                xtype: 'numbercolumn',
				format: '0.00',
                text: l10n.ns('tpm', 'PromoSupportPromoTICost').value('ActualCostTE'),
                dataIndex: 'FactCalculation'
            }, {
                text: l10n.ns('tpm', 'PromoSupportPromoTICost').value('EventName'),
                dataIndex: 'EventName',
                width: 110
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'PromoSupportPromoTICost').value('StartDate'),
                dataIndex: 'StartDate',
                width: 110,
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'PromoSupportPromoTICost').value('EndDate'),
                dataIndex: 'EndDate',
                width: 100,
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'PromoSupportPromoTICost').value('PromoStatusName'),
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
        model: 'App.model.tpm.promosupportpromo.PromoSupportPromoTICost',
        items: []
    }]
});