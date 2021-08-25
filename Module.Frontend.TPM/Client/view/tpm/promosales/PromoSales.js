Ext.define('App.view.tpm.promosales.PromoSales', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.promosales',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoSales'),

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
    
    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.promosales.PromoSales',
            storeId: 'promosalesstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promosales.PromoSales',
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
                text: l10n.ns('tpm', 'PromoSales').value('Number'),
                dataIndex: 'Number'
            }, {
                text: l10n.ns('tpm', 'PromoSales').value('Name'),
                dataIndex: 'Name'
            }, {
                text: l10n.ns('tpm', 'PromoSales').value('ClientCommercialSubnetCommercialNetName'),
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
                text: l10n.ns('tpm', 'PromoSales').value('BrandName'),
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
                text: l10n.ns('tpm', 'PromoSales').value('BrandTechName'),
                dataIndex: 'BrandTechName',
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
                text: l10n.ns('tpm', 'PromoSales').value('PromoStatusName'),
                dataIndex: 'PromoStatusName',
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
            }, {
                text: l10n.ns('tpm', 'PromoSales').value('MechanicName'),
                dataIndex: 'MechanicName',
                filter: {
                    type: 'search',
                    selectorWidget: 'mechanic',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.mechanic.Mechanic',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.mechanic.Mechanic',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'PromoSales').value('MechanicTypeName'),
                dataIndex: 'MechanicTypeName',
                filter: {
                    type: 'search',
                    selectorWidget: 'mechanictype',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.mechanictype.MechanicType',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.mechanictype.MechanicType',
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
                format: '0',
                text: l10n.ns('tpm', 'PromoSales').value('MechanicDiscount'),
                dataIndex: 'MechanicDiscount'
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'PromoSales').value('StartDate'),
                dataIndex: 'StartDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'PromoSales').value('EndDate'),
                dataIndex: 'EndDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'PromoSales').value('DispatchesStart'),
                dataIndex: 'DispatchesStart',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'PromoSales').value('DispatchesEnd'),
                dataIndex: 'DispatchesEnd',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'PromoSales').value('BudgetItemBudgetName'),
                dataIndex: 'BudgetItemBudgetName',
                filter: {
                    type: 'search',
                    selectorWidget: 'budget',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.budget.Budget',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.budget.Budget',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }, {
                text: l10n.ns('tpm', 'PromoSales').value('BudgetItemName'),
                dataIndex: 'BudgetItemName',
                filter: {
                    type: 'search',
                    selectorWidget: 'budgetitem',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.budgetitem.BudgetItem',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.budgetitem.BudgetItem',
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
                format: '0',
                text: l10n.ns('tpm', 'PromoSales').value('Plan'),
                dataIndex: 'Plan'
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'PromoSales').value('Fact'),
                dataIndex: 'Fact'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promosales.PromoSales',
        afterFormShow: function (scope, isCreating) {
            scope.down('textfield[name=Name]').focus(true, 10);
        },
        items: [{
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('Number'),
            name: 'Number'
        }, {
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('Name'),
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Promo').value('MechanicName'),
            name: 'MechanicId',
            selectorWidget: 'mechanic',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.mechanic.Mechanic',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.mechanic.Mechanic',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'MechanicName'
            }]
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Promo').value('MechanicTypeName'),
            name: 'MechanicTypeId',
            selectorWidget: 'mechanictype',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.mechanictype.MechanicType',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.mechanictype.MechanicType',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'MechanicTypeName'
            }]
        }, {
            xtype: 'numberfield',
            minValue: 0,
            maxValue: 100,
            fieldLabel: l10n.ns('tpm', 'Promo').value('MechanicDiscount'),
            name: 'MechanicDiscount'
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('ClientCommercialSubnetCommercialNetName'),
            name: 'ClientId',
            selectorWidget: 'client',
            valueField: 'Id',
            displayField: 'CommercialSubnetCommercialNetName',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.client.Client',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.client.Client',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'CommercialSubnetCommercialNetName',
                to: 'ClientCommercialSubnetCommercialNetName'
            }]
        },  {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Promo').value('BrandName'),
            name: 'BrandName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'Promo').value('BrandTechName'),
            name: 'BrandTechName'
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PromoStatusName'),
            name: 'PromoStatusId',
            selectorWidget: 'promostatus',
            valueField: 'Id',
            displayField: 'Name',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.promostatus.PromoStatus',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.promostatus.PromoStatus',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'PromoStatusName'
            }]
        }, {
            xtype: 'datefield', allowBlank: false, allowOnlyWhitespace: false,
            name: 'StartDate',
            dateFormat: 'd.m.Y',
            fieldLabel: l10n.ns('tpm', 'Promo').value('StartDate'),
        }, {
            xtype: 'datefield', allowBlank: false, allowOnlyWhitespace: false,
            name: 'EndDate',
            dateFormat: 'd.m.Y',
            fieldLabel: l10n.ns('tpm', 'Promo').value('EndDate'),
        }, {
            xtype: 'datefield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'DispatchesStart',
            dateFormat: 'd.m.Y',
            fieldLabel: l10n.ns('tpm', 'Promo').value('DispatchesStart'),
        }, {
            xtype: 'datefield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'DispatchesEnd',
            dateFormat: 'd.m.Y',
            fieldLabel: l10n.ns('tpm', 'Promo').value('DispatchesEnd'),
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('BudgetItemBudgetName'),
            name: 'BudgetItemBudgetName'
        }, {
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('BudgetItemName'),
            name: 'BudgetItemId',
            selectorWidget: 'budgetitem',
            valueField: 'Id',
            displayField: 'Name',
            needUpdateMappings: true,
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.budgetitem.BudgetItem',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.budgetitem.BudgetItem',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'BudgetItemName'
            }, {
                from: 'BudgetName',
                to: 'BudgetItemBudgetName'
            }]
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: false, allowOnlyWhitespace: false,
            name: 'Plan',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('Plan'),
        }, {
            xtype: 'numberfield', allowDecimals: false, allowExponential: false, allowBlank: false, allowOnlyWhitespace: false,
            name: 'Fact',
            minValue: 0,
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('Fact'),
        }]
    }]
});
