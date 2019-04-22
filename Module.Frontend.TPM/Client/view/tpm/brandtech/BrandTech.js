Ext.define('App.view.tpm.brandtech.BrandTech', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.brandtech',
    title: l10n.ns('tpm', 'compositePanelTitles').value('BrandTech'),

    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right'
    }],
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
                glyph: 0xf220,
                itemgroup: 'loadimportbutton',
                exactlyModelCompare: true,
                text: l10n.ns('core', 'additionalMenu').value('fullImportXLSX'),
                resource: '{0}',
                action: 'FullImportXLSX',
                allowFormat: ['zip', 'xlsx']
            }, {
                glyph: 0xf21d,
                itemId: 'loadimporttemplatexlsxbutton',
                exactlyModelCompare: true,
                text: l10n.ns('core', 'additionalMenu').value('importTemplateXLSX'),
                action: 'DownloadTemplateXLSX'
            }, {
                glyph: 0xf21d,
                itemId: 'exportxlsxbutton',
                exactlyModelCompare: true,
                text: l10n.ns('core', 'additionalMenu').value('exportXLSX'),
                action: 'ExportXLSX'
            }]
        }
    }
    ],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.brandtech.BrandTech',
            storeId: 'brandtechstore',
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
                text: l10n.ns('tpm', 'Product').value('BrandName'),
                dataIndex: 'BrandName',
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
                text: l10n.ns('tpm', 'Product').value('TechnologyName'),
                dataIndex: 'TechnologyName',
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
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.brandtech.BrandTech',
        items: [{
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandName'),
            name: 'BrandId',
            selectorWidget: 'brand',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.brand.Brand',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.brand.Brand',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'BrandName'
            }]
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Product').value('TechnologyName'),
            name: 'TechnologyId',
            selectorWidget: 'technology',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.technology.Technology',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.technology.Technology',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'TechnologyName'
            }]
        }]
    }]
});