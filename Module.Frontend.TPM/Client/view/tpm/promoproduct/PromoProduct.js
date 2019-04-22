Ext.define('App.view.tpm.promoproduct.PromoProduct', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.promoproduct',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PromoProduct'),

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
                    text: 'Полный импорт XLSX',
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
                    text: 'Экспорт в XLSX',
                    action: 'ExportXLSX'
                }]
            }
        }
    ],

    dockedItems: [{
        xtype: 'custombigtoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.promoproduct.PromoProduct',
            storeId: 'actualstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.promoproduct.PromoProduct',
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
                minWidth: 110
            },
            items: [{
                text: l10n.ns('tpm', 'PromoProduct').value('EAN'),
                dataIndex: 'EAN'
            }, {
                xtype: 'numbercolumn',
				format: '0',
                text: l10n.ns('tpm', 'PromoProduct').value('ActualProductPCQty'),
                dataIndex: 'ActualProductPCQty'
            }, {
                xtype: 'numbercolumn',
				format: '0.00',
                text: l10n.ns('tpm', 'PromoProduct').value('ActualProductQty'),
                dataIndex: 'ActualProductQty'
            }, {
                text: l10n.ns('tpm', 'PromoProduct').value('ActualProductUOM'),
                dataIndex: 'ActualProductUOM'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'PromoProduct').value('ActualProductShelfPrice'),
                dataIndex: 'ActualProductShelfPrice'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'PromoProduct').value('ActualProductPCLSV'),
                dataIndex: 'ActualProductPCLSV'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.promoproduct.PromoProduct',
        items: []
    }]
});
