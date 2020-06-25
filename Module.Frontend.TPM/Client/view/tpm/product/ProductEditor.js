Ext.define('App.view.tpm.product.ProductEditor', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.producteditor',
    width: 800,
    minWidth: 800,
    maxHeight: 600,
    cls: 'readOnlyFields',

    items: {
        xtype: 'editorform',
        items: [{
            xtype: 'textfield',
            name: 'ZREP',
            fieldLabel: l10n.ns('tpm', 'Product').value('ZREP'),
            maxLength: 255,
        }, {
            xtype: 'textfield',
            name: 'EAN_Case',
            vtype: 'eanNum',
            fieldLabel: l10n.ns('tpm', 'Product').value('EAN_Case'),
            maxLength: 255,
        }, {
            xtype: 'textfield',
            name: 'EAN_PC',
            vtype: 'eanNum',
            fieldLabel: l10n.ns('tpm', 'Product').value('EAN_PC'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ProductEN',
            fieldLabel: l10n.ns('tpm', 'Product').value('ProductEN'),
            maxLength: 255,
        }, {
            //--
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'Brand',
            fieldLabel: l10n.ns('tpm', 'Product').value('Brand'),
            maxLength: 255,
            readOnly: true,
            listeners: {
                afterrender: function (field) {
                    field.addCls('readOnlyField');
                },
                writeablechange: function (field) {
                    if (field.readOnly == false) {
                        field.setReadOnly(true);
                        field.addCls('readOnlyField');
                    }
                }
            }
        }, {
            xtype: 'searchfield',
            name: 'Brand_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('Brand_code'),
            maxLength: 255,
            regex: /^\d+$/,
            regexText: l10n.ns('tpm', 'Brand').value('DigitRegex'),
            selectorWidget: 'brand',
            valueField: 'Brand_code',
            displayField: 'Brand_code',
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.brand.Brand',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.brand.Brand',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Brand_code',
                to: 'Brand_code'
            }]
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'Technology',
            fieldLabel: l10n.ns('tpm', 'Product').value('Technology'),
            maxLength: 255,
            readOnly: true,
            listeners: {
                afterrender: function (field) {
                    field.addCls('readOnlyField');
                },
                writeablechange: function (field) {
                    if (field.readOnly == false) {
                        field.setReadOnly(true);
                        field.addCls('readOnlyField');
                    }
                }
            }
        }, {
            xtype: 'searchfield',
            name: 'Tech_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('Tech_code'),
            maxLength: 255,
            regex: /^\d+$/,
            regexText: l10n.ns('tpm', 'Brand').value('DigitRegex'),
            selectorWidget: 'technology',
            valueField: 'Tech_code',
            displayField: 'Tech_code',
            onSelectButtonClick: function (button) {
                var picker = this.picker,
                    selModel = picker.down(this.selectorWidget).down('grid').getSelectionModel(),
                    record = selModel.hasSelection() ? selModel.getSelection()[0] : null;

                this.setValue(record);
                if (this.needUpdateMappings) {
                    this.updateMappingValues(record);
                }
                this.afterSetValue(record);

                this.up('editorform').down('[name=SubBrand_code]').setValue(record.get('SubBrand_code'));
                this.up('editorform').down('[name=SubBrand]').setValue(record.get('SubBrand'));

                picker.close();
            },
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.technology.Technology',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.technology.Technology',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Tech_code',
                to: 'Tech_code'
            }]
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'BrandTech',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandTech'),
            maxLength: 255,
            readOnly: true,
            listeners: {
                afterrender: function (field) {
                    field.addCls('readOnlyField');
                },
                writeablechange: function (field) {
                    if (field.readOnly == false) {
                        field.setReadOnly(true);
                        field.addCls('readOnlyField');
                    }
                }
            }
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'BrandTech_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandTech_code'),
            maxLength: 255,
            readOnly: true,
            listeners: {
                afterrender: function (field) {
                    field.addCls('readOnlyField');
                },
                writeablechange: function (field) {
                    if (field.readOnly == false) {
                        field.setReadOnly(true);
                        field.addCls('readOnlyField');
                    }
                }
            }
        }, {
            xtype: 'textfield',
            name: 'Segmen_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('Segmen_code'),
            maxLength: 255,
            regex: /^\d+$/,
            regexText: l10n.ns('tpm', 'Brand').value('DigitRegex')
        }, {
            //--
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'BrandsegTech_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandsegTech_code'),
            maxLength: 255,
            readOnly: true,
            listeners: {
                afterrender: function (field) {
                    field.addCls('readOnlyField');
                },
                writeablechange: function (field) {
                    if (field.readOnly == false) {
                        field.setReadOnly(true);
                        field.addCls('readOnlyField');
                    }
                }
            }
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'Brandsegtech',
            fieldLabel: l10n.ns('tpm', 'Product').value('Brandsegtech'),
            maxLength: 255,
            readOnly: true,
            listeners: {
                afterrender: function (field) {
                    field.addCls('readOnlyField');
                },
                writeablechange: function (field) {
                    if (field.readOnly == false) {
                        field.setReadOnly(true);
                        field.addCls('readOnlyField');
                    }
                }
            }
        }, {
            //--
            xtype: 'textfield',
            name: 'BrandsegTechsub_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandsegTechsub_code'),
            maxLength: 255,
            readOnly: true,
            listeners: {
                afterrender: function (field) {
                    field.addCls('readOnlyField');
                },
                writeablechange: function (field) {
                    if (field.readOnly == false) {
                        field.setReadOnly(true);
                        field.addCls('readOnlyField');
                    }
                }
            }
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'BrandsegTechsub',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandsegTechsub'),
            maxLength: 255,
            readOnly: true,
            listeners: {
                afterrender: function (field) {
                    field.addCls('readOnlyField');
                },
                writeablechange: function (field) {
                    if (field.readOnly == false) {
                        field.setReadOnly(true);
                        field.addCls('readOnlyField');
                    }
                }
            }
        }, {
            xtype: 'textfield',
            name: 'SubBrand_code',
            fieldLabel: l10n.ns('tpm', 'Product').value('SubBrand_code'),
            maxLength: 255,
            readOnly: true,
            listeners: {
                afterrender: function (field) {
                    field.addCls('readOnlyField');
                },
                writeablechange: function (field) {
                    if (field.readOnly == false) {
                        field.setReadOnly(true);
                        field.addCls('readOnlyField');
                    }
                }
            }
        }, {
            xtype: 'textfield',
            name: 'SubBrand',
            fieldLabel: l10n.ns('tpm', 'Product').value('SubBrand'),
            maxLength: 255,
            readOnly: true,
            listeners: {
                afterrender: function (field) {
                    field.addCls('readOnlyField');
                },
                writeablechange: function (field) {
                    if (field.readOnly == false) {
                        field.setReadOnly(true);
                        field.addCls('readOnlyField');
                    }
                }
            }
        }, {
            xtype: 'textfield',
            name: 'BrandFlagAbbr',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandFlagAbbr'),
            maxLength: 255,
        }, {
            xtype: 'textfield',
            name: 'BrandFlag',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandFlag'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'SubmarkFlag',
            fieldLabel: l10n.ns('tpm', 'Product').value('SubmarkFlag'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'IngredientVariety',
            fieldLabel: l10n.ns('tpm', 'Product').value('IngredientVariety'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ProductCategory',
            fieldLabel: l10n.ns('tpm', 'Product').value('ProductCategory'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ProductType',
            fieldLabel: l10n.ns('tpm', 'Product').value('ProductType'),
            maxLength: 255,
        }, {
            xtype: 'textfield',
            name: 'MarketSegment',
            fieldLabel: l10n.ns('tpm', 'Product').value('MarketSegment'),
            maxLength: 255,
        }, {
            xtype: 'textfield',
            name: 'SupplySegment',
            fieldLabel: l10n.ns('tpm', 'Product').value('SupplySegment'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'FunctionalVariety',
            fieldLabel: l10n.ns('tpm', 'Product').value('FunctionalVariety'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'Size',
            fieldLabel: l10n.ns('tpm', 'Product').value('Size'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'BrandEssence',
            fieldLabel: l10n.ns('tpm', 'Product').value('BrandEssence'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'PackType',
            fieldLabel: l10n.ns('tpm', 'Product').value('PackType'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'GroupSize',
            fieldLabel: l10n.ns('tpm', 'Product').value('GroupSize'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'TradedUnitFormat',
            fieldLabel: l10n.ns('tpm', 'Product').value('TradedUnitFormat'),
            maxLength: 255,
        }, {
            xtype: 'textfield', allowBlank: true, allowOnlyWhitespace: true,
            name: 'ConsumerPackFormat',
            fieldLabel: l10n.ns('tpm', 'Product').value('ConsumerPackFormat'),
            maxLength: 255,
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
            minLength: 0,
            maxLength: 9,
            allowBlank: true,
            allowOnlyWhitespace: true
        }]
    },
    listeners: {
        afterrender: function (window) {
            if (Ext.ComponentQuery.query('inoutselectionproductwindow')[0]) {
                window.down('#edit').setVisible(false);
            }
        }
    }
});
