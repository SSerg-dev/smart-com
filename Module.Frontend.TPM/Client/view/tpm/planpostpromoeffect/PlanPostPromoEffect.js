Ext.define('App.view.tpm.planpostpromoeffect.PlanPostPromoEffect', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.planpostpromoeffect',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PlanPostPromoEffect'),

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
            model: 'App.model.tpm.planpostpromoeffect.PlanPostPromoEffect',
            storeId: 'planpostpromoeffectstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.planpostpromoeffect.PlanPostPromoEffect',
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
                text: l10n.ns('tpm', 'PlanPostPromoEffect').value('ClientTreeObjectId'),
                dataIndex: 'ClientTreeObjectId'
            }, {
                text: l10n.ns('tpm', 'PlanPostPromoEffect').value('ClientTreeFullPathName'),
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
                text: l10n.ns('tpm', 'PlanPostPromoEffect').value('BrandTechName'),
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
            }, {
                text: l10n.ns('tpm', 'PlanPostPromoEffect').value('Size'),
                dataIndex: 'Size'
            }, {
                text: l10n.ns('tpm', 'PlanPostPromoEffect').value('DiscountRangeName'),
                dataIndex: 'DiscountRangeName'
            }, {
                text: l10n.ns('tpm', 'PlanPostPromoEffect').value('DurationRangeName'),
                dataIndex: 'DurationRangeName'
            }, {
                text: l10n.ns('tpm', 'PlanPostPromoEffect').value('PlanPostPromoEffectW1'),
                dataIndex: 'PlanPostPromoEffectW1'
            }, {
                text: l10n.ns('tpm', 'PlanPostPromoEffect').value('PlanPostPromoEffectW2'),
                dataIndex: 'PlanPostPromoEffectW2'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.planpostpromoeffect.PlanPostPromoEffect',
        items: [{
            xtype: 'treesearchfield',
            name: 'ClientTreeId',
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('ClientTreeFullPathName'),
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
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('BrandTechName'),
            name: 'BrandTechId',
            selectorWidget: 'brandtech',
            allowBlank: true,
            allowOnlyWhitespace: true,
            valueField: 'Id',
            displayField: 'BrandsegTechsub',
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
                    var brandtechCode = newValue ? field.record.get('BrandTech_code') : null;

                    brandtech.setValue(brandtechValue);

                    if (brandtechCode != null) {
                        var planPostPromoEffectController = App.app.getController('tpm.planpostpromoeffect.PlanPostPromoEffect');
                        planPostPromoEffectController.getBrandTechSizes(brandtechCode);
                    }
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
        }, {
            xtype: 'combobox',
            editable: false,
            name: 'Size',
            id: 'SizeComboBox',
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('Size'),
            valueField: 'size',
            displayField: 'size',
            queryMode: 'local',
            selectOnFocus: true,
            allowBlank: false,
            allowOnlyWhitespace: false,
            store: Ext.create('Ext.data.Store', {
                fields: ['size'],
                data: [
                    { size: '10g' },
                    { size: '800g' },
                    { size: '300g' }
                ]
            }),
            mapping: [{
                from: 'size',
                to: 'size'
            }]
        }, {
            xtype: 'searchcombobox',
            editable: false,
            name: 'DiscountRangeId',
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('DiscountRangeName'),
            selectorWidget: 'discountrange',
            valueField: 'Id',
            displayField: 'Name',
            selectOnFocus: true,
            entityType: 'DiscountRange',
            allowBlank: false,
            allowOnlyWhitespace: false,
            store: {
                type: 'simplestore',
                autoLoad: true,
                model: 'App.model.tpm.discountrange.DiscountRange'
            },
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.discountrange.DiscountRange',
                    modelId: 'efselectionmodel'
                }]
            },
            mapping: [{
                from: 'Name',
                to: 'DiscountRangeName'
            }]
        }, {
            xtype: 'searchcombobox',
            itemId: 'duration-range',
            editable: false,
            name: 'DurationRangeId',
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('DurationRangeName'),
            selectorWidget: 'durationrange',
            valueField: 'Id',
            displayField: 'Name',
            selectOnFocus: true,
            entityType: 'DurationRange',
            allowBlank: false,
            allowOnlyWhitespace: false,
            store: {
                type: 'simplestore',
                autoLoad: true,
                model: 'App.model.tpm.durationrange.DurationRange'
            },
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.durationrange.DurationRange',
                    modelId: 'efselectionmodel'
                }]
            },
            mapping: [{
                from: 'Name',
                to: 'DurationRangeName'
            }]
        }, {
            xtype: 'numberfield',
            name: 'PlanPostPromoEffectW1',
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('PlanPostPromoEffectW1')
        }, {
            xtype: 'numberfield',
            name: 'PlanPostPromoEffectW2',
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('PlanPostPromoEffectW2')
        }]
    }]
});
