Ext.define('App.view.tpm.btl.BTLPromo', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.btlpromo',
    title: l10n.ns('tpm', 'BTLPromo').value('ViewTitle'),

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
    ],

    dockedItems: [{
        xtype: 'addonlydirectorytoolbar',
        dock: 'right'
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'associateddirectorystore',
            model: 'App.model.tpm.btl.BTLPromo',
            storeId: 'associatedbtlpromostore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.btl.BTLPromo',
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
                text: l10n.ns('tpm', 'BTLPromo').value('PromoNumber'),
                dataIndex: 'PromoNumber',
            }, {
                text: l10n.ns('tpm', 'BTLPromo').value('TPMmode'),
                dataIndex: 'TPMmode',
                renderer: function (value) {
                    return value;
                },
                xtype: 'booleancolumn',
                trueText: 'RS',
                falseText: 'Current',
                filter: {
                    type: 'bool',
                    store: [
                        [0, 'Current'],
                        [1, 'RS']
                    ]
                }
            }, {
                text: l10n.ns('tpm', 'BTL').value('PlanPromoBTL'),
                dataIndex: 'PlanPromoBTL'
            }, {
                text: l10n.ns('tpm', 'BTL').value('ActualPromoBTL'),
                dataIndex: 'ActualPromoBTL'
            }, {
                text: l10n.ns('tpm', 'BTLPromo').value('PromoName'),
                dataIndex: 'PromoName',
            },  {
                text: l10n.ns('tpm', 'BTLPromo').value('PromoEventName'),
                dataIndex: 'PromoEventName',
                filter: {
                    type: 'search',
                    selectorWidget: 'event',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.event.Event',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.tpm.event.Event',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            },  {
                text: l10n.ns('tpm', 'BTLPromo').value('PromoBrandTechName'),
                dataIndex: 'PromoBrandTechName',
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
            },  {
                text: l10n.ns('tpm', 'BTLPromo').value('PromoStatusName'),
                dataIndex: 'PromoStatusName',
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
            },  {
                text: l10n.ns('tpm', 'BTLPromo').value('ClientTreeFullPathName'),
                dataIndex: 'ClientTreeFullPathName',
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
            },  {
                text: l10n.ns('tpm', 'BTLPromo').value('PromoStartDate'),
                dataIndex: 'PromoStartDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            },  {
                text: l10n.ns('tpm', 'BTLPromo').value('PromoEndDate'),
                dataIndex: 'PromoEndDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.btl.BTLPromo',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'PromoName',
            fieldLabel: 'Promo Name'
        }]
    }]

});