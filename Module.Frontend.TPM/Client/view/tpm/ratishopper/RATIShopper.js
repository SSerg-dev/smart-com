﻿Ext.define('App.view.tpm.ratishopper.RATIShopper', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.ratishopper',
    title: l10n.ns('tpm', 'compositePanelTitles').value('RATIShopper'),

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

        items: [{
            xtype: 'widthexpandbutton',
            ui: 'fill-gray-button-toolbar',
            text: l10n.ns('core', 'selectablePanelButtons').value('toolbarCollapse'),
            glyph: 0xf13d,
            glyph1: 0xf13e,
            target: function () {
                return this.up('toolbar');
            },
        }, {
            itemId: 'createbutton',
            action: 'Post',
            glyph: 0xf415,
            text: l10n.ns('core', 'crud').value('createButtonText'),
            tooltip: l10n.ns('core', 'crud').value('createButtonText')
        }, {
            itemId: 'updatebutton',
            action: 'Patch',
            glyph: 0xf64f,
            text: l10n.ns('core', 'crud').value('updateButtonText'),
            tooltip: l10n.ns('core', 'crud').value('updateButtonText')
        }, {
            itemId: 'deletebutton',
            action: 'Delete',
            glyph: 0xf5e8,
            text: l10n.ns('core', 'crud').value('deleteButtonText'),
            tooltip: l10n.ns('core', 'crud').value('deleteButtonText')
        }, {
            itemId: 'historybutton',
            resource: 'Historical{0}',
            action: 'Get{0}',
            glyph: 0xf2da,
            text: l10n.ns('core', 'crud').value('historyButtonText'),
            tooltip: l10n.ns('core', 'crud').value('historyButtonText')
        }, '-', {
            itemId: 'extfilterbutton',
            glyph: 0xf349,
            text: l10n.ns('core', 'toptoolbar').value('filterButtonText'),
            tooltip: l10n.ns('core', 'toptoolbar').value('filterButtonText')
        }, {
            itemId: 'deletedbutton',
            resource: 'Deleted{0}',
            action: 'Get{0}',
            glyph: 0xf258,
            text: l10n.ns('core', 'toptoolbar').value('deletedButtonText'),
            tooltip: l10n.ns('core', 'toptoolbar').value('deletedButtonText')
        }, '-', '->', '-', {
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
        }]
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',
        editorModel: 'Core.form.EditorDetailWindowModel',
        store: {
            type: 'directorystore',
            model: 'App.model.tpm.ratishopper.RATIShopper',
            storeId: 'ratishopperstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.tpm.ratishopper.RATIShopper',
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
                text: l10n.ns('tpm', 'RATIShopper').value('Year'),
                dataIndex: 'Year'
            }, {
                text: l10n.ns('tpm', 'RATIShopper').value('ClientTreeFullPathName'),
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
                text: l10n.ns('tpm', 'RATIShopper').value('ClientTreeObjectId'),
                dataIndex: 'ClientTreeObjectId'
            }, {
                text: l10n.ns('tpm', 'RATIShopper').value('RATIShopperPercent'),
                dataIndex: 'RATIShopperPercent'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.tpm.ratishopper.RATIShopper',
        items: [{
            xtype: 'numberfield',
            name: 'Year',
            fieldLabel: l10n.ns('tpm', 'RATIShopper').value('Year'),
            minValue: 0,
            maxValue: 100,
            readOnly: true,
            allowBlank: false,
            listeners: {
                change: function (newValue, oldValue) {
                }
            }   
        }, {
            xtype: 'treesearchfield',
            name: 'ClientTreeId',
            fieldLabel: l10n.ns('tpm', 'RATIShopper').value('ClientTreeFullPathName'),
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
            xtype: 'numberfield',
            name: 'RATIShopperPercent',
            fieldLabel: l10n.ns('tpm', 'RATIShopper').value('RATIShopperPercent'),
            minValue: 0,
            maxValue: 100,
            readOnly: true,
            allowBlank: false,
            listeners: {
                change: function (newValue, oldValue) {
                    if (newValue > 100) {
                        this.setValue(oldValue);
                    }
                }
            }
        }]

    }]
});