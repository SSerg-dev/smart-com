Ext.define('App.view.core.loophandler.AdminLoopHandler', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.adminloophandler',
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
            }, {
                text: l10n.ns('core', 'SingleLoopHandler').value('UserName'),
                dataIndex: 'UserName',
                filter: {
                    type: 'search',
                    selectorWidget: 'user',
                    valueField: 'Name',
                    store: {
                        type: 'directorystore',
                        model: 'App.model.core.user.User',
                        extendedFilter: {
                            xclass: 'App.ExtFilterContext',
                            supportedModels: [{
                                xclass: 'App.ExtSelectionFilterModel',
                                model: 'App.model.core.user.User',
                                modelId: 'efselectionmodel'
                            }, {
                                xclass: 'App.ExtTextFilterModel',
                                modelId: 'eftextmodel'
                            }]
                        }
                    }
                }
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.core.loophandler.SingleLoopHandler',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'CreateDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('core', 'SingleLoopHandler').value('CreateDate')
        }, {
            xtype: 'textfield',
            name: 'Description',
            fieldLabel: l10n.ns('core', 'SingleLoopHandler').value('Description')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Status',
            fieldLabel: l10n.ns('core', 'SingleLoopHandler').value('Status')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'UserName',
            fieldLabel: l10n.ns('core', 'SingleLoopHandler').value('UserName')
        }]
    }]

});