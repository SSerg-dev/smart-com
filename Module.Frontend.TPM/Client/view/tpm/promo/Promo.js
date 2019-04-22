Ext.define('App.view.tpm.promo.Promo', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.promo',
    id: 'promoGrid',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Promo'),

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
        }, {
            itemId: 'createbutton',
            action: 'Post',
            glyph: 0xf415,
            text: l10n.ns('core', 'crud').value('createButtonText'),
            tooltip: l10n.ns('core', 'crud').value('createButtonText')
        }, {
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
        }, '-', '->', '-', {
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

                //обновление вертикальной прокрутки при разворачивании/сворачивании строки 
                grid.view.on('expandBody', function (rowNode, record, expandRow, eOpts) {
                    grid.view.refresh();
                });

                grid.view.on('collapseBody', function (rowNode, record, expandRow, eOpts) {
                    grid.view.refresh();
                });
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
                dataIndex: 'PromoEventName',
                width: 110,
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
        },
    }]
});