Ext.define('App.view.tpm.nonpromosupport.NonPromoSupportBrandTechDetail', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.nonpromosupportbrandtechdetail',
    title: l10n.ns('tpm', 'compositePanelTitles').value('BrandTech'),

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

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.nonpromosupportbrandtech.NonPromoSupportBrandTechDetail',
            storeId: 'nonpromosupportbrandtechdetailstore',
            autoLoad: false,
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.nonpromosupportbrandtech.NonPromoSupportBrandTechDetail',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            }, 
            listeners: {
                load: function (store) {
                    var filters = store.filters,
                        grid = Ext.ComponentQuery.query('nonpromosupportbrandtechdetail directorygrid')[0];

                    if (filters.items.length === 0) {
                        grid.gridTotalCount = store.getTotalCount();
                    }
                }
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
                text: l10n.ns('tpm', 'NonPromoSupportBrandTech').value('BrandTechBrandName'),
                dataIndex: 'BrandTechBrandName',
                filter: {
                    type: 'search',
                    selectorWidget: 'brand',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.brand.Brand',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.brand.Brand',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'NonPromoSupportBrandTech').value('BrandTechTechnologyName'),
                dataIndex: 'BrandTechTechnologyName',
                filter: {
                    type: 'search',
                    selectorWidget: 'technology',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.technology.Technology',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.technology.Technology',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'NonPromoSupportBrandTech').value('BrandTechBrandTech_code'),
                dataIndex: 'BrandTechBrandTech_code'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.nonpromosupportbrandtech.NonPromoSupportBrandTechDetail',
        items: []
    }]
});