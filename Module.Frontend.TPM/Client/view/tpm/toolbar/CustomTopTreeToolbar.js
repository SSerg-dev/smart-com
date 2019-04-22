Ext.define('App.view.tpm.toolbar.CustomTopTreeToolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.customtoptreetoolbar',
    height: 36,
    cls: 'hierarchy-header',

    layout: 'hbox',

    defaults: {
        ui: 'gray-button-toolbar',
        padding: 0,
        textAlign: 'left'
    }
});