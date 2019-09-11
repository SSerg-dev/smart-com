Ext.define('App.view.core.associateduser.userrole.AssociatedUserRole', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.associateduseruserrole',
    title: l10n.ns('core', 'compositePanelTitles').value('AssociatedUserRoleTitle'),

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
        }, '-', {
            itemId: 'addbutton',
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
            itemId: 'setdefaultbutton',
            action: 'SetDefault',
            glyph: 0xf4ce,
            text: l10n.ns('core', 'AssociatedUserRole', 'buttons').value('setdefault'),
            tooltip: l10n.ns('core', 'AssociatedUserRole', 'buttons').value('setdefault'),
            disabled: true
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

        store: {
            type: 'associateddirectorystore',
            model: 'App.model.core.associateduser.userrole.AssociatedUserRole',
            storeId: 'associateduseruserrolestore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
				supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.associateduser.userrole.AssociatedUserRole',
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
                text: l10n.ns('core', 'AssociatedUserRole').value('SystemName'),
                dataIndex: 'SystemName'
            }, {
                text: l10n.ns('core', 'AssociatedUserRole').value('DisplayName'),
                dataIndex: 'DisplayName'
            }, {
                xtype: 'booleancolumn',
                text: l10n.ns('core', 'AssociatedUserRole').value('IsAllow'),
                dataIndex: 'IsAllow',
                trueText: l10n.ns('core', 'booleanValues').value('true'),
                falseText: l10n.ns('core', 'booleanValues').value('false'),
            }, {
                text: l10n.ns('core', 'AssociatedUserRole').value('IsDefault'),
                dataIndex: 'IsDefault',
                xtype: 'booleancolumn',
                trueText: l10n.ns('core', 'booleanValues').value('true'),
                falseText: l10n.ns('core', 'booleanValues').value('false')
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.core.associateduser.userrole.AssociatedUserRole',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'SystemName',
            fieldLabel: l10n.ns('core', 'AssociatedUserRole').value('SystemName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DisplayName',
            fieldLabel: l10n.ns('core', 'AssociatedUserRole').value('DisplayName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'IsAllow',
            renderer: App.RenderHelper.getBooleanRenderer(),
            fieldLabel: l10n.ns('core', 'AssociatedUserRole').value('IsAllow')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'IsDefault',
            renderer: App.RenderHelper.getBooleanRenderer(),
            fieldLabel: l10n.ns('core', 'AssociatedUserRole').value('IsDefault')
        }]
    }]

});