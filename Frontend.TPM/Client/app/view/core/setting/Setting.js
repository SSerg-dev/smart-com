Ext.define('App.view.core.setting.Setting', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.setting',
    title: l10n.ns('core', 'compositePanelTitles').value('SettingTitle'),

    dockedItems: [{
        xtype: 'readonlydirectorytoolbar',
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
            itemId: 'updatebutton',
            action: 'Patch',
            glyph: 0xf64f,
            text: l10n.ns('core', 'crud').value('updateButtonText'),
            tooltip: l10n.ns('core', 'crud').value('updateButtonText')
        }, {
            itemId: 'historybutton',
            resource: 'Historical{0}',
            action: 'Get{0}',
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

        store: {
            type: 'directorystore',
            model: 'App.model.core.setting.Setting',
            storeId: 'settingstore',
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
				supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.setting.Setting',
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
				text: l10n.ns('core', 'Setting').value('Name'),
				dataIndex: 'Name'
			}, { 
				text: l10n.ns('core', 'Setting').value('Type'),
				dataIndex: 'Type'
			}, { 
				text: l10n.ns('core', 'Setting').value('Value'),
				dataIndex: 'Value',
				renderer: function (value, metaData, record) {
				    if (record.get('Type') == 'System.DateTime' && value) {
				        return Ext.Date.format(new Date(value), 'd.m.Y');
				    }
				    return value;
				}
			}, { 
				text: l10n.ns('core', 'Setting').value('Description'),
				dataIndex: 'Description'
			}]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
		model: 'App.model.core.setting.Setting',
        items: [{ 
			xtype: 'singlelinedisplayfield',
			name: 'Name',
			fieldLabel: l10n.ns('core', 'Setting').value('Name')		
		}, { 
			xtype: 'singlelinedisplayfield',
			name: 'Type',
			fieldLabel: l10n.ns('core', 'Setting').value('Type')		
		}, { 
			xtype: 'textfield',
			name: 'Value',
			fieldLabel: l10n.ns('core', 'Setting').value('Value')		
		}, { 
			xtype: 'textfield',
			name: 'Description',
			fieldLabel: l10n.ns('core', 'Setting').value('Description'),
			allowBlank: true,
			allowOnlyWhitespace: true
		}]
    }]

});