Ext.define('App.view.tpm.metricslivehistory.MetricsLiveHistory', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.metricslivehistory',
    title: l10n.ns('tpm', 'compositePanelTitles').value('MetricsLiveHistory'),

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
    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.metricslivehistory.MetricsLiveHistory',
            storeId: 'metricslivehistoriesstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.metricslivehistory.MetricsLiveHistory',
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
            items: [
                {
                    xtype: 'booleancolumn',
                    text: l10n.ns('tpm', 'MetricsLiveHistory').value('Type'),
                    dataIndex: 'Type',
                    width: 130,
                    renderer: function (value) {
                        return value;
                    },
                    trueText: 'PCT',
                    falseText: 'PPA',
                    filter: {
                        type: 'bool',
                        store: [
                            [0, 'PPA'],
                            [1, 'PCT']
                        ]
                    }
                },
                {
                    xtype: 'datecolumn',
                    text: l10n.ns('tpm', 'MetricsLiveHistory').value('Date'),
                    dataIndex: 'Date',
                    width: 130,
                    renderer: Ext.util.Format.dateRenderer('d.m.Y H:i'),
                },
                {
                    text: l10n.ns('tpm', 'MetricsLiveHistory').value('ClientHierarchy'),
                    dataIndex: 'ClientTreeId',
                    width: 250,
                    filter: {
                        xtype: 'treefsearchfield',
                        trigger2Cls: '',
                        selectorWidget: 'clienttree',
                        valueField: 'ObjectId',
                        displayField: 'ObjectId',
                        multiSelect: true,
                        operator: 'eq',
                        store: {
                            model: 'App.model.tpm.clienttree.ClientTree',
                            autoLoad: false,
                            root: {}
                        },
                    },
                    //renderer: function (value) {
                    //    debugger;
                    //    return value;
                    //    //return renderWithDelimiter(value, ' > ', '  ');
                    //}
                },
                {
                    xtype: 'numbercolumn',
                    text: l10n.ns('tpm', 'MetricsLiveHistory').value('Value'),
                    dataIndex: 'Value',
                    width: 110,
                },
                {
                    xtype: 'numbercolumn',
                    text: l10n.ns('tpm', 'MetricsLiveHistory').value('ValueLSV'),
                    dataIndex: 'ValueLSV',
                    width: 110,
                },
            ]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.metricslivehistory.MetricsLiveHistory',
        items: [{
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('tpm', 'Budget').value('Name')
        }]
    }]
});
