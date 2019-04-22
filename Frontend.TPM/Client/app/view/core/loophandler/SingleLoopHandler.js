Ext.define('App.view.core.loophandler.SingleLoopHandler', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.singleloophandler',
    title: l10n.ns('core', 'compositePanelTitles').value('SingleLoopHandlerTitle'),

    dockedItems: [{
        xtype: 'readonlydirectorytoolbar',
        dock: 'right'
    }],

    customHeaderItems: [
        ResourceMgr.getAdditionalMenu('core').loophandler
    ],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',

        store: {
            type: 'directorystore',
            model: 'App.model.core.loophandler.SingleLoopHandler',
            storeId: 'singleloophandlerstore',
            sorters: [{
                property: 'CreateDate',
                direction: 'DESC'
            }],
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.loophandler.SingleLoopHandler',
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
                text: l10n.ns('core', 'SingleLoopHandler').value('CreateDate'),
                dataIndex: 'CreateDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('core', 'SingleLoopHandler').value('Description'),
                dataIndex: 'Description'
            }, {
                text: l10n.ns('core', 'SingleLoopHandler').value('Status'),
                dataIndex: 'Status'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.core.loophandler.SingleLoopHandler',
        items: [{
            xtype: 'textfield',
            name: 'Description',
            fieldLabel: l10n.ns('core', 'SingleLoopHandler').value('Description')
        }, {
            xtype: 'textfield',
            name: 'Name',
            fieldLabel: l10n.ns('core', 'SingleLoopHandler').value('Name')
        }, {
            xtype: 'numberfield',
            name: 'ExecutionPeriod',
            minValue: 0,
            allowDecimals: false,
            fieldLabel: l10n.ns('core', 'SingleLoopHandler').value('ExecutionPeriod')
        }, {
            xtype: 'textfield',
            name: 'ExecutionMode',
            fieldLabel: l10n.ns('core', 'SingleLoopHandler').value('ExecutionMode')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'CreateDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'SingleLoopHandler').value('CreateDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'LastExecutionDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'SingleLoopHandler').value('LastExecutionDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'NextExecutionDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'SingleLoopHandler').value('NextExecutionDate')
        }, {
            xtype: 'textfield',
            name: 'ConfigurationName',
            fieldLabel: l10n.ns('core', 'SingleLoopHandler').value('ConfigurationName')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Status',
            fieldLabel: l10n.ns('core', 'SingleLoopHandler').value('Status')
        }]
    }]

});