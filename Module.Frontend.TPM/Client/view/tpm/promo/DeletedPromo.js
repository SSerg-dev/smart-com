Ext.define('App.view.tpm.promo.DeletedPromo', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.deletedpromo',
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
            model: 'App.model.tpm.promo.DeletedPromo',
            storeId: 'deletedpromostore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promo.DeletedPromo',
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
                text: l10n.ns('tpm', 'Promo').value('ClientHierarchy'),
                dataIndex: 'ClientHierarchy',
                width: 250,
                filter: {
                    type: 'string',
                    operator: 'conts'
                },
                renderer: function (value) {
                    return renderWithDelimiter(value, ' > ', '  ');
                }
            }, {
                text: l10n.ns('tpm', 'Promo').value('InOut'),
                dataIndex: 'InOut',
                renderer: function (value) {
                    return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                }
            }, {
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
            }, {
                text: l10n.ns('tpm', 'Promo').value('GrowthAcceleration'),
                dataIndex: 'IsGrowthAcceleration',
                renderer: function (value) {
                    return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                }
            }, {
                text: l10n.ns('tpm', 'Promo').value('ApolloExport'),
                dataIndex: 'IsApolloExport',
                renderer: function (value) {
                    return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
                }
            }, {
                text: l10n.ns('tpm', 'Promo').value('Adjustment'),
                dataIndex: 'DeviationCoefficient',
                width: 130,
            }, {
                text: l10n.ns('tpm', 'Promo').value('Name'),
                dataIndex: 'Name',
                width: 150,
            }, {
                text: l10n.ns('tpm', 'Promo').value('PlanPromoUpliftPercent'),
                dataIndex: 'PlanPromoUpliftPercent',
                width: 150,
            }, {
                text: l10n.ns('tpm', 'Promo').value('PlanPromoIncrementalLSV'),
                dataIndex: 'PlanPromoIncrementalLSV',
                width: 150,
            }, {
                text: l10n.ns('tpm', 'Promo').value('PlanPromoBaselineLSV'),
                dataIndex: 'PlanPromoBaselineLSV',
                width: 150,
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
                text: l10n.ns('tpm', 'Promo').value('EventName'),
                dataIndex: 'EventName',
                width: 110,
            },  {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Promo').value('LastChangedDate'),
                dataIndex: 'LastChangedDate',
                width: 130,
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i'),
                hidden: true
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Promo').value('LastChangedDateDemand'),
                dataIndex: 'LastChangedDateDemand',
                width: 130,
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i'),
                hidden: true
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Promo').value('LastChangedDateFinance'),
                dataIndex: 'LastChangedDateFinance',
                width: 130,
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i'),
                hidden: true
            }, {
                text: l10n.ns('tpm', 'Promo').value('Mechanic'),
                dataIndex: 'Mechanic',
                width: 110,
            }, {
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
            }, {
                text: l10n.ns('tpm', 'Promo').value('MechanicIA'),
                dataIndex: 'MechanicIA',
                width: 110,
            }, {
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
            }, {
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
            }, {
                text: l10n.ns('tpm', 'Promo').value('MarsDispatchesStart'),
                dataIndex: 'MarsDispatchesStart',
                width: 170,
                filter: {
                    xtype: 'marsdatefield',
                    operator: 'like',
                    validator: function (value) {
                        // дает возможность фильтровать только по году
                        return true;
                    },
                }
            }, {
                text: l10n.ns('tpm', 'Promo').value('PromoStatusName'),
                dataIndex: 'PromoStatusName',
                width: 120,
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
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Promo').value('MarsMechanicDiscount'),
                dataIndex: 'MarsMechanicDiscount',
                width: 130,
                hidden: true,
            }, {
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
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicDiscount'),
                dataIndex: 'PlanInstoreMechanicDiscount',
                width: 110,
                hidden: true,
            }, {
                text: l10n.ns('tpm', 'Promo').value('MechanicComment'),
                dataIndex: 'MechanicComment',
                width: 100,
                hidden: true,
            }, {
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
            }, {
                text: l10n.ns('tpm', 'Promo').value('MarsEndDate'),
                dataIndex: 'MarsEndDate',
                width: 120,
                hidden: true,
                filter: {
                    xtype: 'marsdatefield',
                    operator: 'like',
                    valueToRaw: function (value) {
                        // в cs между годом и остальной частью добавляется пробел
                        // а в js нет, поэтому добавляем пробел
                        var result = value;

                        if (value) {
                            var stringValue = value.toString();

                            if (stringValue.indexOf('P') >= 0)
                                result = stringValue.replace('P', ' P')
                        }

                        return result;
                    },
                }
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Promo').value('StartDate'),
                dataIndex: 'StartDate',
                width: 110,
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
                hidden: true,
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Promo').value('EndDate'),
                dataIndex: 'EndDate',
                width: 100,
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
                hidden: true,
            }, {
                text: l10n.ns('tpm', 'Promo').value('MarsDispatchesEnd'),
                dataIndex: 'MarsDispatchesEnd',
                width: 120,
                hidden: true,
                valueToRaw: function (value) {
                    // в cs между годом и остальной частью добавляется пробел
                    // а в js нет, поэтому добавляем пробел
                    var result = value;

                    if (value) {
                        var stringValue = value.toString();

                        if (stringValue.indexOf('P') >= 0)
                            result = stringValue.replace('P', ' P')
                    }

                    return result;
                },
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Promo').value('DispatchesStart'),
                dataIndex: 'DispatchesStart',
                width: 115,
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
                hidden: true,
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Promo').value('DispatchesEnd'),
                dataIndex: 'DispatchesEnd',
                width: 115,
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
                hidden: true,
            }, {
                xtype: 'numbercolumn',
                format: '0',
                text: l10n.ns('tpm', 'Promo').value('CalendarPriority'),
                dataIndex: 'CalendarPriority',
                hidden: true,
            }, {
                text: l10n.ns('tpm', 'Promo').value('ColorDisplayName'),
                dataIndex: 'ColorDisplayName',
                renderer: function (value, metaData, record, rowIndex, colIndex, store, view) {
                    var descr = record.get('ColorDisplayName');
                    return Ext.String.format('<div style="background-color:{0};width:50px;height:10px;display:inline-block;margin:0 5px 0 5px;border:solid;border-color:gray;border-width:1px;"></div><div style="display:inline-block">{1}</div>', record.get('ColorSystemName'), descr ? descr : '');
                },
                hidden: true,
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promo.DeletedPromo',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'DeletedDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'BaseDeletedEntity').value('DeletedDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PromoStatusName',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PromoStatusName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ClientHierarchy',
            fieldLabel: l10n.ns('tpm', 'Promo').value('ClientHierarchy'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MarsMechanicName',
            fieldLabel: l10n.ns('tpm', 'Promo').value('MechanicName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MarsMechanicTypeName',
            fieldLabel: l10n.ns('tpm', 'Promo').value('MechanicTypeName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MarsMechanicDiscount',
            fieldLabel: l10n.ns('tpm', 'Promo').value('MechanicDiscount')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanInstoreMechanicName',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanInstoreMechanicTypeName',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicTypeName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanInstoreMechanicDiscount',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanInstoreMechanicDiscount')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'MechanicComment',
            fieldLabel: l10n.ns('tpm', 'Promo').value('MechanicComment')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'Promo').value('StartDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'Promo').value('EndDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DispatchesStart',
            fieldLabel: l10n.ns('tpm', 'Promo').value('DispatchesStart'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DispatchesEnd',
            fieldLabel: l10n.ns('tpm', 'Promo').value('DispatchesEnd'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'EventName',
            fieldLabel: l10n.ns('tpm', 'Promo').value('EventName'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CalendarPriority',
            fieldLabel: l10n.ns('tpm', 'Promo').value('CalendarPriority')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanPromoTIShopper',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoTIShopper'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanPromoTIMarketing',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoTIMarketing'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanPromoBranding',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoBranding'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanPromoCost',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoCost'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanPromoBTL',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoBTL'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanPromoCostProduction',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoCostProduction'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanPromoUpliftPercent',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoUpliftPercent'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanPromoIncrementalLSV',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoIncrementalLSV'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanPromoBaselineLSV',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoBaselineLSV'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanPromoPostPromoEffectLSV',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoPostPromoEffectLSV'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanPromoROIPercent',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoROIPercent'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanPromoIncrementalNSV',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoIncrementalNSV'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanPromoNetIncrementalNSV',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoNetIncrementalNSV'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'PlanPromoIncrementalMAC',
            fieldLabel: l10n.ns('tpm', 'Promo').value('PlanPromoIncrementalMAC'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualPromoTIShopper',
            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoTIShopper'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualPromoTIMarketing',
            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoTIMarketing'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualPromoBranding',
            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoBranding'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualPromoBTL',
            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoBTL'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualPromoCostProduction',
            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoCostProduction'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualPromoCost',
            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoCost'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualPromoUpliftPercent',
            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoUpliftPercent'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualPromoIncrementalLSV',
            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoIncrementalLSV'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualPromoLSV',
            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoLSV'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualPromoLSVByCompensation',
            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoLSVByCompensation'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualPromoPostPromoEffectLSV',
            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoPostPromoEffectLSV'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualPromoROIPercent',
            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoROIPercent'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualPromoIncrementalNSV',
            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoIncrementalNSV'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualPromoNetIncrementalNSV',
            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoNetIncrementalNSV'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ActualPromoIncrementalMAC',
            fieldLabel: l10n.ns('tpm', 'Promo').value('ActualPromoIncrementalMAC'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'InvoiceType',
            fieldLabel: l10n.ns('tpm', 'Promo').value('InvoiceType'),
        }]
    }]
});
