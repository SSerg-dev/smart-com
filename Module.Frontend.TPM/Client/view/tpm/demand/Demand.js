Ext.define('App.view.tpm.demand.Demand', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.demand',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Demand'),

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
                    text: 'Полный импорт XLSX',
                    resource: '{0}',
                    action: 'FullImportXLSX',
                    allowFormat: ['zip', 'xlsx']
                }, {
                    glyph: 0xf21d,
                    itemId: 'exportxlsxbutton',
                    exactlyModelCompare: true,
                    text: 'Экспорт в XLSX',
                    action: 'ExportXLSX'
                }]
            }
        }
    ],

    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right'
    }],
    listeners: {
        afterrender: function (panel, e) {
            panel.down('#createbutton').hide();
        }
    },
    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.demand.Demand',
            storeId: 'demandstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.demand.Demand',
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
                text: l10n.ns('tpm', 'Demand').value('Number'),
                dataIndex: 'Number'
            }, {
                text: l10n.ns('tpm', 'Demand').value('Name'),
                dataIndex: 'Name'
            }, {
			    text: l10n.ns('tpm', 'Demand').value('ClientCommercialSubnetCommercialNetName'),
			    dataIndex: 'ClientCommercialSubnetCommercialNetName',
			    filter: {
			        type: 'search',
			        selectorWidget: 'client',
			        valueField: 'CommercialSubnetCommercialNetName',
			        store: {
			            type: 'directorystore',
			            model: 'App.model.tpm.client.Client',
			            extendedFilter: {
			                xclass: 'App.ExtFilterContext',
			                supportedModels: [{
			                    xclass: 'App.ExtSelectionFilterModel',
			                    model: 'App.model.tpm.client.Client',
			                    modelId: 'efselectionmodel'
			                }, {
			                    xclass: 'App.ExtTextFilterModel',
			                    modelId: 'eftextmodel'
			                }]
			            }
			        }
			    }
            }, {
                text: l10n.ns('tpm', 'Demand').value('BrandName'),
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
			    text: l10n.ns('tpm', 'Demand').value('BrandTechName'),
			    dataIndex: 'BrandTechName',
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
			    xtype: 'datecolumn',
			    text: l10n.ns('tpm', 'Demand').value('StartDate'),
			    dataIndex: 'StartDate',
			    renderer: Ext.util.Format.dateRenderer('d.m.Y')
			}, {
			    xtype: 'datecolumn',
			    text: l10n.ns('tpm', 'Demand').value('EndDate'),
			    dataIndex: 'EndDate',
			    renderer: Ext.util.Format.dateRenderer('d.m.Y')
			}, {
			    xtype: 'datecolumn',
			    text: l10n.ns('tpm', 'Demand').value('DispatchesStart'),
			    dataIndex: 'DispatchesStart',
			    renderer: Ext.util.Format.dateRenderer('d.m.Y')
			}, {
			    xtype: 'datecolumn',
			    text: l10n.ns('tpm', 'Demand').value('DispatchesEnd'),
			    dataIndex: 'DispatchesEnd',
			    renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
			    xtype: 'numbercolumn',
			    format: '0',
			    text: l10n.ns('tpm', 'Demand').value('PlanBaseline'),
			    dataIndex: 'PlanBaseline'
			}, {
			    xtype: 'numbercolumn',
			    format: '0',
			    text: l10n.ns('tpm', 'Demand').value('PlanDuration'),
			    dataIndex: 'PlanDuration'
			}, {
			    xtype: 'numbercolumn',
			    format: '0',
			    text: l10n.ns('tpm', 'Demand').value('PlanUplift'),
			    dataIndex: 'PlanUplift'
			}, {
			    xtype: 'numbercolumn',
			    format: '0',
			    text: l10n.ns('tpm', 'Demand').value('PlanIncremental'),
			    dataIndex: 'PlanIncremental'
			}, {
			    xtype: 'numbercolumn',
			    format: '0',
			    text: l10n.ns('tpm', 'Demand').value('PlanActivity'),
			    dataIndex: 'PlanActivity'
			}, {
			    xtype: 'numbercolumn',
			    format: '0',
			    text: l10n.ns('tpm', 'Demand').value('PlanSteal'),
			    dataIndex: 'PlanSteal'
			}, {
			    xtype: 'numbercolumn',
			    format: '0',
			    text: l10n.ns('tpm', 'Demand').value('FactBaseline'),
			    dataIndex: 'FactBaseline'
			}, {
			    xtype: 'numbercolumn',
			    format: '0',
			    text: l10n.ns('tpm', 'Demand').value('FactDuration'),
			    dataIndex: 'FactDuration'
			}, {
			    xtype: 'numbercolumn',
			    format: '0',
			    text: l10n.ns('tpm', 'Demand').value('FactUplift'),
			    dataIndex: 'FactUplift'
			}, {
			    xtype: 'numbercolumn',
			    format: '0',
			    text: l10n.ns('tpm', 'Demand').value('FactIncremental'),
			    dataIndex: 'FactIncremental'
			}, {
			    xtype: 'numbercolumn',
			    format: '0',
			    text: l10n.ns('tpm', 'Demand').value('FactActivity'),
			    dataIndex: 'FactActivity'
			}, {
			    xtype: 'numbercolumn',
			    format: '0',
			    text: l10n.ns('tpm', 'Demand').value('FactSteal'),
			    dataIndex: 'FactSteal'
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.demand.Demand',
        afterFormShow: function (scope, isCreating) {
            scope.down('numberfield[name=PlanBaseline]').focus(true, 10);
        },
        items: [{
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('Number'),
            name: 'Number'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Demand').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('ClientCommercialSubnetCommercialNetName'),
            name: 'ClientCommercialSubnetCommercialNetName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('BrandName'),
            name: 'BrandName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Demand').value('BrandTechName'),
            name: 'BrandTechName'
        }, {
            xtype: 'singlelinedisplayfield',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'Demand').value('StartDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'Demand').value('EndDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            name: 'DispatchesStart',
            fieldLabel: l10n.ns('tpm', 'Demand').value('DispatchesStart'),
        }, {
            xtype: 'singlelinedisplayfield',
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            name: 'DispatchesEnd',
            fieldLabel: l10n.ns('tpm', 'Demand').value('DispatchesEnd'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'PlanBaseline',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanBaseline'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'PlanDuration',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanDuration'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'PlanUplift',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanUplift'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'PlanIncremental',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanIncremental'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'PlanActivity',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanActivity'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'PlanSteal',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('PlanSteal'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'FactBaseline',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactBaseline'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'FactDuration',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactDuration'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'FactUplift',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactUplift'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'FactIncremental',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactIncremental'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'FactActivity',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactActivity'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: true, allowOnlyWhitespace: true,
            name: 'FactSteal',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'Demand').value('FactSteal'),
        }]
    }]
});
