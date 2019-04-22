Ext.define('App.view.core.associateduser.aduser.AssociatedAdUser', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.associatedaduseruser',
    title: l10n.ns('core', 'compositePanelTitles').value('AssociatedUserTitle'),

    dockedItems: [{
        xtype: 'standarddirectorytoolbar',
        dock: 'right',

        items: [{
            xtype: 'widthexpandbutton',
            ui: 'fill-gray-button-toolbar',
            text: l10n.ns('core', 'selectablePanelButtons').value('toolbarCollapse'),
            glyph: 0xf13d,
            glyph1: 0xf13e,
            target: function () {
                return this.up('toolbar');
            }
        }, {
            glyph: 0xf2c1,
            itemId: 'table',
            text: l10n.ns('core', 'selectablePanelButtons').value('table'),
            tooltip: l10n.ns('core', 'selectablePanelButtons').value('table')
        }, {
            glyph: 0xf1fd,
            itemId: 'detail',
            text: l10n.ns('core', 'selectablePanelButtons').value('detail'),
            tooltip: l10n.ns('core', 'selectablePanelButtons').value('detail'),
            disabled: true
        }, '-', {
            itemId: 'extfilterbutton',
            glyph: 0xf349,
            text: l10n.ns('core', 'toptoolbar').value('filterButtonText'),
            tooltip: l10n.ns('core', 'toptoolbar').value('filterButtonText')
        }, {
            itemId: 'deletedbutton',
            resource: 'DeletedUsers',
            action: 'GetDeletedUsers',
            glyph: 0xf258,
            text: l10n.ns('core', 'toptoolbar').value('deletedButtonText'),
            tooltip: l10n.ns('core', 'toptoolbar').value('deletedButtonText')
        }, '-', {
            itemId: 'createbutton',
            action: 'Post',
            glyph: 0xf415,
            text: l10n.ns('core', 'crud').value('addButtonText'),
            tooltip: l10n.ns('core', 'crud').value('addButtonText')
        }, {
            itemId: 'deletebutton',
            action: 'Delete',
            glyph: 0xf5e8,
            text: l10n.ns('core', 'crud').value('deleteButtonText'),
            tooltip: l10n.ns('core', 'crud').value('deleteButtonText')
        }, {
            itemId: 'historybutton',
            resource: 'HistoricalUsers',
            action: 'GetHistoricalUsers',
            glyph: 0xf2da,
            text: l10n.ns('core', 'crud').value('historyButtonText'),
            tooltip: l10n.ns('core', 'crud').value('historyButtonText')
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
        editorModel: 'Core.form.EditorWindowModel',

        store: {
            type: 'directorystore',
            model: 'App.model.core.associateduser.user.AssociatedUser',
            storeId: 'associateduseruserstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.associateduser.user.AssociatedUser',
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
                text: l10n.ns('core', 'AssociatedUser').value('Sid'),
                dataIndex: 'Sid'
            }, {
                text: l10n.ns('core', 'AssociatedUser').value('Name'),
                dataIndex: 'Name'
            }, {
                text: l10n.ns('core', 'AdUser').value('Email'),
                dataIndex: 'Email'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.core.associateduser.user.AssociatedUser',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'Sid',
            fieldLabel: l10n.ns('core', 'AssociatedUser').value('Sid')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Name',
            fieldLabel: l10n.ns('core', 'AssociatedUser').value('Name')
        }, {
            xtype: 'textfield',
            text: l10n.ns('core', 'AdUser').value('Email'),
            name: 'Email'
        }]
    }]

});