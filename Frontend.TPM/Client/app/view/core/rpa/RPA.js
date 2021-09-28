Ext.define('App.view.core.rpa.RPA', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.rpa',
    title: l10n.ns('core', 'compositePanelTitles').value('RPATitle'),

    dockedItems: [{
        xtype: 'rpadirectorytoolbar',
        dock: 'right',
        items:[{
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
            itemId: 'updategroupbutton',
            glyph: 0xf4f0,
            action: 'Patch',
            text: l10n.ns('tpm', 'button').value('updateGroupButtonText'),
            tooltip: l10n.ns('tpm', 'button').value('updateGroupButtonText')
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
            model: 'App.model.core.rpa.RPA',
            storeId: 'rpastore',
            sorters: [{
                property: 'CreateDate',
                direction: 'DESC'
            }],
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.rpa.RPA',
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
                text: l10n.ns('core', 'RPA').value('HandlerName'),
                dataIndex: 'HandlerName'
            }, {
                text: l10n.ns('core', 'RPA').value('CreateDate'),
                dataIndex: 'CreateDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('core', 'RPA').value('UserName'),
                dataIndex: 'UserName'
            }, {
                text: l10n.ns('core', 'RPA').value('Constraint'),
                dataIndex: 'Constraint'
            }, {
                text: l10n.ns('core', 'RPA').value('Parametrs'),
                dataIndex: 'Parametrs'
            }, {
                text: l10n.ns('core', 'RPA').value('Status'),
                dataIndex: 'Status'
            }, {
                text: l10n.ns('core', 'RPA').value('FileURL'),
                dataIndex: 'FileURL'
            }, {
                text: l10n.ns('core', 'RPA').value('LogURL'),
                dataIndex: 'LogURL'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.core.rpa.RPA',
        items: [{
            xtype: 'textfield',
            name: 'HandlerName',
            fieldLabel: l10n.ns('core', 'RPA').value('HandlerName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CreateDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'RPA').value('CreateDate')
        }, {
            xtype: 'textfield',
            name: 'UserName',
            fieldLabel: l10n.ns('core', 'RPA').value('UserName')
        }, {
            xtype: 'textfield',
            name: 'Constraint',
            fieldLabel: l10n.ns('core', 'RPA').value('Constraint')
        }, {
            xtype: 'textfield',
            name: 'Parametrs',
            fieldLabel: l10n.ns('core', 'RPA').value('Parametrs')
        }, {
            xtype: 'textfield',
            name: 'Status',
            fieldLabel: l10n.ns('core', 'RPA').value('Status')
        }, {
            xtype: 'textfield',
            name: 'FileURL',
            fieldLabel: l10n.ns('core', 'RPA').value('FileURL')
        }, {
            xtype: 'textfield',
            name: 'LogURL',
            fieldLabel: l10n.ns('core', 'RPA').value('LogURL')
        }]
    }]

});