Ext.define('App.view.core.loophandler.LoopHandler', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.loophandler',
    title: l10n.ns('core', 'compositePanelTitles').value('LoopHandlerTitle'),

    dockedItems: [{
        xtype: 'harddeletedirectorytoolbar',
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
            itemId: 'deletebutton',
            action: 'Delete',
            glyph: 0xf5e8,
            text: l10n.ns('core', 'crud').value('deleteButtonText'),
            tooltip: l10n.ns('core', 'crud').value('deleteButtonText')
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

    customHeaderItems: [{
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
            }, {
                glyph: 0xf219,
                itemId: 'taskdetails',
                resource: 'LoopHandlers',
                action: 'Parameters',
                text: l10n.ns('core', 'additionalMenu').value('taskDetailsMenuItem')
            }, {
                glyph: 0xf459,
                itemId: 'loophandlerstart',
                resource: 'LoopHandlers',
                action: 'Start',
                text: l10n.ns('core', 'additionalMenu').value('startMenuItem')
            }]
        }
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',

        store: {
            type: 'directorystore',
            model: 'App.model.core.loophandler.LoopHandler',
            storeId: 'loophandlerstore',
            sorters: [{
                property: 'CreateDate',
                direction: 'DESC'
            }],
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.loophandler.LoopHandler',
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
                text: l10n.ns('core', 'LoopHandler').value('Description'),
                dataIndex: 'Description'
            }, {
                text: l10n.ns('core', 'LoopHandler').value('Name'),
                dataIndex: 'Name'
            }, {
                text: l10n.ns('core', 'LoopHandler').value('ExecutionPeriod'),
                dataIndex: 'ExecutionPeriod'
            }, {
                text: l10n.ns('core', 'LoopHandler').value('ExecutionMode'),
                dataIndex: 'ExecutionMode'
            }, {
                text: l10n.ns('core', 'LoopHandler').value('CreateDate'),
                dataIndex: 'CreateDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('core', 'LoopHandler').value('LastExecutionDate'),
                dataIndex: 'LastExecutionDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('core', 'LoopHandler').value('NextExecutionDate'),
                dataIndex: 'NextExecutionDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('core', 'LoopHandler').value('ConfigurationName'),
                dataIndex: 'ConfigurationName'
            }, {
                text: l10n.ns('core', 'LoopHandler').value('Status'),
                dataIndex: 'Status'
            }, {
                text: l10n.ns('core', 'LoopHandler').value('UserName'),
                dataIndex: 'UserName'//,
                //filter: {
                //	type: 'search',
                //	selectorWidget: 'user',
                //	valueField: 'Name',
                //	store: {
                //		type: 'directorystore',
                //		model: 'App.model.core.user.User',
                //		extendedFilter: {
                //			xclass: 'App.ExtFilterContext',
                //			supportedModels: [{
                //				xclass: 'App.ExtSelectionFilterModel',
                //				model: 'App.model.core.user.User',
                //				modelId: 'efselectionmodel'
                //			}, {
                //				xclass: 'App.ExtTextFilterModel',
                //				modelId: 'eftextmodel'
                //			}]
                //		}
                //	}
                //}
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.core.loophandler.LoopHandler',
        items: [{
            xtype: 'textfield',
            name: 'Description',
            fieldLabel: l10n.ns('core', 'LoopHandler').value('Description')
        }, {
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('core', 'LoopHandler').value('Name')
        }, {
            xtype: 'numberfield',
            name: 'ExecutionPeriod',
            minValue: 0,
            allowDecimals: false,
            fieldLabel: l10n.ns('core', 'LoopHandler').value('ExecutionPeriod')
        }, {
            xtype: 'textfield',
            name: 'ExecutionMode',
            fieldLabel: l10n.ns('core', 'LoopHandler').value('ExecutionMode')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CreateDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'LoopHandler').value('CreateDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'LastExecutionDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'LoopHandler').value('LastExecutionDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'NextExecutionDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'LoopHandler').value('NextExecutionDate')
        }, {
            xtype: 'textfield',
            name: 'ConfigurationName',
            fieldLabel: l10n.ns('core', 'LoopHandler').value('ConfigurationName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Status',
            fieldLabel: l10n.ns('core', 'LoopHandler').value('Status')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'UserName',
            fieldLabel: l10n.ns('core', 'LoopHandler').value('UserName')
        }]
    }]

});