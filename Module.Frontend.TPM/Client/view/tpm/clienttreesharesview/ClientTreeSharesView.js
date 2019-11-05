Ext.define('App.view.tpm.clienttreesharesview.ClientTreeSharesView', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.clienttreesharesview',
    title: l10n.ns('tpm', 'compositePanelTitles').value('ClientTreeSharesView'),

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
            model: 'App.model.tpm.clienttreesharesview.ClientTreeSharesView',
            storeId: 'clienttreesharesviewstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.clienttreesharesview.ClientTreeSharesView',
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
                text: l10n.ns('tpm', 'ClientTreeSharesView').value('ResultNameStr'),
                dataIndex: 'ResultNameStr',
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
                text: l10n.ns('tpm', 'ClientTreeSharesView').value('BOI'),
                dataIndex: 'BOI'
            }, {
                text: l10n.ns('tpm', 'ClientTreeSharesView').value('DemandCode'),
                dataIndex: 'DemandCode'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.clienttreesharesview.ClientTreeSharesView',
        items: [{
            xtype: 'textfield',
            name: 'ResultNameStr',
            fieldLabel: l10n.ns('tpm', 'ClientTreeSharesView').value('ResultNameStr'),
            renderer: function (value) {
                return renderWithDelimiter(value, ' > ', '  ');
            }
        }, {
            xtype: 'textfield',
            name: 'BOI',
            fieldLabel: l10n.ns('tpm', 'ClientTreeSharesView').value('BOI')
        }, {
            text: l10n.ns('tpm', 'ClientTreeSharesView').value('DemandCode'),
            dataIndex: 'DemandCode'
        }]
    }]
});
