Ext.define('App.view.tpm.rsmode.RSmode', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.rsmode',
    title: l10n.ns('tpm', 'compositePanelTitles').value('RSmode'),

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
        items: [
            {
                xtype: 'widthexpandbutton',
                ui: 'fill-gray-button-toolbar',
                text: l10n.ns('core', 'selectablePanelButtons').value('toolbarCollapse'),
                glyph: 0xf13d,
                glyph1: 0xf13e,
                target: function () {
                    return this.up('toolbar');
                },
            },
            {
                itemId: 'onapprovalbutton',
                action: 'OnApproval',
                disabled: true,
                glyph: 0xF5E1,
                text: 'Send for approval ',
                tooltip: 'Send for approval '
            },
            {
                itemId: 'massapprovebutton',
                action: 'MassApprove',
                glyph: 0xf0d7,
                text: l10n.ns('tpm', 'Promo').value('MassApprovalButtonText'),
                tooltip: l10n.ns('tpm', 'Promo').value('MassApprovalButtonText')
            },
            {
                itemId: 'approvebutton',
                action: 'Approve',
                disabled: true,
                glyph: 0xF5E0,
                text: 'Approve',
                tooltip: 'Approve'
            },
            {
                itemId: 'declinebutton',
                action: 'Decline',
                disabled: true,
                glyph: 0xF739,
                text: 'Decline',
                tooltip: 'Decline'
            },
            {
                itemId: 'deletedbutton',
                resource: 'Deleted{0}',
                action: 'Get{0}',
                glyph: 0xf258,
                text: 'Cancelled',
                tooltip: 'Cancelled'
            },
            {
                itemId: 'historybutton',
                resource: 'Historical{0}',
                action: 'Get{0}',
                glyph: 0xf2da,
                text: l10n.ns('core', 'crud').value('historyButtonText'),
                tooltip: l10n.ns('core', 'crud').value('historyButtonText')
            },
            '-',
            {
                itemId: 'extfilterbutton',
                glyph: 0xf349,
                text: l10n.ns('core', 'toptoolbar').value('filterButtonText'),
                tooltip: l10n.ns('core', 'toptoolbar').value('filterButtonText')
            }, '-', '->', '-',
            {
                itemId: 'extfilterclearbutton',
                ui: 'blue-button-toolbar',
                disabled: true,
                glyph: 0xf232,
                text: l10n.ns('core', 'filter').value('filterEmptyStatus'),
                tooltip: l10n.ns('core', 'filter').value('filterEmptyStatus'),
                overCls: '',
                style: {
                    'cursor': 'default'
                }
            }
        ]
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        name: 'RSmodeGrid',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.rsmode.RSmode',
            storeId: 'rsmodestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.rsmode.RSmode',
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
                    text: l10n.ns('tpm', 'RSmode').value('RSId'),
                    dataIndex: 'RSId'
                },
                {
                    text: l10n.ns('tpm', 'RSmode').value('ClientTreeFullPathName'),
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
                },
                {
                    text: l10n.ns('tpm', 'RSmode').value('ExpirationDate'),
                    dataIndex: 'ExpirationDate',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y')
                },
                {
                    text: l10n.ns('tpm', 'RSmode').value('StartDate'),
                    dataIndex: 'StartDate',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y')
                },
                {
                    text: l10n.ns('tpm', 'RSmode').value('EndDate'),
                    dataIndex: 'EndDate',
                    renderer: Ext.util.Format.dateRenderer('d.m.Y')
                },
                {
                    text: l10n.ns('tpm', 'RSmode').value('PromoStatusName'),
                    dataIndex: 'PromoStatusName',
                    width: 120,
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
                },
            ]
        }
    },
    {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.rsmode.RSmode',
        items: []
    }
    ]
});
