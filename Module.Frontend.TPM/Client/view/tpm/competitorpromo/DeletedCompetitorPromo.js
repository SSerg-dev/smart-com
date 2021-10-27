Ext.define('App.view.tpm.competitorpromo.DeletedCompetitorPromo', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedcompetitorpromo',
    title: l10n.ns('tpm', 'compositePanelTitles').value('CompetitorPromo'),

    dockedItems: [{
        xtype: 'readonlydeleteddirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.competitorpromo.DeletedCompetitorPromo',
            storeId: 'deletedcompetitorpromostore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.competitorpromo.DeletedCompetitorPromo',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            sorters: [{
                property: 'DeletedDate',
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
                text: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate'),
                dataIndex: 'DeletedDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
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
                text: l10n.ns('tpm', 'Promo').value('ClientHierarchy'),
                dataIndex: 'Client',
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
                text: l10n.ns('tpm', 'Promo').value('Name'),
                dataIndex: 'Name',
                width: 150,
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
                hidden: true,
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'CompetitorPromo').value('Price'),
                dataIndex: 'Price',
                width: 110,
                hidden: true,
            }, {
                text: l10n.ns('tpm', 'CompetitorPromo').value('Subrange'),
                dataIndex: 'Subrange',
                width: 150,
            }, {
                text: l10n.ns('tpm', 'Promo').value('GrowthAcceleration'),
                dataIndex: 'IsGrowthAcceleration',
                renderer: function (value) {
                    return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                }
            }, {
                text: l10n.ns('tpm', 'Promo').value('PromoStatusName'),
                dataIndex: 'Status',
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
            }
            ]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.competitorpromo.DeletedCompetitorPromo',
        items: [{
            xtype: 'datecolumn',
            name: 'DeletedDate',
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Number',
            fieldLabel: l10n.ns('tpm', 'Promo').value('Number'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CompetitorBrandTechName',
            fieldLabel: l10n.ns('tpm', 'Promo').value('BrandTechName'),
        }, {
            xtype: 'datecolumn',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'Promo').value('StartDate'),
        }, {
            xtype: 'datecolumn',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'Promo').value('EndDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Discount',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('Discount'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Price',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('Price'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Subrange',
            fieldLabel: l10n.ns('tpm', 'CompetitorPromo').value('Subrange'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'GrowthAcceleration',
            fieldLabel: l10n.ns('tpm', 'Promo').value('GrowthAcceleration'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Status',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PromoStatusName'),
        }
        ]
    }]
});
