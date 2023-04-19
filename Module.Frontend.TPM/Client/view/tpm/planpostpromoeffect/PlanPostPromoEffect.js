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
        }, {
            xtype: 'textfield',
            name: 'Size',
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('Size')
            /*editable: false,
            displayField: 'Name',
            entityType: 'RPASetting',
            queryMode: 'local',
            valueField: 'Id',
            forceSelection: true,
            allowBlank: false,
            allowOnlyWhitespace: false,
            store: {
                type: 'simplestore',
                autoLoad: false,
                model: 'App.model.core.rpasetting.RPASetting',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.core.rpasetting.RPASetting',
                        modelId: 'efselectionmodel'
                    }]
                }
            },*/
        }, {
            xtype: 'combobox',
            editable: false,
            name: 'DiscountRangeId',
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('DiscountRangeName'),
            queryMode: 'remote',
            valueField: 'Id',
            forceSelection: true,
            allowBlank: false,
            allowOnlyWhitespace: false,
            store: {
                type: 'simplestore',
                autoLoad: false,
                model: 'App.model.tpm.discountrange.DiscountRange',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.discountrange.DiscountRange',
                        modelId: 'efselectionmodel'
                    }]
                }
            }
        }, {
            xtype: 'combobox',
            editable: false,
            name: 'DurationRangeId',
            fieldLabel: l10n.ns('tpm', 'PlanPostPromoEffect').value('DurationRangeName'),
            queryMode: 'remote',
            valueField: 'Id',
            forceSelection: true,
            allowBlank: false,
            allowOnlyWhitespace: false,
            store: {
                type: 'simplestore',
                autoLoad: false,
                model: 'App.model.tpm.durationrange.DurationRange',
                extendedFilter: {
                    xclass: 'App.ExtFilterContext',
                    supportedModels: [{
                        xclass: 'App.ExtSelectionFilterModel',
                        model: 'App.model.tpm.durationrange.DurationRange',
                        modelId: 'efselectionmodel'
                    }]
                }
            }
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
