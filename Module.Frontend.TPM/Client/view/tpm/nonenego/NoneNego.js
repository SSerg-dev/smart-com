Ext.define('App.view.tpm.nonenego.NoneNego', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.nonenego',
    title: l10n.ns('tpm', 'compositePanelTitles').value('NoneNego'),
    itemId: 'NoneNego',

    listeners: {
        afterrender: function () {
            this.isEditorForUpdateNode = false;
        }
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
        listeners: {
            afterrender: function () {
                var noneNego = this.up('nonenego');
                var createButton = this.down('#createbutton');
                var updateButton = this.down('#updatebutton');

                createButton.addListener('click', function () {
                    noneNego.isEditorForUpdateNode = false;
                });

                updateButton.addListener('click', function () {
                    noneNego.isEditorForUpdateNode = true;
                });
            }
        }
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',

        store: {
            type: 'directorystore',
            model: 'App.model.tpm.nonenego.NoneNego',
            storeId: 'nonenegostore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.nonenego.NoneNego',
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
                text: l10n.ns('tpm', 'NoneNego').value('ClientTreeFullPathName'),
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
                text: l10n.ns('tpm', 'NoneNego').value('ClientTreeObjectId'),
                dataIndex: 'ClientTreeObjectId'
            }, {
                text: l10n.ns('tpm', 'NoneNego').value('ProductTreeFullPathName'),
                dataIndex: 'ProductTreeFullPathName',
                minWidth: 200,
                filter: {
                    xtype: 'treefsearchfield',
                    trigger2Cls: '',
                    selectorWidget: 'producttree',
                    valueField: 'FullPathName',
                    displayField: 'FullPathName',
                    multiSelect: true,
                    operator: 'conts',
                    store: {
                        model: 'App.model.tpm.producttree.ProductTree',
                        autoLoad: false,
                        root: {}
                    },
                },
                renderer: function (value) {
                    return renderWithDelimiter(value, ' > ', '  ');
                }
            }, {
                text: l10n.ns('tpm', 'NoneNego').value('ProductTreeObjectId'),
                dataIndex: 'ProductTreeObjectId'
            }, {
                text: l10n.ns('tpm', 'NoneNego').value('MechanicName'),
                dataIndex: 'MechanicName',
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
                }
            }, {
                text: l10n.ns('tpm', 'NoneNego').value('MechanicTypeName'),
                dataIndex: 'MechanicTypeName',
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
                }
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'NoneNego').value('Discount'),
                dataIndex: 'Discount',
            }, {
                text: l10n.ns('tpm', 'NoneNego').value('FromDate'),
                dataIndex: 'FromDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'NoneNego').value('ToDate'),
                dataIndex: 'ToDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'NoneNego').value('CreateDate'),
                dataIndex: 'CreateDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.nonenego.NoneNego',

        items: [{
            xtype: 'searchfield',
            name: 'ClientId',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ClientHierarchy'),
            selectorWidget: 'client',
            valueField: 'Id',
            displayField: 'CommercialSubnetCommercialNetName',
            allowBlank: false,
            allowOnlyWhitespace: false,
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.client.Client',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.client.Client',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'ProductEN',
                to: 'Product'
            }]
        }, {
            xtype: 'searchfield',
            name: 'ProductId',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ProductHierarchy'),
            selectorWidget: 'product',
            valueField: 'Id',
            displayField: 'ProductEN',
            allowBlank: false,
            allowOnlyWhitespace: false,
            store: {
                type: 'directorystore',
                model: 'App.model.tpm.product.Product',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.product.Product',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'ProductEN',
                to: 'Product'
            }]
        }, {
            xtype: 'searchcombobox',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('MechanicName'),
            name: 'MechanicId',
            selectorWidget: 'mechanic',
            valueField: 'Id',
            displayField: 'Name',
            entityType: 'Mechanic',
            allowBlank: false,
            allowOnlyWhitespace: false,
            store: {
                type: 'simplestore',
                autoLoad: false,
                model: 'App.model.tpm.mechanic.Mechanic',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.mechanic.Mechanic',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            onTrigger3Click: function () {
                var mechanicType = this.up().down('[name=MechanicTypeId]');
                var discount = this.up().down('[name=Discount]');

                this.reset();
                mechanicType.reset();
                discount.reset();
            },
            mapping: [{
                from: 'Name',
                to: 'Mechanic'
            }],
            listeners: {
                select: function (combo, records) {
                    var mechanicType = this.up('form').down('[name=MechanicTypeId]');
                    var discount = this.up('form').down('[name=Discount]');

                    if (combo.record.data.Name === 'TPR' || combo.record.data.Name === 'Other') {
                        mechanicType.setReadOnly(true);
                        discount.setReadOnly(false);
                        mechanicType.reset();
                        discount.reset();
                    } else if (combo.record.data.Name === 'VP') {
                        mechanicType.setReadOnly(false);
                        discount.setReadOnly(true);
                        mechanicType.reset();
                        discount.reset();
                    }
                }
            }
        }, {
            xtype: 'searchcombobox',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('MechanicTypeName'),
            name: 'MechanicTypeId',
            selectorWidget: 'mechanictype',
            valueField: 'Id',
            displayField: 'Name',
            entityType: 'MechanicType',
            allowBlank: false,
            allowOnlyWhitespace: false,
            readOnly: true,
            store: {
                type: 'simplestore',
                autoLoad: false,
                model: 'App.model.tpm.mechanictype.MechanicType',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.mechanictype.MechanicType',
                        modelId: 'efselectionmodel'
                    }]
                }
            },
            mapping: [{
                from: 'Name',
                to: 'MechanicType'
            }]
        }, {
            xtype: 'numberfield',
            name: 'Discount',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('Discount'),
            allowBlank: true,
            allowOnlyWhitespace: true,
            minValue: 0,
            maxValue: 100,
            allowBlank: false,
            allowOnlyWhitespace: false,
            allowDecimals: true,
            readOnly: true
        }, {
            xtype: 'datefield',
            name: 'FromDate',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('FromDate'),
            allowBlank: true,
            allowOnlyWhitespace: true,
            listeners: {
                change: function (newValue, oldValue) {
                    var toDate = this.up('form').down('[name=ToDate]');
                    toDate.setMinValue(newValue.getValue());
                }
            }
        }, {
            xtype: 'datefield',
            name: 'ToDate',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('ToDate'),
            allowBlank: true,
            allowOnlyWhitespace: true,
            listeners: {
                change: function (newValue, oldValue) {
                    var fromDate = this.up('form').down('[name=FromDate]');
                    fromDate.setMaxValue(newValue.getValue());
                }
            }
        }, {
            xtype: 'datefield',
            name: 'CreateDate',
            fieldLabel: l10n.ns('tpm', 'NoneNego').value('CreateDate'),
            allowBlank: true,
            allowOnlyWhitespace: true,
        }]
    }]
});