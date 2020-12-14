Ext.define('App.view.tpm.promo.Promo', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.promo',
    id: 'promoGrid',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Promo'),
    // Для того чтобы показывать полные записи в удалённых и истории
    baseModel: Ext.ModelManager.getModel('App.model.tpm.promo.Promo'),
    getDefaultResource: function () {
        return 'PromoGridViews';
    },
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
                    glyph: 0xf21d,
                    itemId: 'exportxlsxbutton',
                    exactlyModelCompare: true,
                    text: l10n.ns('core', 'additionalMenu').value('exportXLSX'),
                    action: 'ExportXLSX'
                }]
            }
        }
    ],
    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right',
        items: [{
            xtype: 'widthexpandbutton',
            ui: 'fill-gray-button-toolbar',
            text: l10n.ns('core', 'selectablePanelButtons').value('toolbarCollapse'),
            glyph: 0xf13d,
            glyph1: 0xf13e,
            target: function () {
                return this.up('toolbar');
            },
            toggleCollapse: function () {
                var target = this.getTarget();
                var isCollapsed = this.isCollapsed();
                target.setWidth(isCollapsed ? target.maxWidth : target.minWidth);
                if (isCollapsed) {
                    target.down('#createbutton').setUI('create-promo-btn-toolbar-expanded');
                    target.down('#createbutton').setText(l10n.ns('tpm', 'Schedule').value('CreateExpanded'));



                } else {
                    target.down('#createbutton').setUI('create-promo-btn-toolbar');
                    target.down('#createbutton').setText(l10n.ns('tpm', 'Schedule').value('CreateCollapsed'));
                }
                target.isExpanded = !target.isExpanded;
            },
        },

            {
                itemId: 'createbutton',
                action: 'Post',
                glyph: 0xf0f3,
                text: l10n.ns('tpm', 'Promo').value('CreateCollapsed'),
                tooltip: l10n.ns('tpm', 'Promo').value('CreateCollapsed'),
                ui: 'create-promo-btn'
            },
       //     {
       //     itemId: 'createbutton',
       //     action: 'Post',
       //     glyph: 0xf0f3,
       //     text: l10n.ns('tpm', 'Promo').value('CreateCollapsed'),
       //     tooltip: l10n.ns('tpm', 'Promo').value('CreateCollapsed'),
       //     ui: 'create-promo-btn'
       // }, {
       //     itemId: 'createinoutbutton',
       //     action: 'Post',
       //     glyph: 0xf0f3,
       //     text: l10n.ns('tpm', 'Promo').value('CreateInOutCollapsed'),
       //     tooltip: l10n.ns('tpm', 'Promo').value('CreateInOutCollapsed'),
       //     ui: 'create-promo-btn'
            // },
            {
            itemId: 'updatebutton',
            action: 'Patch',
            glyph: 0xf64f,
            text: l10n.ns('core', 'crud').value('updateButtonText'),
            tooltip: l10n.ns('core', 'crud').value('updateButtonText')
        }, {
            itemId: 'deletebutton',
            action: 'Delete',
            glyph: 0xf5e8,
            text: l10n.ns('core', 'crud').value('deleteButtonText'),
            tooltip: l10n.ns('core', 'crud').value('deleteButtonText')
        }, {
            itemId: 'historybutton',
            resource: 'Historical{0}',
            action: 'Get{0}',
            glyph: 0xf2da,
            text: l10n.ns('core', 'crud').value('historyButtonText'),
            tooltip: l10n.ns('core', 'crud').value('historyButtonText')
        }, '-', {
            itemId: 'extfilterbutton',
            glyph: 0xf349,
            text: l10n.ns('core', 'toptoolbar').value('filterButtonText'),
            tooltip: l10n.ns('core', 'toptoolbar').value('filterButtonText')
        }, {
            itemId: 'deletedbutton',
            resource: 'Deleted{0}',
            action: 'Get{0}',
            glyph: 0xf258,
            text: l10n.ns('core', 'toptoolbar').value('deletedButtonText'),
            tooltip: l10n.ns('core', 'toptoolbar').value('deletedButtonText')
        }, {
            itemId: 'canchangestateonlybutton',
            action: 'GetCanChangeState{0}',
            glyph: 0xf5c7,
            text: l10n.ns('tpm', 'Promo').value('ChangeStateOnlyButtonText'),
            tooltip: l10n.ns('tpm', 'Promo').value('ChangeStateOnlyButtonText'),
        }, {
            itemId: 'canchangeresponsible',
            resource: 'Promoes',
            action: 'ChangeResponsible',
            glyph: 0xf00f,
            text: l10n.ns('tpm', 'Promo').value('ChangeResponsible'),
            tooltip: l10n.ns('tpm', 'Promo').value('ChangeResponsible'),
        },
            '-', '->', '-', {
            itemId: 'extfilterclearbutton',
            ui: 'blue-button-toolbar',
            disabled: true,
            glyph: 0xf232,
            text: l10n.ns('core', 'filter').value('filterEmptyStatus'),
            tooltip: l10n.ns('core', 'filter').value('filterEmptyStatus'),
            overCls: '',
            style: {
                'cursor': 'default'
            }
        }]
    }],
    
    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorWindowModel',
        hasExpandedRows: true,
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.promo.PromoGridView',
            storeId: 'promostore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promo.PromoGridView',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            sorters: [{
                property: 'Number',
                direction: 'DESC'
            }],
            // размер страницы уменьшен для ускорения загрузки грида
            trailingBufferZone: 20,
            leadingBufferZone: 20,
            pageSize: 30
        },
        // стор для получения полной записи промо
        promoStore: Ext.create('App.store.core.SimpleStore', {
            model: 'App.model.tpm.promo.Promo',
            storeId: 'gridviewpromostore',
            autoLoad: false,
        }),
        plugins: [{
            ptype: 'filterbar',
            renderHidden: false,
            showShowHideButton: false,
            showClearAllButton: false,
            enableOperators: false
        }, {
            ptype: 'clearfilterbutton',
            tooltipText: l10n.ns('core').value('filterBarClearButtonText'),
            iconCls: 'mdi mdi-filter-remove'
        }, {
            ptype: 'bufferedrenderer'
        }, {
            ptype: 'maskbinder',
            msg: l10n.ns('core').value('loadingText')
        }, {
            ptype: 'rowexpander',
            rowBodyTpl: Ext.create('App.view.tpm.common.promoDashboardTpl').formatTpl,
            expandOnDblClick: false
        }],

        listeners: {
            viewready: function (grid) {
                //перестановка expander столбца на 0 позицию
                var ind = 0,
                    expandedCol;
                var columns = grid.headerCt.query('gridcolumn');
                columns.forEach(function (col, index) {
                    if (col.innerCls === "x-grid-cell-inner-row-expander") {
                        ind = index;
                        expandedCol = col;
                    }
                });
                if (ind !== 0) {
                    for (var i = ind; i > 0; i--) {
                        grid.headerCt.insert(i, columns[i - 1]);
                    }
                }
                grid.headerCt.insert(0, expandedCol);

                grid.expandedRows = 0;

                grid.view.on('expandBody', function (rowNode, record, expandRow, eOpts) {
                    grid.expandedRows++;
                    grid.view.refresh();
                });
                grid.view.on('collapseBody', function (rowNode, record, expandRow, eOpts) {
                    grid.expandedRows--;
                    grid.view.refresh();
                })
            }
        },

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
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
                text: l10n.ns('tpm', 'Promo').value('Adjustment'),
                dataIndex: 'DeviationCoefficient'
            }, {
                text: l10n.ns('tpm', 'Promo').value('ApolloExport'),
                dataIndex: 'IsApolloExport',
                renderer: function (value) {
                    return value ? l10n.ns('core', 'booleanValues').value('true') : l10n.ns('core', 'booleanValues').value('false');
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
                dataIndex: 'PromoEventName',
                width: 110,
            }, {
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
                xtype: 'numbercolumn',
                text: l10n.ns('tpm', 'Promo').value('PlanPromoUpliftPercent'),
                dataIndex: 'PlanPromoUpliftPercent',
                width: 110,
            }, {
                xtype: 'numbercolumn',
                text: l10n.ns('tpm', 'Promo').value('PlanPromoIncrementalLSV'),
                dataIndex: 'PlanPromoIncrementalLSV',
                width: 110,
            }, {
                xtype: 'numbercolumn',
                text: l10n.ns('tpm', 'Promo').value('PlanPromoBaselineLSV'),
                dataIndex: 'PlanPromoBaselineLSV',
                width: 110,
            }, {
                text: l10n.ns('tpm', 'Promo').value('Mechanic'),
                dataIndex: 'Mechanic',
                width: 130,
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
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Promo').value('StartDate'),
                dataIndex: 'StartDate',
                width: 105,
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
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
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Promo').value('EndDate'),
                dataIndex: 'EndDate',
                width: 100,
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            }, {
                text: l10n.ns('tpm', 'Promo').value('MarsEndDate'),
                dataIndex: 'MarsEndDate',
                width: 125,
                filter: {
                    xtype: 'marsdatefield',
                    operator: 'like'
                }
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Promo').value('DispatchesStart'),
                dataIndex: 'DispatchesStart',
                width: 130,
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            }, {
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
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'Promo').value('DispatchesEnd'),
                dataIndex: 'DispatchesEnd',
                width: 115,
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            }, {
                text: l10n.ns('tpm', 'Promo').value('MarsDispatchesEnd'),
                dataIndex: 'MarsDispatchesEnd',
                width: 155,
                filter: {
                    xtype: 'marsdatefield',
                    operator: 'like'
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
                }, ]
        }
    }]
});