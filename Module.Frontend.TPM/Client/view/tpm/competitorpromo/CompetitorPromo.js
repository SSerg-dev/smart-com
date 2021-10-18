Ext.define('App.view.tpm.competitorpromo.CompetitorPromo', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.competitorpromo',
    title: l10n.ns('tpm', 'compositePanelTitles').value('CompetitorPromo'),

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
            model: 'App.model.tpm.competitorbrandtech.CompetitorPromo',
            storeId: 'competitorpromostore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.competitorpromo.CompetitorPRomo',
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
            items: [
                {
                    text: l10n.ns('tpm', 'CompetitorPromo').value('Number'),
                    dataIndex: 'Number',
                    width: 110
                }, {
                    text: l10n.ns('tpm', 'CompetitorPromo').value('CompetitorName'),
                    dataIndex: 'CompetitorName',
                    filter: {
                        type: 'search',
                        selectorWidget: 'competitorname',
                        valueField: 'CompetitorName',
                        store: {
                            type: 'directorystore',
                            model: 'App.model.tpm.competitor.Competitor',
                            extendedFilter: {
                                xclass: 'App.ExtFilterContext',
                                supportedModels: [{
                                    xclass: 'App.ExtSelectionFilterModel',
                                    model: 'App.model.tpm.competitor.Competitor',
                                    modelId: 'efselectionmodel'
                                }, {
                                    xclass: 'App.ExtTextFilterModel',
                                    modelId: 'eftextmodel'
                                }]
                            }
                        }
                    }
                }, {
                    text: l10n.ns('tpm', 'CompetitorPromo').value('ClientTree'),
                    dataIndex: 'ClientTree',
                    width: 250,
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
                    text: l10n.ns('tpm', 'CompetitorPromo').value('Name'),
                    dataIndex: 'Name',
                    width: 150,
                }, {
                    text: l10n.ns('tpm', 'CompetitorPromo').value('BrandTechName'),
                    dataIndex: 'CompetitorBrandTechName',
                    width: 120,
                    filter: {
                        type: 'search',
                        selectorWidget: 'brandtech',
                        valueField: 'BrandsegTechsub',
                        store: {
                            type: 'directorystore',
                            model: 'App.model.tpm.competitorbrandtech.CompetitorBrandTech',
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
                    text: l10n.ns('tpm', 'CompetitorPromo').value('StartDate'),
                    dataIndex: 'StartDate',
                    width: 105,
                    renderer: Ext.util.Format.dateRenderer('d.m.Y'),
                }, {
                    xtype: 'datecolumn',
                    text: l10n.ns('tpm', 'CompetitorPromo').value('EndDate'),
                    dataIndex: 'EndDate',
                    width: 100,
                    renderer: Ext.util.Format.dateRenderer('d.m.Y'),
                }, {
                    xtype: 'numbercolumn',
                    format: '0.00',
                    text: l10n.ns('tpm', 'CompetitorPromo').value('Discount'),
                    dataIndex: 'Discount',
                    width: 110,
                    hidden: true,
                }, {
                    xtype: 'numbercolumn',
                    format: '0.00',
                    text: l10n.ns('tpm', 'CompetitorPromo').value('ShelfPrice'),
                    dataIndex: 'ShelfPrice',
                    width: 110,
                    hidden: true,
                }, {
                    text: l10n.ns('tpm', 'CompetitorPromo').value('Subrange'),
                    dataIndex: 'Subrange',
                    width: 150,
                }, {
                    text: l10n.ns('tpm', 'CompetitorPromo').value('GrowthAcceleration'),
                    dataIndex: 'IsGrowthAcceleration',
                    renderer: function (value) {
                        return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                    }
                }, {
                    text: l10n.ns('tpm', 'CompetitorPromo').value('PromoStatusName'),
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
                },
            ]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.competitorpromo.CompetitorPromo',
        afterFormShow: function () {
            this.down('circlecolorfield').fireEvent("afterrender");
        },
        items: [{
                text: l10n.ns('tpm', 'Promo').value('Number'),
                dataIndex: 'Number',
                width: 110
            }, {
                xtype: 'searchfield',
                fieldLabel: l10n.ns('tpm', 'CompetitorPromo ').value('CompetitorName'),
                name: 'CompetitorName',
                selectorWidget: 'competitorname',
                valueField: 'Id',
                displayField: 'CompetitorName',
                allowBlank: true,
                allowOnlyWhitespace: true,
                store: {
                    type: 'directorystore',
                    model: 'App.model.tpm.competitor.Competitor',
                    extendedFilter: {
                        xclass: 'App.ExtFilterContext',
                        supportedModels: [{
                            xclass: 'App.ExtSelectionFilterModel',
                            model: 'App.model.tpm.competitor.Competitor',
                            modelId: 'efselectionmodel'
                        }]
                    }
                },
                mapping: [{
                    from: 'Name',
                    to: 'CompetitorName'
                }]
            }, {
                text: l10n.ns('tpm', 'CompetitorPromo').value('ClientHierarchy'),
                dataIndex: 'ClientHierarchy',
                width: 250,
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
                text: l10n.ns('tpm', 'Promo').value('BrandTechName'),
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
                text: l10n.ns('tpm', 'Promo').value('Name'),
                dataIndex: 'Name',
                width: 150,
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Promo').value('StartDate'),
                dataIndex: 'StartDate',
                width: 105,
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Promo').value('StartDate'),
                dataIndex: 'StartDate',
                width: 105,
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            }, {
                xtype: 'numbercolumn',
                text: l10n.ns('tpm', 'Promo').value('ShelfPrice'),
                dataIndex: 'ShelfPrice',
                width: 110,
            }, {
                xtype: 'numbercolumn',
                text: l10n.ns('tpm', 'Promo').value('Discount'),
                dataIndex: 'Discount',
                width: 110,
            }, {
                text: l10n.ns('tpm', 'Promo').value('Subranges'),
                dataIndex: 'Name',
                width: 150,
            }, {
                text: l10n.ns('tpm', 'Promo').value('IsGrowthAcceleration'),
                dataIndex: 'IsGrowthAcceleration',
                renderer: function (value) {
                    return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                }
            }, {
                text: l10n.ns('tpm', 'Promo').value('PromoStatusName'),
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
    }]
});
