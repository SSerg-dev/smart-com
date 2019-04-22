Ext.define('App.view.core.common.AssociatedDirectoryView', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.associateddirectoryview',
    ui: 'marengo-panel',
    cls: 'associated-marengo-panel',
    autoScroll: true,
    margin: 0,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    defaults: {
        flex: 1,
        margin: '0 8 20 20'
    },

    initComponent: function () {
        this.callParent(arguments);

        // Workaround из-за некорректной работы класса Selectable.
        var cdPanels = this.query('combineddirectorypanel');
        cdPanels.forEach(function (item) {
            if (!item.isMain()) {
                var grid = item.down('directorygrid'),
                    filterbar = grid && grid.getPlugin('filterbar');

                if (filterbar) {
                    filterbar.needFocus = false;
                }
            }
        });
    }

});