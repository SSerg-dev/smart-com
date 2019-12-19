Ext.define('App.view.core.toolbar.StandardDirectoryToolbar', {
    extend: 'Ext.toolbar.Toolbar',
    alias: 'widget.standarddirectorytoolbar',
    ui: 'light-gray-toolbar',
    cls: 'directorygrid-toolbar',

    width: 30,
    minWidth: 30,
    maxWidth: 250,

    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start',
        overflowHandler: 'Scroller'
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
        glyph: 0xf1fd,
        itemId: 'detail',
        text: l10n.ns('core', 'selectablePanelButtons').value('detail'),
        tooltip: l10n.ns('core', 'selectablePanelButtons').value('detail'),
        disabled: true
    }, '-', {
        itemId: 'extfilterbutton',
        glyph: 0xf349,
        text: l10n.ns('core', 'toptoolbar').value('filterButtonText'),
        tooltip: l10n.ns('core', 'toptoolbar').value('filterButtonText')
    }, {
        itemId: 'deletedbutton',
        resource: 'Deleted{0}',
        action: 'Get{0}',
        glyph: 0xf258,
        text: l10n.ns('core', 'toptoolbar').value('deletedButtonText'),
        tooltip: l10n.ns('core', 'toptoolbar').value('deletedButtonText')
    }, '-', {
        itemId: 'createbutton',
        action: 'Post',
        glyph: 0xf415,
        text: l10n.ns('core', 'crud').value('createButtonText'),
        tooltip: l10n.ns('core', 'crud').value('createButtonText')
    }, {
        itemId: 'updatebutton',
        action: 'Patch',
        glyph: 0xf64f,
        text: l10n.ns('core', 'crud').value('updateButtonText'),
        tooltip: l10n.ns('core', 'crud').value('updateButtonText')
    }, {
        itemId: 'deletebutton',
        action: 'Delete',
        glyph: 0xf5e8,
        text: l10n.ns('core', 'crud').value('deleteButtonText'),
        tooltip: l10n.ns('core', 'crud').value('deleteButtonText')
    }, {
        itemId: 'historybutton',
        resource: 'Historical{0}',
        action: 'Get{0}',
        glyph: 0xf2da,
        text: l10n.ns('core', 'crud').value('historyButtonText'),
        tooltip: l10n.ns('core', 'crud').value('historyButtonText')
    }, '-', '->', '-', {
        itemId: 'extfilterclearbutton',
        ui: 'blue-button-toolbar',
        disabled: true,
        glyph: 0xf232,
        text: l10n.ns('core', 'filter').value('filterEmptyStatus'),
        tooltip: l10n.ns('core', 'filter').value('filterEmptyStatus'),
        overCls: '',
        style: {
            'cursor': 'default'
        }
    }]

});