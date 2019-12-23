Ext.define('App.view.tpm.product.Product', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.product',
    title: l10n.ns('tpm', 'compositePanelTitles').value('Product'),

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
            resource: 'Products',
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
    items: [{
        xtype: 'directorygrid',
        //alias: 'widget.productgrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.product.Product',
            storeId: 'productstore',
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
            },
            sorters: [{
                property: 'ZREP',
                direction: 'ASC'
            }]
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
                text: l10n.ns('tpm', 'Product').value('ZREP'),
                dataIndex: 'ZREP'
            }, {
                text: l10n.ns('tpm', 'Product').value('EAN_Case'),
                dataIndex: 'EAN_Case'
            },{
                text: l10n.ns('tpm', 'Product').value('EAN_PC'),
                dataIndex: 'EAN_PC'
            }, {
                text: l10n.ns('tpm', 'Product').value('ProductEN'),
                dataIndex: 'ProductEN'
            }, {
				//--
				text: l10n.ns('tpm', 'Product').value('Brand'),
                dataIndex: 'Brand'
            }, {
                text: l10n.ns('tpm', 'Product').value('Brand_code'),
                dataIndex: 'Brand_code'
            }, {
                text: l10n.ns('tpm', 'Product').value('Technology'),
                dataIndex: 'Technology'
            }, {
                text: l10n.ns('tpm', 'Product').value('Tech_code'),
                dataIndex: 'Tech_code'
            }, {
                text: l10n.ns('tpm', 'Product').value('BrandTech'),
                dataIndex: 'BrandTech'
            }, {
                text: l10n.ns('tpm', 'Product').value('BrandTech_code'),
                dataIndex: 'BrandTech_code'
            }, {
                text: l10n.ns('tpm', 'Product').value('Segmen_code'),
                dataIndex: 'Segmen_code'
            }, {
				//--
                text: l10n.ns('tpm', 'Product').value('BrandsegTech_code'),
                dataIndex: 'BrandsegTech_code'
            }, {
                text: l10n.ns('tpm', 'Product').value('Brandsegtech'),
                dataIndex: 'Brandsegtech'
            }, {
                text: l10n.ns('tpm', 'Product').value('BrandFlagAbbr'),
                dataIndex: 'BrandFlagAbbr'
            }, {
                text: l10n.ns('tpm', 'Product').value('BrandFlag'),
                dataIndex: 'BrandFlag'
            }, {
                text: l10n.ns('tpm', 'Product').value('SubmarkFlag'),
                dataIndex: 'SubmarkFlag'
            }, {
                text: l10n.ns('tpm', 'Product').value('IngredientVariety'),
                dataIndex: 'IngredientVariety'
            }, {
                text: l10n.ns('tpm', 'Product').value('ProductCategory'),
                dataIndex: 'ProductCategory'
            }, {
                text: l10n.ns('tpm', 'Product').value('ProductType'),
                dataIndex: 'ProductType'
            }, {
                text: l10n.ns('tpm', 'Product').value('MarketSegment'),
                dataIndex: 'MarketSegment'
            }, {
                text: l10n.ns('tpm', 'Product').value('SupplySegment'),
                dataIndex: 'SupplySegment'
            }, {
                text: l10n.ns('tpm', 'Product').value('FunctionalVariety'),
                dataIndex: 'FunctionalVariety'
            }, {
                text: l10n.ns('tpm', 'Product').value('Size'),
                dataIndex: 'Size'
            }, {
                text: l10n.ns('tpm', 'Product').value('BrandEssence'),
                dataIndex: 'BrandEssence'
            }, {
                text: l10n.ns('tpm', 'Product').value('PackType'),
                dataIndex: 'PackType'
            }, {
                text: l10n.ns('tpm', 'Product').value('GroupSize'),
                dataIndex: 'GroupSize'
            }, {
                text: l10n.ns('tpm', 'Product').value('TradedUnitFormat'),
                dataIndex: 'TradedUnitFormat'
            }, {
                text: l10n.ns('tpm', 'Product').value('ConsumerPackFormat'),
                dataIndex: 'ConsumerPackFormat'
            }, {
                text: l10n.ns('tpm', 'Product').value('UOM_PC2Case'),
                dataIndex: 'UOM_PC2Case'
            }, {
                text: l10n.ns('tpm', 'Product').value('Division'),
                dataIndex: 'Division'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.product.Product',
        items: [{
            xtype: 'textfield',
            name: 'ZREP',
            fieldLabel: l10n.ns('tpm', 'Product').value('ZREP'),
        }, {
            xtype: 'textfield',
            name: 'EAN_Case',
            vtype: 'eanNum',
            fieldLabel: l10n.ns('tpm', 'Product').value('EAN_Case'),
        }, {
            xtype: 'textfield',
            name: 'EAN_PC',
            vtype: 'eanNum',
            fieldLabel: l10n.ns('tpm', 'Product').value('EAN_PC'),
        },  {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ProductEN',
            fieldLabel: l10n.ns('tpm', 'Product').value('ProductEN'),
        }, {
			//--
			xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'Brand',
            fieldLabel: l10n.ns('tpm', 'Product').value('Brand'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'Brand_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('Brand_code'),
        }, {
            xtype: 'textfield',
            name: 'Technology',
            fieldLabel: l10n.ns('tpm', 'Product').value('Technology'),
        }, {
            xtype: 'textfield',
            name: 'Tech_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('Tech_code'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'BrandTech',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandTech'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'BrandTech_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandTech_code'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'Segmen_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('Segmen_code'),
        }, {
			//--
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'BrandsegTech_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandsegTech_code'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'Brandsegtech',
            fieldLabel: l10n.ns('tpm', 'Product').value('Brandsegtech'),
        }, {
            xtype: 'textfield',
            name: 'BrandFlagAbbr',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandFlagAbbr'),
        }, {
            xtype: 'textfield',
            name: 'BrandFlag',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandFlag'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'SubmarkFlag',
            fieldLabel: l10n.ns('tpm', 'Product').value('SubmarkFlag'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'IngredientVariety',
            fieldLabel: l10n.ns('tpm', 'Product').value('IngredientVariety'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ProductCategory',
            fieldLabel: l10n.ns('tpm', 'Product').value('ProductCategory'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ProductType',
            fieldLabel: l10n.ns('tpm', 'Product').value('ProductType'),
        }, {
            xtype: 'textfield',
            name: 'MarketSegment',
            fieldLabel: l10n.ns('tpm', 'Product').value('MarketSegment'),
        }, {
            xtype: 'textfield',
            name: 'SupplySegment',
            fieldLabel: l10n.ns('tpm', 'Product').value('SupplySegment'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'FunctionalVariety',
            fieldLabel: l10n.ns('tpm', 'Product').value('FunctionalVariety'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'Size',
            fieldLabel: l10n.ns('tpm', 'Product').value('Size'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'BrandEssence',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandEssence'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'PackType',
            fieldLabel: l10n.ns('tpm', 'Product').value('PackType'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'GroupSize',
            fieldLabel: l10n.ns('tpm', 'Product').value('GroupSize'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'TradedUnitFormat',
            fieldLabel: l10n.ns('tpm', 'Product').value('TradedUnitFormat'),
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ConsumerPackFormat',
            fieldLabel: l10n.ns('tpm', 'Product').value('ConsumerPackFormat'),
        }, {
            xtype: 'numberfield',
            name: 'UOM_PC2Case',
            fieldLabel: l10n.ns('tpm', 'Product').value('UOM_PC2Case'),
            allowDecimals: false,
            allowExponential: false,
            minValue: 0,
            maxValue: 999999999,
            enforceMaxLength: true,
            maxLength: 9
        }, {
            xtype: 'numberfield',
            name: 'Division',
            fieldLabel: l10n.ns('tpm', 'Product').value('Division'),
            allowDecimals: false,
            allowExponential: false,
            minValue: 0,
            maxValue: 999999999,
            enforceMaxLength: true,
            maxLength: 9
        }]
    }]
});
