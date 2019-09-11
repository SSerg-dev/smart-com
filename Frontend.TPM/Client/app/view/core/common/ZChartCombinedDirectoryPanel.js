﻿Ext.define('App.view.core.common.ChartCombinedDirectoryPanel', {
    extend: 'App.view.core.common.CombinedDirectoryPanel',
    alias: 'widget.chartcombineddirectorypanel',
    layout: 'fit',
    minHeight: 145,

    dockedItems: [],
    customHeaderItems: [],

    systemHeaderItems: [{
        xtype: 'expandbutton',
        glyph: 0xf063,
        glyph1: 0xf04b,
        itemId: 'collapse',
        tooltip: l10n.ns('core', 'selectablePanelButtons').value('collapse'),
        target: function () {
            return this.up('combineddirectorypanel');
        }
    }]
});