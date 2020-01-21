Ext.define('App.view.tpm.promo.choosepromo', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.choosepromo',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Promo'),

    dockedItems: [{
        xtype: 'readonlydirectorytoolbar',
        dock: 'right'
    }],

    systemHeaderItems: [],
    customHeaderItems: [],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        multiSelect: true,
        editorModel: 'Core.form.EditorWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.promo.Promo',
            storeId: 'promostore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promo.Promo',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            sorters: [{
                property: 'Number',
                direction: 'DESC'
            }]
        },

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true
            },
            items: [{
                text: l10n.ns('tpm', 'Promo').value('Number'),
                dataIndex: 'Number',
                width: 110
            }, {    
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
                    crudAccess: ['Administrator', 'SupportAdministrator', 'FunctionalExpert', 'CMManager', 'CustomerMarketing', 'KeyAccountManager'],
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
                dataIndex: 'BrandTechName',
                width: 120,
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
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Promo').value('StartDate'),
                dataIndex: 'StartDate',
                width: 110,
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Promo').value('EndDate'),
                dataIndex: 'EndDate',
                width: 100,
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
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
        },
         
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promo.Promo',
        items: []
    }]
});