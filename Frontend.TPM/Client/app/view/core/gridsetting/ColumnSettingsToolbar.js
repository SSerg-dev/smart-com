Ext.define('App.view.core.gridsetting.ColumnSettingsToolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.colsettingstoolbar',
    ui: 'transparent-toolbar',

    layout: {
        type: 'vbox',
        pack: 'center',
        align: 'center'
    },

    defaults: {
        ui: 'white-button-footer',
        margin: '0 10 10 10',
        padding: 7,
        disabled: true
    }
});