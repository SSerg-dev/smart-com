Ext.define('App.view.tpm.promoproductcorrection.PromoProductCorrection', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.promoproductcorrection',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoProductCorrection'),

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
                    action: 'DownloadTemplateXLSX',
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
            model: 'App.model.tpm.promoproductcorrection.PromoProductCorrection',
            storeId: 'promoproductcorrectionstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promoproductcorrection.PromoProductCorrection',
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
        },

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

                    text: l10n.ns('tpm', 'PromoProductCorrection').value('Number'),
                    dataIndex: 'Number',

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
        model: 'App.model.tpm.PromoProductCorrection.PromoProductCorrection',
        items: [{

        }]
    }]
});
