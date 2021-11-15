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
            model: 'App.model.tpm.competitorpromo.CompetitorPromo',
            storeId: 'competitorpromostore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.competitorpromo.CompetitorPromo',
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
                    text: l10n.ns('tpm', 'Promo').value('Number'),
                    dataIndex: 'Number',
                    width: 110
                }, {
                    text: l10n.ns('tpm', 'CompetitorPromo').value('CompetitorName'),
                    dataIndex: 'CompetitorName',
                    filter: {
                        type: 'search',
                        selectorWidget: 'competitor',
                        valueField: 'Name',
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
                    text: l10n.ns('tpm', 'CompetitorPromo').value('ClientTreeFullPathName'),
                    dataIndex: 'ClientTreeFullPathName',
                    minWidth: 200,
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
                    text: l10n.ns('tpm', 'CompetitorPromo').value('ClientTreeObjectId'),
                    dataIndex: 'ClientTreeObjectId'
                }, {
                    text: l10n.ns('tpm', 'Promo').value('BrandTechName'),
                    dataIndex: 'CompetitorBrandTechName',
                    width: 120,
                    filter: {
                        type: 'search',
                        selectorWidget: 'competitorbrandtech',
                        valueField: 'BrandTech',
                        store: {
                            type: 'directorystore',
                            model: 'App.model.tpm.competitorbrandtech.CompetitorBrandTech',
                            extendedFilter: {
                                xclass: 'App.ExtFilterContext',
                                supportedModels: [{
                                    xclass: 'App.ExtSelectionFilterModel',
                                    model: 'App.model.tpm.competitorbrandtech.CompetitorBrandTech',
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
                    text: l10n.ns('tpm', 'Promo').value('EndDate'),
                    dataIndex: 'EndDate',
                    width: 100,
                    renderer: Ext.util.Format.dateRenderer('d.m.Y'),
                }, {
                    xtype: 'numbercolumn',
                    format: '0.00',
                    text: l10n.ns('tpm', 'CompetitorPromo').value('Discount'),
                    dataIndex: 'Discount',
                    width: 110,
                    hidden: false,
                }, {
                    xtype: 'numbercolumn',
                    format: '0.00',
                    text: l10n.ns('tpm', 'CompetitorPromo').value('Price'),
                    dataIndex: 'Price',
                    width: 110,
                    hidden: false,
                }, {
                    text: l10n.ns('tpm', 'CompetitorPromo').value('Subrange'),
                    dataIndex: 'Subrange',
                    width: 150,
                }
            ]
        }
    }, {
            xtype: 'editabledetailform',
            itemId: 'detailform',
            model: 'App.model.tpm.competitorpromo.CompetitorPromo',
            items: [{
                text: l10n.ns('tpm', 'Promo').value('Number'),
                dataIndex: 'Number',
                width: 110
            }, {
                xtype: 'treesearchfield',
                name: 'ClientTreeId',
                fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('ClientTreeFullPathName'),
                selectorWidget: 'clienttree',
                valueField: 'Id',
                displayField: 'FullPathName',
                store: {
                    storeId: 'clienttreestore',
                    model: 'App.model.tpm.clienttree.ClientTree',
                    autoLoad: false,
                    root: {}
                },
                mapping: [{
                    from: 'FullPathName',
                    to: 'ClientTreeFullPathName'
                }]
            }, {
                text: l10n.ns('tpm', 'Promo').value('BrandTechName'),
                dataIndex: 'CompetitorBrandTechName',
                width: 120,
                filter: {
                    type: 'search',
                    selectorWidget: 'competitorbrandtech',
                    valueField: 'BrandTech',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.competitorbrandtech.CompetitorBrandTech',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.competitorbrandtech.CompetitorBrandTech',
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
                text: l10n.ns('tpm', 'Promo').value('EndDate'),
                dataIndex: 'EndDate',
                width: 105,
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            }, {
                xtype: 'numbercolumn',
                text: l10n.ns('tpm', 'CompetitorPromo').value('Price'),
                dataIndex: 'Price',
                width: 110,
            }, {
                xtype: 'numbercolumn',
                text: l10n.ns('tpm', 'CompetitorPromo').value('Discount'),
                dataIndex: 'Discount',
                width: 110,
            }, {
                text: l10n.ns('tpm', 'CompetitorPromo').value('Subrange'),
                dataIndex: 'Name',
                width: 150,
            }
            ]
        }]
});
