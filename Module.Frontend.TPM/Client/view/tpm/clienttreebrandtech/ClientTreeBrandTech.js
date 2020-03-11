Ext.define('App.view.tpm.clienttreebrandtech.ClientTreeBrandTech', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.clienttreebrandtech',
    title: l10n.ns('tpm', 'compositePanelTitles').value('ClientTreeBrandTech'),

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
            model: 'App.model.tpm.clienttreebrandtech.ClientTreeBrandTech',
            storeId: 'clienttreebrandtechstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.clienttreebrandtech.ClientTreeBrandTech',
                    modelId: 'efselectionmodel'
                }, {
                    xclass: 'App.ExtTextFilterModel',
                    modelId: 'eftextmodel'
                }]
            },
            sorters: [{
                property: 'ClientTreeObjectId',
                direction: 'ASC'
            }]
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
                text: l10n.ns('tpm', 'ClientTreeBrandTech').value('ParentClientTreeDemandCode'),
                dataIndex: 'ParentClientTreeDemandCode',
                filter: {
                    xtype: 'treefsearchfield',
                    trigger2Cls: '',
                    selectorWidget: 'clienttree',
                    valueField: 'DemandCode',
                    displayField: 'DemandCode',
                    multiSelect: true,
                    operator: 'conts',
                    store: {
                        model: 'App.model.tpm.clienttree.ClientTree',
                        autoLoad: false,
                        root: {}
                    },                         
                }
            }, {
                text: l10n.ns('tpm', 'ClientTreeBrandTech').value('ClientTreeObjectId'),
                dataIndex: 'ClientTreeObjectId',
                minWidth: 200
            }, {
                text: l10n.ns('tpm', 'ClientTreeBrandTech').value('ClientTreeName'),
                dataIndex: 'ClientTreeName',
                minWidth: 200,
                filter: {
                    xtype: 'treefsearchfield',
                    trigger2Cls: '',
                    selectorWidget: 'clienttree',
                    valueField: 'Name',
                    displayField: 'Name',
                    multiSelect: true,
                    operator: 'conts',
                    store: {
                        model: 'App.model.tpm.clienttree.ClientTree',
                        autoLoad: false,
                        root: {}
                    },                         
                }
            }, {
                text: l10n.ns('tpm', 'ClientTreeBrandTech').value('CurrentBrandTechName'),
                dataIndex: 'CurrentBrandTechName',
                filter: {
                    xtype: 'fsearchfield',
                    trigger2Cls: '',
                    selectorWidget: 'brandtech',
                    valueField: 'Name',
                    displayField: 'Name',
                    multiSelect: true,
                    operator: 'conts',
                    store: {
                        model: 'App.model.tpm.brandtech.BrandTech',
                    },                         
                }
            }, {
                text: l10n.ns('tpm', 'ClientTreeBrandTech').value('Share'),
                dataIndex: 'Share',
                renderer: function (value, meta) {
                    return value.toString().replace('.', ',')
                },
                filter: {
                    xtype: 'numberfield',
                    submitLocaleSeparator: false,
                    decimalPrecision: 15,
                    allowDecimals: true,
                    decimalSeparator: ',',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    operator: 'eq'
                }
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.clienttreebrandtech.ClientTreeBrandTech',
        items: [{
            name: 'ParentClientTreeDemandCode',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('ParentClientTreeDemandCode'),
        }, {
            name: 'ClientTreeObjectId',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('ClientTreeObjectId')
        }, {
            name: 'ClientTreeName',
            fieldLabel: l10n.ns('tpm', 'ClientTreeBrandTech').value('ClientTreeName')
        }, {
            text: l10n.ns('tpm', 'ClientTreeBrandTech').value('CurrentBrandTechName'),
            dataIndex: 'CurrentBrandTechName'
        }, {
            text: l10n.ns('tpm', 'ClientTreeBrandTech').value('Share'),
            dataIndex: 'Share'
        }]
    }]
});
