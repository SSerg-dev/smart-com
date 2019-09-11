Ext.define('App.view.core.filter.ExtendedFilterSettingsWindow', {
    extend: 'App.view.core.base.BaseModalWindow',
    alias: 'widget.extfiltersettings',
    title: l10n.ns('core', 'filter').value('filterSettingsTitle'),
    width: 350,
    minWidth: 350,
    minHeight: 450,
    maxHeight: 600,

    items: [{
        xtype: 'panel',
        frame: true,
        ui: 'light-gray-panel',
        cls: 'extfiltersettings-panel',
        layout: 'fit',
        bodyPadding: '10 0 10 0',

        items: [{
            xtype: 'panel',
            ui: 'light-gray-panel',
            autoScroll: true,

            layout: {
                type: 'vbox',
                align: 'stretch'
            },

            items: [{
                xtype: 'grid',
                itemId: 'filtersettingsgrid',
                frame: true,
                ui: 'grid-panel',
                cls: 'filtersettingsgrid',
                selType: 'checkboxmodel',
                enableColumnHide: false,
                enableColumnMove: false,
                scroll: false,
                margin: '0 6 0 10',

                viewConfig: {
                    trackOver: false
                },

                store: {
                    fields: ['id', 'name']
                },

                columns: [{
                    text: l10n.ns('core', 'filter').value('settingsHeader'),
                    dataIndex: 'name',
                    flex: 1,
                    plugins: ['sortbutton'],
                    menuDisabled: true,
                    resizable: false
                }]
            }]
        }]
    }]
});