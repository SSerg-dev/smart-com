Ext.define('App.view.tpm.promo.PromoMechanicAddPromoes', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.promomechanicaddpromoes',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Promo'),
    width: "95%",
    height: "95%",
    minWidth: 800,
    minHeight: 600,

    PromoId: null,
    StartDateFilter: null,
    ClientTreeId: null,    

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
    ],

    dockedItems: [],

    items: [
        {
            xtype: 'directorygrid',
            itemId: 'datatable',
            multiSelect: true,
            editorModel: 'Core.form.EditorDetailWindowModel',
            store: {
                type: 'simplestore',
                model: 'App.model.tpm.promo.Promo',
                storeId: 'customizemechanicactualstore',
                autoLoad: false,
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [
                        {
                            xclass: 'App.ExtSelectionFilterModel',
                            model: 'App.model.tpm.promo.Promo',
                            modelId: 'efselectionmodel'
                        },
                        {
                            xclass: 'App.ExtTextFilterModel',
                            modelId: 'eftextmodel'
                        }
                    ]
                }
            },

            columns: {
                defaults: {
                    plugins: ['sortbutton'],
                    menuDisabled: true,
                    filter: true,
                    flex: 1,
                    minWidth: 110
                },
                items: [
                    {
                        text: l10n.ns('tpm', 'Promo').value('Number'),
                        dataIndex: 'Number',
                        width: 110
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('ClientHierarchy'),
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
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('InOut'),
                        dataIndex: 'InOut',
                        renderer: function (value) {
                            return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                        }
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('InvoiceType'),
                        dataIndex: 'IsOnInvoice',
                        width: 100,
                        xtype: 'booleancolumn',
                        trueText: l10n.ns('tpm', 'InvoiceTypes').value('OnInvoice'),
                        falseText: l10n.ns('tpm', 'InvoiceTypes').value('OffInvoice'),
                        filter: {
                            type: 'bool',
                            store: [
                                [true, l10n.ns('tpm', 'InvoiceTypes').value('OnInvoice')],
                                [false, l10n.ns('tpm', 'InvoiceTypes').value('OffInvoice')]
                            ]
                        }
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('GrowthAcceleration'),
                        dataIndex: 'IsGrowthAcceleration',
                        renderer: function (value) {
                            return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                        }
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('Adjustment'),
                        dataIndex: 'DeviationCoefficient'
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('ApolloExport'),
                        dataIndex: 'IsApolloExport',
                        renderer: function (value) {
                            return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                        }
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('Name'),
                        dataIndex: 'Name',
                        width: 150,
                    },
                    {
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
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('EventName'),
                        dataIndex: 'PromoEventName',
                        width: 110,
                    },
                    {
                        xtype: 'datecolumn',
                        text: l10n.ns('tpm', 'Promo').value('LastChangedDate'),
                        dataIndex: 'LastChangedDate',
                        width: 130,
                        renderer: Ext.util.Format.dateRenderer('d.m.Y H:i'),
                        hidden: true
                    },
                    {
                        xtype: 'datecolumn',
                        text: l10n.ns('tpm', 'Promo').value('LastChangedDateDemand'),
                        dataIndex: 'LastChangedDateDemand',
                        width: 130,
                        renderer: Ext.util.Format.dateRenderer('d.m.Y H:i'),
                        hidden: true
                    },
                    {
                        xtype: 'datecolumn',
                        text: l10n.ns('tpm', 'Promo').value('LastChangedDateFinance'),
                        dataIndex: 'LastChangedDateFinance',
                        width: 130,
                        renderer: Ext.util.Format.dateRenderer('d.m.Y H:i'),
                        hidden: true
                    },
                    {
                        xtype: 'numbercolumn',
                        text: l10n.ns('tpm', 'Promo').value('PlanPromoUpliftPercent'),
                        dataIndex: 'PlanPromoUpliftPercent',
                        width: 110,
                    },
                    {
                        xtype: 'numbercolumn',
                        text: l10n.ns('tpm', 'Promo').value('PlanPromoIncrementalLSV'),
                        dataIndex: 'PlanPromoIncrementalLSV',
                        width: 110,
                    },
                    {
                        xtype: 'numbercolumn',
                        text: l10n.ns('tpm', 'Promo').value('PlanPromoBaselineLSV'),
                        dataIndex: 'PlanPromoBaselineLSV',
                        width: 110,
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('Mechanic'),
                        dataIndex: 'Mechanic',
                        width: 130,
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('MarsMechanicName'),
                        dataIndex: 'MarsMechanicName',
                        width: 130,
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
                        },
                        hidden: true
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('MechanicIA'),
                        dataIndex: 'MechanicIA',
                        width: 110,
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicName'),
                        dataIndex: 'PlanInstoreMechanicName',
                        width: 110,
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
                        },
                        hidden: true
                    },
                    {
                        xtype: 'datecolumn',
                        text: l10n.ns('tpm', 'Promo').value('StartDate'),
                        dataIndex: 'StartDate',
                        width: 105,
                        renderer: Ext.util.Format.dateRenderer('d.m.Y'),
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('MarsStartDate'),
                        dataIndex: 'MarsStartDate',
                        width: 150,
                        filter: {
                            xtype: 'marsdatefield',
                            operator: 'like',
                            validator: function (value) {
                                // дает возможность фильтровать только по году
                                return true;
                            },
                        }
                    },
                    {
                        xtype: 'datecolumn',
                        text: l10n.ns('tpm', 'Promo').value('EndDate'),
                        dataIndex: 'EndDate',
                        width: 100,
                        renderer: Ext.util.Format.dateRenderer('d.m.Y'),
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('MarsEndDate'),
                        dataIndex: 'MarsEndDate',
                        width: 125,
                        filter: {
                            xtype: 'marsdatefield',
                            operator: 'like'
                        }
                    },
                    {
                        xtype: 'datecolumn',
                        text: l10n.ns('tpm', 'Promo').value('DispatchesStart'),
                        dataIndex: 'DispatchesStart',
                        width: 130,
                        renderer: Ext.util.Format.dateRenderer('d.m.Y'),
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('MarsDispatchesStart'),
                        dataIndex: 'MarsDispatchesStart',
                        width: 165,
                        filter: {
                            xtype: 'marsdatefield',
                            operator: 'like',
                            validator: function (value) {
                                // дает возможность фильтровать только по году
                                return true;
                            },
                        }
                    },
                    {
                        xtype: 'datecolumn',
                        text: l10n.ns('tpm', 'Promo').value('DispatchesEnd'),
                        dataIndex: 'DispatchesEnd',
                        width: 115,
                        renderer: Ext.util.Format.dateRenderer('d.m.Y'),
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('MarsDispatchesEnd'),
                        dataIndex: 'MarsDispatchesEnd',
                        width: 155,
                        filter: {
                            xtype: 'marsdatefield',
                            operator: 'like'
                        }
                    },
                    {
                        text: l10n.ns('tpm', 'PromoROIReport').value('BudgetYear'),
                        dataIndex: 'BudgetYear',
                        width: 110,
                        hidden: true,
                    },
                    {
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
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('MarsMechanicTypeName'),
                        dataIndex: 'MarsMechanicTypeName',
                        width: 130,
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
                        },
                        hidden: true,
                    },
                    {
                        xtype: 'numbercolumn',
                        format: '0',
                        text: l10n.ns('tpm', 'Promo').value('MarsMechanicDiscount'),
                        dataIndex: 'MarsMechanicDiscount',
                        width: 130,
                        hidden: true,
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicTypeName'),
                        dataIndex: 'PlanInstoreMechanicTypeName',
                        width: 110,
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
                        },
                        hidden: true,
                    },
                    {
                        xtype: 'numbercolumn',
                        format: '0',
                        text: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicDiscount'),
                        dataIndex: 'PlanInstoreMechanicDiscount',
                        width: 110,
                        hidden: true,
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('BrandName'),
                        dataIndex: 'BrandName',
                        width: 110,
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
                        },
                        hidden: true,
                    },
                    {
                        text: l10n.ns('tpm', 'Promo').value('PromoTypesName'),
                        dataIndex: 'PromoTypesName',
                        width: 130,
                        filter: {
                            type: 'search',
                            selectorWidget: 'promotypes',
                            valueField: 'Name',
                            store: {
                                type: 'directorystore',
                                model: 'App.model.tpm.promotypes.PromoTypes',
                                extendedFilter: {
                                    xclass: 'App.ExtFilterContext',
                                    supportedModels: [{
                                        xclass: 'App.ExtSelectionFilterModel',
                                        model: 'App.model.tpm.promotypes.PromoTypes',
                                        modelId: 'efselectionmodel'
                                    }, {
                                        xclass: 'App.ExtTextFilterModel',
                                        modelId: 'eftextmodel'
                                    }]
                                }
                            }
                        },
                        hidden: false
                    }
                ]
            }
        },

    ],
    buttons: [{
        text: l10n.ns('core', 'buttons').value('cancel'),
        itemId: 'close'
    }, {
        text: l10n.ns('tpm', 'buttons').value('ok'),
        ui: 'green-button-footer-toolbar',
        itemId: 'ok'
    }]
});
