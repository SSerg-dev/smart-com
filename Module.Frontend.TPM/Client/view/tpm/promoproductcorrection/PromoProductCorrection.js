﻿Ext.define('App.view.tpm.promoproductcorrection.PromoProductCorrection', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.promoproductcorrection',
    id: 'promoProductCorrectionGrid',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoProductCorrection'),
    baseModel: Ext.ModelManager.getModel('App.model.tpm.promoproductcorrection.PromoProductCorrection'),
    getDefaultResource: function () {
        return 'PromoProductCorrectionViews';
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
                items: [
                    {
                        glyph: 0xf220,
                        itemId: 'loadimportppcbutton',
                        exactlyModelCompare: true,
                        text: l10n.ns('core', 'additionalMenu').value('fullImportXLSX'),
                        resource: '{0}',
                        action: 'FullImportXLSX',
                        allowFormat: ['zip', 'xlsx']
                    },
                    {
                        glyph: 0xf21d,
                        itemId: 'customloadimporttemplatebutton',
                        exactlyModelCompare: true,
                        text: l10n.ns('core', 'additionalMenu').value('importTemplateXLSX'),
                        action: 'DownloadTemplateXLSX',
                    },
                    {
                        glyph: 0xf21d,
                        itemId: 'exportbutton',
                        exactlyModelCompare: true,
                        text: l10n.ns('core', 'additionalMenu').value('exportXLSX'),
                        action: 'ExportXLSX'
                    },
                    {
                        glyph: 0xf21d,
                        itemId: 'exportcorrectionxlsxbutton',
                        exactlyModelCompare: true,
                        text: l10n.ns('tpm', 'PromoProductCorrection').value('ExportCorrections'),
                        action: 'ExportCorrectionXLSX'
                    }
                ]
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
        }, '-', {
            itemId: 'extfilterbutton',
            glyph: 0xf349,
            text: l10n.ns('core', 'toptoolbar').value('filterButtonText'),
            tooltip: l10n.ns('core', 'toptoolbar').value('filterButtonText')
        },
            '-',
        {
            itemId: 'historybutton',
            resource: 'Historical{0}',
            action: 'Get{0}',
            glyph: 0xf2da,
            text: l10n.ns('core', 'crud').value('historyButtonText'),
            tooltip: l10n.ns('core', 'crud').value('historyButtonText')
        }, '-',
        {
            itemId: 'deletebutton',
            action: 'Delete',
            glyph: 0xf5e8,
            text: l10n.ns('core', 'crud').value('deleteButtonText'),
            tooltip: l10n.ns('core', 'crud').value('deleteButtonText')
        }, '-',
        {
            itemId: 'deletedbutton',
            resource: 'Deleted{0}',
            action: 'Get{0}',
            glyph: 0xf258,
            text: l10n.ns('core', 'toptoolbar').value('deletedButtonText'),
            tooltip: l10n.ns('core', 'toptoolbar').value('deletedButtonText')
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
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.promoproductcorrection.PromoProductCorrectionView',
            storeId: 'promoproductcorrectionstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promoproductcorrection.PromoProductCorrectionView',
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
        // стор для получения полной записи PromoProductCorrection
        promoProductCorrectionStore: Ext.create('App.store.core.SimpleStore', {
            model: 'App.model.tpm.promoproductcorrection.PromoProductCorrection',
            storeId: 'gridviewpromoproductcorrectionstore',
            autoLoad: false,
        }),
        
        columns: {
            defaults: {
                plugins: ['sortbutton'],
                menuDisabled: true,
                filter: true,
                flex: 1,
                minWidth: 100
            },
            items: [
                {
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('TPMmode'),
                    dataIndex: 'TPMmode',
                    filter: {
                        type: 'combo',
                        valueField: 'id',
                        displayField: 'alias',
                        store: {
                            type: 'modestore'
                        },
                        operator: 'eq'
                    }
                },
                {

                    text: l10n.ns('tpm', 'PromoProductCorrection').value('Number'),
                    dataIndex: 'Number',

                },
                {
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('ClientHierarchy'),
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
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('BrandTech'),
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
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('ProductSubrangesList'),
                    dataIndex: 'ProductSubrangesList',
                    width: 110,
                },
                {
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('Mechanic'),
                    dataIndex: 'MarsMechanicName',
                    width: 130,
                },
                {
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('Event'),
                    dataIndex: 'EventName',
                    width: 110,
                },
                {
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('Status'),
                    dataIndex: 'PromoStatusSystemName',
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
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('MarsStartDate'),
                    dataIndex: 'MarsStartDate',
                    width: 120,
                },
                {
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('MarsEndDate'),
                    dataIndex: 'MarsEndDate',
                    width: 120,
                },
                {
                    xtype: 'numbercolumn',
                    format: '0.00',
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('PlanProductBaselineLSV'),
                    dataIndex: 'PlanProductBaselineLSV'
                },
                {
                    xtype: 'numbercolumn',
                    format: '0.00',
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('PlanProductIncrementalLSV'),
                    dataIndex: 'PlanProductIncrementalLSV'
                },
                {
                    xtype: 'numbercolumn',
                    format: '0.00',
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('PlanProductLSV'),
                    dataIndex: 'PlanProductLSV'
                },
                {
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('ZREP'),
                    dataIndex: 'ZREP',
                    filter: {
                        type: 'search',
                        selectorWidget: 'product',
                        valueField: 'ZREP',
                        store: {
                            type: 'directorystore',
                            model: 'App.model.tpm.product.Product',
                            extendedFilter: {
                                xclass: 'App.ExtFilterContext',
                                supportedModels: [{
                                    xclass: 'App.ExtSelectionFilterModel',
                                    model: 'App.model.tpm.product.Product',
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
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('PlanProductUpliftPercentCorrected'),
                    dataIndex: 'PlanProductUpliftPercentCorrected',
                    renderer: function (value) {
                        if (value !== null && value !== undefined) {
                            return Ext.util.Format.number(value, '0.00');
                        } else {
                            return null;
                        }
                    }
                },
                {
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('CreateDate'),
                    dataIndex: 'CreateDate',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y')
                },
                {
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('ChangeDate'),
                    dataIndex: 'ChangeDate',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y')
                },
                {
                    text: l10n.ns('tpm', 'PromoProductCorrection').value('UserName'),
                    dataIndex: 'UserName'
                }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promoproductcorrection.PromoProductCorrectionView',
        items: []
    }]
});
