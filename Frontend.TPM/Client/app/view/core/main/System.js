Ext.define('App.view.core.main.System', {
    extend: 'Ext.tab.Panel',
    alias: 'widget.system',
    ui: 'system-panel',
    deferredRender: false,
    activeTab: -1,

    // Workaround в связи с багом в loadmask
    style: {
        zIndex: 2
    },

    tabBar: {
        height: 40,

        defaults: {
            border: '1 1 0 0',
            width: 130,
            height: 40
        }

    },

    defaults: {
        layout: 'fit',

        dockedItems: {
            xtype: 'toolbar',
            dock: 'right',
            ui: 'transparent-toolbar',

            items: {
                ui: 'white-button-toolbar',
                glyph: 0xf156,
                padding: '3 4 0 0',
                handler: function () {
                    var sysPanel = this.up('system');
                    sysPanel.tabBar.activeTab.deactivate();
                    sysPanel.tabBar.activeTab = null;
                    sysPanel.activeTab = null;
                    sysPanel.collapse();
                }
            }
        }

    },

    items: [{
        title: l10n.ns('core', 'System').value('Tasks'),
        itemId: 'systemUserTasksTab',

        items: {
            xtype: 'userloophandler',
            selectedUI: 'blue-selectable-panel'
        },

        tabConfig: {
            ui: 'system-panel-tab-button',
            border: '1 1 0 1'
        }
    }],

    initComponent: function () {
        this.callParent(arguments);
        //Set the docked item to which panel will collapse.
        this.header = this.tabBar;
    },

    setActiveTab: function (tab) {
        this.callParent(arguments);
        this.expand();
        if (Ext.isFunction(tab.down)) {
            var grid = tab.down('directorygrid');
            if (grid) {
                grid.getStore().load();
            }
        }
    }

});