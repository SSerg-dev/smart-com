Ext.define('App.view.tpm.toolbar.calculatinglog.CustomLogTopToolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.customlogtoptoolbar',
    cls: 'custom-top-panel',

    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    defaults: {
        ui: 'gray-button-toolbar',
        padding: '0 0 0 0',
        margin: { right: 10 },
        textAlign: 'left'
    },

    items: [{
        xtype: 'label',
        text: 'Log', 
        cls: 'logtext'
    }, {
            xtype: 'button',
            name: 'logError',
            cls: 'errorbutton messageclicked',
            text: '0 Errors',
            filterText: 'ERROR',
            glyph: 0xf159,
            enableToggle: true,
            pressed: true
        }, {
            xtype: 'button',
            name: 'logWarning',
            cls: 'warningbutton messageclicked',
            text: '0 Warnings',
            filterText: 'WARNING',
            glyph: 0xf026,
            enableToggle: true,
            pressed: true
        }, {
            xtype: 'button',
            name: 'logMessage',
            cls: 'messagebutton messageclicked',
            text: '0 Infos',
            filterText: 'INFO',
            glyph: 0xf028,
            enableToggle: true,
            pressed: true
        },
        '->',
    {
        xtype: 'expandbutton',
        glyph: 0xf063,
        glyph1: 0xf04b,
        itemId: 'collapse',        
        ui: 'white-button-toolbar',
        tooltip: l10n.ns('core', 'selectablePanelButtons').value('collapse'),
        target: function () {
            return this.up('combineddirectorypanel');
        }
    }]
})