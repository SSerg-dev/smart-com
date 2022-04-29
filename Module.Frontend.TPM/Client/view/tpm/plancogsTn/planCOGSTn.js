Ext.define('App.view.tpm.plancogstn.PlanCOGSTn', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.plancogstn',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PlanCOGSTn'),

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
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.plancogstn.PlanCOGSTn',
            storeId: 'plancogstnstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.plancogstn.PlanCOGSTn',
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
                text: l10n.ns('tpm', 'PlanCOGSTn').value('StartDate'),
                dataIndex: 'StartDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'PlanCOGSTn').value('EndDate'),
                dataIndex: 'EndDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'PlanCOGSTn').value('Year'),
                dataIndex: 'Year'
            }, {
                text: l10n.ns('tpm', 'PlanCOGSTn').value('ClientTreeFullPathName'),
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
                text: l10n.ns('tpm', 'PlanCOGSTn').value('ClientTreeObjectId'),
                dataIndex: 'ClientTreeObjectId'
            }, {
                text: l10n.ns('tpm', 'PlanCOGSTn').value('BrandTechName'),
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
                text: l10n.ns('tpm', 'PlanCOGSTn').value('Volume'),
                dataIndex: 'Volume'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.plancogstn.PlanCOGSTn',
        items: [{
            xtype: 'datefield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'PlanCOGSTn').value('StartDate'),
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
            fieldLabel: l10n.ns('tpm', 'PlanCOGSTn').value('EndDate'),
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
            fieldLabel: l10n.ns('tpm', 'PlanCOGSTn').value('ClientTreeFullPathName'),
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
            fieldLabel: l10n.ns('tpm', 'PlanCOGSTn').value('BrandTechName'),
            name: 'BrandTechName',
            selectorWidget: 'brandtech',
            valueField: 'Id',
            displayField: 'BrandsegTechsub',
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
                from: 'BrandsegTechsub',
                to: 'BrandTechName'
            }]
        }
            , {
            xtype: 'numberfield',
                name: 'Volume',
                fieldLabel: l10n.ns('tpm', 'PlanCOGSTn').value('Volume'),
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
        }]
    }]
});
