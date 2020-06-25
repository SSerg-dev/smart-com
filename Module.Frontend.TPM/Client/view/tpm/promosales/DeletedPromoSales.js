Ext.define('App.view.tpm.promosales.DeletedPromoSales', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedpromosales',
    title: l10n.ns('core', 'compositePanelTitles').value('deletedPanelTitle'),

    dockedItems: [{
        xtype: 'readonlydeleteddirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',

        store: {
            type: 'directorystore',
            model: 'App.model.tpm.promosales.DeletedPromoSales',
            storeId: 'deletedpromosalesstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promosales.DeletedPromoSales',
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
                dataIndex: 'StartDate'
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'PromoSales').value('EndDate'),
                dataIndex: 'EndDate'
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'PromoSales').value('DispatchesStart'),
                dataIndex: 'DispatchesStart'
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'PromoSales').value('DispatchesEnd'),
                dataIndex: 'DispatchesEnd'
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
        model: 'App.model.tpm.promosales.DeletedPromoSales',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('Number'),
            name: 'Number'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('Name'),
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('MechanicName'),
            name: 'MechanicName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('MechanicTypeName'),
            name: 'MechanicTypeName',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('MechanicDiscount'),
            name: 'MechanicDiscount',
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('ClientCommercialSubnetCommercialNetName'),
            name: 'ClientCommercialSubnetCommercialNetName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('BrandName'),
            name: 'BrandName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('BrandTechName'),
            name: 'BrandTechName'
        }, {
            xtype: 'singlelinedisplayfield',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('PromoStatusName'),
            name: 'PromoStatusName'
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('StartDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('EndDate'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DispatchesStart',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('DispatchesStart'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DispatchesEnd',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('DispatchesEnd'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BudgetItemBudgetName',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('BudgetItemBudgetName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'BudgetItemName',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('BudgetItemName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Plan',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('Plan'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Fact',
            fieldLabel: l10n.ns('tpm', 'PromoSales').value('Fact'),
        }]
    }]
});
