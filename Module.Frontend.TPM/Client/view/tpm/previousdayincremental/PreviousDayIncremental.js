Ext.define('App.view.tpm.previousdayincremental.PreviousDayIncremental', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.previousdayincremental',
    title: l10n.ns('tpm', 'compositePanelTitles').value('PreviousDayIncremental'),
    dockedItems: [{
        xtype: 'readonlydirectorytoolbar',
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
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.previousdayincremental.PreviousDayIncremental',
            storeId: 'previousdayincrementalstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.previousdayincremental.PreviousDayIncremental',
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
                text: l10n.ns('tpm', 'PreviousDayIncremental').value('Week'),
                dataIndex: 'Week'
            },{ 
                text: l10n.ns('tpm', 'PreviousDayIncremental').value('DemandCode'),
                dataIndex: 'DemandCode'
            },{
                xtype: 'numbercolumn',
                format: '0.00',
                text: l10n.ns('tpm', 'PreviousDayIncremental').value('IncrementalQty'),
                dataIndex: 'IncrementalQty'
            }, {
                text: l10n.ns('tpm', 'PreviousDayIncremental').value('LastChangeDate'),
                dataIndex: 'LastChangeDate',
                renderer: Ext.util.Format.dateRenderer('d.m.Y')
            }, {
                text: l10n.ns('tpm', 'PreviousDayIncremental').value('ZREP'),
                dataIndex: 'ZREP',
                filter: {
                    type: 'search',
                    selectorWidget: 'product',
                    valueField: 'ZREP',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.tpm.product.Product',

                    }
                }
            }, {
                text: l10n.ns('tpm', 'PreviousDayIncremental').value('Number'),
                dataIndex: 'Number'
            },]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.view.tpm.previousdayincremental.PreviousDayIncremental',
        items: [ ]
    }]
});
