Ext.define('App.view.tpm.postpromoeffect.PostPromoEffect', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.postpromoeffect',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PostPromoEffect'),

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
            model: 'App.model.tpm.postpromoeffect.PostPromoEffect',
            storeId: 'postpromoeffectstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.postpromoeffect.PostPromoEffect',
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
                text: l10n.ns('tpm', 'PostPromoEffect').value('StartDate'),
                dataIndex: 'StartDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'PostPromoEffect').value('EndDate'),
                dataIndex: 'EndDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'PostPromoEffect').value('ClientTreeFullPathName'),
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
                text: l10n.ns('tpm', 'PostPromoEffect').value('ProductTreeFullPathName'),
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
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'PostPromoEffect').value('EffectWeek1'),
                dataIndex: 'EffectWeek1'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'PostPromoEffect').value('EffectWeek2'),
                dataIndex: 'EffectWeek2'
            }, {
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'PostPromoEffect').value('TotalEffect'),
                dataIndex: 'TotalEffect'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.postpromoeffect.PostPromoEffect',
        items: [{
            xtype: 'datefield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('StartDate'),
            allowBlank: false,
            allowOnlyWhitespace: false,
            editable: false,
            format: 'd.m.Y'        
        }, {
            xtype: 'datefield',
            name: 'EndDate',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('EndDate'),
            allowBlank: false,
            allowOnlyWhitespace: false,
            editable: false,
            format: 'd.m.Y'        
        }, {
            xtype: 'treesearchfield',
            name: 'ClientTreeId',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('ClientTreeFullPathName'),
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
            xtype: 'treesearchfield',
            name: 'ProductTreeId',
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('ProductTreeFullPathName'),
            selectorWidget: 'producttree',
            valueField: 'Id',
            displayField: 'FullPathName',
            store: {
                storeId: 'clienttreestore',
                model: 'App.model.tpm.producttree.ProductTree',
                autoLoad: false,
                root: {}
            },
            mapping: [{
                from: 'FullPathName',
                to: 'ProductTreeFullPathName'
            }]
        }, {
            xtype: 'numberfield',
            name: 'EffectWeek1',
            allowDecimals: true,
            allowExponential: false,
            minValue: -10000000000,
            maxValue: 10000000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('EffectWeek1'),
        }, {
            xtype: 'numberfield',
            name: 'EffectWeek2',
            allowDecimals: true,
            allowExponential: false,
            minValue: -10000000000,
            maxValue: 10000000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('EffectWeek2'),
        }, {
            xtype: 'numberfield',
            name: 'TotalEffect',
            allowDecimals: true,
            allowExponential: false,
            minValue: -10000000000,
            maxValue: 10000000000,
            allowBlank: true,
            allowOnlyWhitespace: true,
            fieldLabel: l10n.ns('tpm', 'PostPromoEffect').value('TotalEffect'),
        }]
    }]
});
