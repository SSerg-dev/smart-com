Ext.define('App.view.tpm.demandpricelist.HistoricalDemandPriceListDetail', {
    extend: 'App.view.core.common.EditorDetailWindow',
    alias: 'widget.historicaldemandpricelistdetail',
    width: 800,
    minWidth: 800,
    maxHeight: 600,

    items: {
        xtype: 'editorform',
        itemId: 'historicaldetailform',
        items: [{
            xtype: 'singlelinedisplayfield',
            name: '_User',
            fieldLabel: l10n.ns('tpm', 'HistoricalDemandPriceList').value('_User')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Role',
            fieldLabel: l10n.ns('tpm', 'HistoricalDemandPriceList').value('_Role')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_EditDate',
            renderer: Ext.util.Format.dateRenderer('d.m.Y H:i:s'),
            fieldLabel: l10n.ns('tpm', 'HistoricalDemandPriceList').value('_EditDate')
        }, {
            xtype: 'singlelinedisplayfield',
            name: '_Operation',
            renderer: App.RenderHelper.getLocalizedRenderer('tpm.HistoricalDemandPriceList', 'OperationType'),
            fieldLabel: l10n.ns('tpm', 'HistoricalDemandPriceList').value('_Operation')
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'ProductZREP',
            fieldLabel: l10n.ns('tpm', 'DemandPriceList').value('ProductZREP'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'DemandCode',
            fieldLabel: l10n.ns('tpm', 'DemandPriceList').value('ClientTreeDemandCode'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'StartDate',
            fieldLabel: l10n.ns('tpm', 'DemandPriceList').value('StartDate'),
            renderer: Ext.util.Format.dateRenderer('d.m.Y'),
        }, {
            xtype: 'singlelinedisplayfield',
            name: 'Price',
            fieldLabel: l10n.ns('tpm', 'DemandPriceList').value('Price'),
        }]
    }
});
