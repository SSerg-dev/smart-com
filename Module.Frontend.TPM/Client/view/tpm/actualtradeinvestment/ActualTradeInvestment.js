Ext.define('App.view.tpm.actualtradeinvestment.ActualTradeInvestment', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.actualtradeinvestment',
    title: l10n.ns('tpm', 'compositePanelTitles').value('ActualTradeInvestment'),

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
            itemId: 'recalculateactualtipreviousyearbutton',
            glyph: 0xf0ec,
            action: 'PreviousYearPromoList',
            text: l10n.ns('tpm', 'ActualTradeInvestment').value('recalculatePreviousYearButtonYearText'),
            tooltip: l10n.ns('tpm', 'ActualTradeInvestment').value('recalculatePreviousYearButtonYearText'),
            disabled: true
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
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.actualtradeinvestment.ActualTradeInvestment',
            storeId: 'actualtradeinvestmentstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.actualtradeinvestment.ActualTradeInvestment',
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
                text: l10n.ns('tpm', 'ActualTradeInvestment').value('StartDate'),
                dataIndex: 'StartDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'ActualTradeInvestment').value('EndDate'),
                dataIndex: 'EndDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'ActualTradeInvestment').value('Year'),
                dataIndex: 'Year'
            }, {
                text: l10n.ns('tpm', 'ActualTradeInvestment').value('ClientTreeFullPathName'),
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
                text: l10n.ns('tpm', 'ActualTradeInvestment').value('ClientTreeObjectId'),
                dataIndex: 'ClientTreeObjectId'
            }, {
                text: l10n.ns('tpm', 'ActualTradeInvestment').value('BrandTechName'),
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
                text: l10n.ns('tpm', 'ActualTradeInvestment').value('TIType'),
                dataIndex: 'TIType'
            }, {
                text: l10n.ns('tpm', 'ActualTradeInvestment').value('TISubType'),
                dataIndex: 'TISubType'
            }, {
                text: l10n.ns('tpm', 'ActualTradeInvestment').value('SizePercent'),
                dataIndex: 'SizePercent'
            }, {
                xtype: 'booleancolumn',
                text: l10n.ns('tpm', 'ActualTradeInvestment').value('MarcCalcROI'),
                dataIndex: 'MarcCalcROI',
                trueText: l10n.ns('core', 'booleanValues').value('true'),
                falseText: l10n.ns('core', 'booleanValues').value('false')
            }, {
                xtype: 'booleancolumn',
                text: l10n.ns('tpm', 'ActualTradeInvestment').value('MarcCalcBudgets'),
                dataIndex: 'MarcCalcBudgets',
                trueText: l10n.ns('core', 'booleanValues').value('true'),
                falseText: l10n.ns('core', 'booleanValues').value('false')
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.actualtradeinvestment.ActualTradeInvestment',
        items: [{
            xtype: 'datefield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'ActualTradeInvestment').value('StartDate'),
            readOnly: false,
            editable: true,
            format: 'd.m.Y',
            listeners: {
                afterrender: function () {
                    this.setValue(new Date());
                },
                change: function (newValue, oldValue) {
                    var toDate = this.up('form').down('[name=EndDate]');
                    toDate.setMinValue(newValue.getValue());
                }
            }
        }, {
            xtype: 'datefield',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'ActualTradeInvestment').value('EndDate'),
            readOnly: false,
            editable: true,
            format: 'd.m.Y',
            listeners: {
                afterrender: function () {
                    this.setValue(new Date());
                },
                change: function (newValue, oldValue) {
                    var fromDate = this.up('form').down('[name=StartDate]');
                    fromDate.setMaxValue(newValue.getValue());
                }
            }
        }, {
            xtype: 'treesearchfield',
            name: 'ClientTreeId',
            fieldLabel: l10n.ns('tpm', 'ActualTradeInvestment').value('ClientTreeFullPathName'),
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
            xtype: 'searchfield',
            fieldLabel: l10n.ns('tpm', 'ActualTradeInvestment').value('BrandTechName'),
            name: 'BrandTechName',
            selectorWidget: 'brandtech',
            valueField: 'Id',
            displayField: 'Name',
            allowBlank: true,
            allowOnlyWhitespace: true,
            onTrigger2Click: function () {
                var technology = this.up().down('[name=BrandTechName]');

                this.clearValue();
                technology.setValue(null);
            },
            listeners: {
                afterrender: function (field) {
                    if (!field.value) {
                        field.value = null;
                    }
                },
                change: function (field, newValue, oldValue) {
                    var brandtech = field.up().down('[name=BrandTechName]');
                    var brandtechValue = newValue ? field.record.get('BrandTechId') : null;

                    brandtech.setValue(brandtechValue);
                }
            },
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.brandtech.BrandTech',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.brandtech.BrandTech',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'BrandTechName'
            }]
        }, {
            xtype: 'textfield',            name: 'TIType',            fieldLabel: l10n.ns('tpm', 'ActualTradeInvestment').value('TIType'),
        }, {
            xtype: 'textfield',            name: 'TISubType',            fieldLabel: l10n.ns('tpm', 'ActualTradeInvestment').value('TISubType'),
        }, {
            xtype: 'numberfield',
            name: 'SizePercent',
            fieldLabel: l10n.ns('tpm', 'ActualTradeInvestment').value('SizePercent'),
            minValue: 0,
            maxValue: 100,
            readOnly: true,
            allowBlank: false,
            listeners: {
                change: function (newValue, oldValue) {
                    if (newValue > 100) {
                        this.setValue(oldValue);
                    }
                },
            }
        }, {
            xtype: 'booleancombobox',
            store: {
                type: 'booleannonemptystore'
            },
            name: 'MarcCalcROI',
            fieldLabel: l10n.ns('tpm', 'ActualTradeInvestment').value('MarcCalcROI')
        }, {
            xtype: 'booleancombobox',
            store: {
                type: 'booleannonemptystore'
            },
            name: 'MarcCalcBudgets',
            fieldLabel: l10n.ns('tpm', 'ActualTradeInvestment').value('MarcCalcBudgets')
        }]
    }]
});