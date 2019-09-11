Ext.define('App.view.core.toolbar.ReportsFilterToolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.reportsfiltertoolbar',
    ui: 'light-gray-toolbar',
    cls: 'directorygrid-toolbar',

    width: 30,
    minWidth: 30,
    maxWidth: 250,

    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start'
    },

    defaults: {
        ui: 'gray-button-toolbar',
        padding: 6, //TODO: временно
        textAlign: 'left'
    },

    items: [{
        xtype: 'widthexpandbutton',
        ui: 'fill-gray-button-toolbar',
        text: l10n.ns('core', 'selectablePanelButtons').value('toolbarCollapse'),
        glyph: 0xf13d,
        glyph1: 0xf13e,
        target: function () {
            return this.up('toolbar');
        }
    }, {
        glyph: 0xf2c1,
        itemId: 'table',
        text: l10n.ns('core', 'selectablePanelButtons').value('table'),
        tooltip: l10n.ns('core', 'selectablePanelButtons').value('table')
    }, {
        glyph: 0xf1da,
        itemId: 'download',
        text: l10n.ns('core', 'selectablePanelButtons').value('download'),
        tooltip: l10n.ns('core', 'selectablePanelButtons').value('download'),
        //disabled: true
    }, '-', {
        itemId: 'extfilterbutton',
        glyph: 0xf349,
        text: l10n.ns('core', 'toptoolbar').value('filterButtonText'),
        tooltip: l10n.ns('core', 'toptoolbar').value('filterButtonText')
    },'-', '->', '-',{
        itemId: 'applyReportFilter',
       // ui: 'gray-button-toolbar-toolbar',
        disabled: false,
        glyph: 0xf21b,
        text: l10n.ns('core', 'filter').value('reportsOnly'),
        tooltip: l10n.ns('core', 'filter').value('reportsOnly'),
        overCls: '',
        style: {
            'cursor': 'default'
        }
    },{
        itemId: 'applyTaskFilter',
      //  ui: 'gray-button-toolbar-toolbar',
        disabled: false,
        glyph: 0xf219,
        text: l10n.ns('core', 'filter').value('taskOnly'),
        tooltip: l10n.ns('core', 'filter').value('taskOnly'),
        overCls: '',
        style: {
            'cursor': 'default'
        }
    },{
        itemId: 'clearTaskFilter',
        ui: 'blue-pressed-button-toolbar',
        glyph: 0xf234,
        text: l10n.ns('core', 'filter').value('all'),
        tooltip: l10n.ns('core', 'filter').value('all'),
        overCls: '',
        style: {
            'cursor': 'default'
        }
    }]

});