Ext.define('App.view.tpm.filter.ExtendedDetailFilterWindow', {
    extend: 'App.view.core.filter.ExtendedFilterWindow',
    alias: 'widget.extdetailfilter',

    buttons: [{
        text: l10n.ns('core', 'filter', 'buttons').value('close'),
        itemId: 'cancel'
    }, {
        text: l10n.ns('core', 'filter', 'buttons').value('reject'),
        itemId: 'reject'
    }, {
        text: l10n.ns('core', 'filter', 'buttons').value('apply'),
        ui: 'green-button-footer-toolbar',
        itemId: 'detailapply'
    }],

    items: [{
        xtype: 'panel',
        itemId: 'modelcontainer',
        frame: true,
        ui: 'light-gray-panel',
        layout: 'fit',
        bodyPadding: '10 10 0 10',
        margin: '10 8 15 15',
        flex: 0,

        dockedItems: [{
            xtype: 'toolbar',
            ui: 'light-gray-toolbar',
            cls: 'directorygrid-toolbar',
            dock: 'right',

            itemId: 'filtertoolbar',

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
                cls: 'tr-radius-button',
                glyph: 0xf13d,
                glyph1: 0xf13e,
                target: function () {
                    return this.up('toolbar');
                }
            }, {
                itemId: 'efselectionmodelbutton',
                glyph: 0xf16b,
                text: l10n.ns('core', 'filter').value('selectionFilter'),
                tooltip: l10n.ns('core', 'filter').value('selectionFilter')
                },
                //{
                //itemId: 'eftextmodelbutton',
                //glyph: 0xf60e,
                //text: l10n.ns('core', 'filter').value('textFilter'),
                //tooltip: l10n.ns('core', 'filter').value('textFilter')
                //}, '-',
                {
                itemId: 'efsettingsbutton',
                glyph: 0xf493,
                text: l10n.ns('core', 'filter', 'buttons').value('settings'),
                tooltip: l10n.ns('core', 'filter', 'buttons').value('settings')
            }, '-']

        }]
    }]
});