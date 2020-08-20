Ext.define('App.view.tpm.rollingvolume.RollingVolume', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.rollingvolume',
    title: l10n.ns('tpm', 'compositePanelTitles').value('RollingVolume'),

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
                    itemId: 'ImportXLSX',
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
            model: 'App.model.tpm.rollingvolume.RollingVolume',
            storeId: 'rollingvolumestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.rollingvolume.RollingVolume',
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
                text: l10n.ns('tpm', 'RollingVolume').value('ZREP'),
                dataIndex: 'ZREP',
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
                text: l10n.ns('tpm', 'RollingVolume').value('SKU'),
                dataIndex: 'SKU',
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
            },{
                text: l10n.ns('tpm', 'RollingVolume').value('BrandTech'),
                dataIndex: 'BrandTech',
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
            },{
                text: l10n.ns('tpm', 'RollingVolume').value('DemandGroup'),
                dataIndex: 'DemandGroup'
            },{
                text: l10n.ns('tpm', 'RollingVolume').value('Week'),
                dataIndex: 'Week'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'RollingVolume').value('PlanProductIncrementalQTY'),
                dataIndex: 'PlanProductIncrementalQTY'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'RollingVolume').value('Actuals'),
                dataIndex: 'Actuals'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'RollingVolume').value('OpenOrders'),
                dataIndex: 'OpenOrders'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'RollingVolume').value('ActualOO'),
                dataIndex: 'ActualOO'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'RollingVolume').value('Baseline'),
                dataIndex: 'Baseline'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'RollingVolume').value('ActualIncremental'),
                dataIndex: 'ActualIncremental'
            },  {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'RollingVolume').value('PreliminaryRollingVolumes'),
                dataIndex: 'PreliminaryRollingVolumes'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'RollingVolume').value('PreviousRollingVolumes'),
                dataIndex: 'PreviousRollingVolumes'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'RollingVolume').value('PromoDifference'),
                dataIndex: 'PromoDifference'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'RollingVolume').value('RollingVolumesCorrection'),
                dataIndex: 'RollingVolumesCorrection'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'RollingVolume').value('RollingVolumesTotal'),
                dataIndex: 'RollingVolumesTotal'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'RollingVolume').value('ManualRollingTotalVolumes'),
                dataIndex: 'ManualRollingTotalVolumes'
            },{
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'RollingVolume').value('FullWeekDiff'),
                dataIndex: 'FullWeekDiff'
            },]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.rollingvolume.RollingVolume',
        items: []
    }]
});

