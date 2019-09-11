Ext.define('App.util.core.System', {
    alternateClassName: 'App.System',

    statics: {
        openUserTasksPanel: function () {
            var systemPanel = Ext.ComponentQuery.query('system')[0],
                tasksTab = Ext.ComponentQuery.query('#systemUserTasksTab')[0];

            if (systemPanel && tasksTab) {
                systemPanel.setActiveTab(tasksTab);
            }
        }
    }
});