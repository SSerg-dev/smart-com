Ext.define('App.view.core.loophandler.UserLoopHandler', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.userloophandler',
    title: l10n.ns('core', 'compositePanelTitles').value('UserLoopHandlerTitle'),
    margin: '20 7 20 20',
    minHeight: null,
    isMain: false,

    suppressSelection: true,

    dockedItems: [{
        xtype: 'reportsfiltertoolbar',
        dock: 'right'
    }],

    customHeaderItems: [
        ResourceMgr.getAdditionalMenu('core').loophandler
    ],

    systemHeaderItems: [{
        glyph: 0xf4e6,
        itemId: 'refresh',
        tooltip: l10n.ns('core', 'selectablePanelButtons').value('update')
    }],

    items: [{
        xtype: 'directorygrid',
        itemId: 'datatable',

        store: {
            type: 'directorystore',
            model: 'App.model.core.loophandler.UserLoopHandler',
            storeId: 'userloophandlerstore',
            autoLoad: true,
            sorters: [{
                property: 'CreateDate',
                direction: 'DESC'
            }],
            extendedFilter: {
                xclass: 'App.ExtFilterContext',
                supportedModels: [{
                    xclass: 'App.ExtSelectionFilterModel',
                    model: 'App.model.core.loophandler.UserLoopHandler',
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
                text: l10n.ns('core', 'UserLoopHandler').value('CreateDate'),
                dataIndex: 'CreateDate',
                xtype: 'datecolumn',
                renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
            }, {
                text: l10n.ns('core', 'UserLoopHandler').value('Description'),
                dataIndex: 'Description'
            }, {
                text: l10n.ns('core', 'UserLoopHandler').value('Status'),
                dataIndex: 'Status'
            }]
        }
    }, {
        xtype: 'editabledetailform',
        itemId: 'detailform',
        model: 'App.model.core.loophandler.UserLoopHandler',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: 'CreateDate',
            fieldLabel: l10n.ns('core', 'UserLoopHandler').value('CreateDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Description',
            fieldLabel: l10n.ns('core', 'UserLoopHandler').value('Description')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Status',
            fieldLabel: l10n.ns('core', 'UserLoopHandler').value('Status')
        }]
    }],

    initComponent: function () {
        this.callParent(arguments);

        // Workaround в связи с багом в loadmask
        this.down('directorygrid').getView().loadMask = {
            maskOnDisable: false,
            fixedZIndex: 3
        };
    }
});
