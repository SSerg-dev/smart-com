Ext.define('App.view.tpm.actualLSV.ActualLSV', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.actuallsv',
    title: l10n.ns('tpm', 'ActualLSV').value('ActualLSV'),

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
            },/* {
                glyph: 0xf21d,
                itemId: 'loadimporttemplatexlsxbutton',
                exactlyModelCompare: true,
                text: l10n.ns('core', 'additionalMenu').value('importTemplateXLSX'),
                action: 'DownloadTemplateXLSX'
            }, */{
                glyph: 0xf21d,
                itemId: 'exportxlsxbutton',
                exactlyModelCompare: true,
                text: l10n.ns('core', 'additionalMenu').value('exportXLSX'),
                action: 'ExportXLSX'
            }]
        }
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.actualLSV.ActualLSV',
            storeId: 'actuallsvstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.actualLSV.ActualLSV',
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

        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
                flex: 1,
                minWidth: 100
            },
            items: [{
                text: l10n.ns('tpm', 'ActualLSV').value('Number'),
                dataIndex: 'Number',
                width: 110
            }, {
                text: l10n.ns('tpm', 'ActualLSV').value('ClientHierarchy'),
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
                text: l10n.ns('tpm', 'ActualLSV').value('Name'),
                dataIndex: 'Name',
                width: 150,
            }, {
                text: l10n.ns('tpm', 'ActualLSV').value('InOut'),
                dataIndex: 'InOut',
                width: 150
            }, {
                text: l10n.ns('tpm', 'ActualLSV').value('BrandTech'),
                dataIndex: 'BrandTech',
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
                text: l10n.ns('tpm', 'ActualLSV').value('Event'),
                dataIndex: 'Event',
                width: 110,
            }, {
                text: l10n.ns('tpm', 'ActualLSV').value('Mechanic'),
                dataIndex: 'Mechanic',
                width: 130,
            }, {
                text: l10n.ns('tpm', 'ActualLSV').value('MechanicIA'),
                dataIndex: 'MechanicIA',
                width: 110,
            }, {
                xtype: 'datecolumn',
                text: l10n.ns('tpm', 'ActualLSV').value('StartDate'),
                dataIndex: 'StartDate',
                width: 105,
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            }, {
                text: l10n.ns('tpm', 'ActualLSV').value('MarsStartDate'),
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
                text: l10n.ns('tpm', 'ActualLSV').value('EndDate'),
                dataIndex: 'EndDate',
                width: 100,
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            }, {
                text: l10n.ns('tpm', 'ActualLSV').value('MarsEndDate'),
                dataIndex: 'MarsEndDate',
                width: 125,
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
                text: l10n.ns('tpm', 'ActualLSV').value('DispatchesStart'),
                dataIndex: 'DispatchesStart',
                width: 130,
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            }, {
                text: l10n.ns('tpm', 'ActualLSV').value('MarsDispatchesStart'),
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
                text: l10n.ns('tpm', 'ActualLSV').value('DispatchesEnd'),
                dataIndex: 'DispatchesEnd',
                width: 115,
                renderer: Ext.util.Format.dateRenderer('d.m.Y'),
            }, {
                text: l10n.ns('tpm', 'ActualLSV').value('MarsDispatchesEnd'),
                dataIndex: 'MarsDispatchesEnd',
                width: 155,
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
                text: l10n.ns('tpm', 'ActualLSV').value('Status'),
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
            },
            {
                text: l10n.ns('tpm', 'ActualLSV').value('ActualInStoreDiscount'),
                dataIndex: 'ActualInStoreDiscount'
            },
            {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'ActualLSV').value('PlanPromoUpliftPercent'),
                dataIndex: 'PlanPromoUpliftPercent'
            },
            {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'ActualLSV').value('ActualPromoUpliftPercent'),
                dataIndex: 'ActualPromoUpliftPercent'
            },
            {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'ActualLSV').value('PlanPromoBaselineLSV'),
                dataIndex: 'PlanPromoBaselineLSV'
            },
            {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'ActualLSV').value('ActualPromoBaselineLSV'),
                dataIndex: 'ActualPromoBaselineLSV'
            },
            {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'ActualLSV').value('PlanPromoIncrementalLSV'),
                dataIndex: 'PlanPromoIncrementalLSV'
            },
            {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'ActualLSV').value('ActualPromoIncrementalLSV'),
                dataIndex: 'ActualPromoIncrementalLSV'
            },
            {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'ActualLSV').value('PlanPromoLSV'),
                dataIndex: 'PlanPromoLSV'
            },
            {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'ActualLSV').value('ActualPromoLSVByCompensation'),
                dataIndex: 'ActualPromoLSVByCompensation'
            },
            {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'ActualLSV').value('ActualPromoLSV'),
                dataIndex: 'ActualPromoLSV'
            },
            {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'ActualLSV').value('ActualPromoLSVSI'),
                dataIndex: 'ActualPromoLSVSI'
            },
            {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'ActualLSV').value('ActualPromoLSVSO'),
                dataIndex: 'ActualPromoLSVSO'
            },
            {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'ActualLSV').value('PlanPromoPostPromoEffectLSVW1'),
                dataIndex: 'PlanPromoPostPromoEffectLSVW1'
            },
            {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'ActualLSV').value('ActualPromoPostPromoEffectLSVW1'),
                dataIndex: 'ActualPromoPostPromoEffectLSVW1'
            },
            {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'ActualLSV').value('PlanPromoPostPromoEffectLSVW2'),
                dataIndex: 'PlanPromoPostPromoEffectLSVW2'
            },
            {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'ActualLSV').value('ActualPromoPostPromoEffectLSVW2'),
                dataIndex: 'ActualPromoPostPromoEffectLSVW2'
            },
            {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'ActualLSV').value('PlanPromoPostPromoEffectLSV'),
                dataIndex: 'PlanPromoPostPromoEffectLSV'
            },
            {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'ActualLSV').value('ActualPromoPostPromoEffectLSV'),
                dataIndex: 'ActualPromoPostPromoEffectLSV'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.actualLSV.ActualLSV',
        items: []
    }]
});
